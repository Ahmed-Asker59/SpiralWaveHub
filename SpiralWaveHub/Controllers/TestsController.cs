

using Microsoft.EntityFrameworkCore;
using SpiralWaveHub.Services;

namespace SpiralWaveHub.Controllers
{
    public class TestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private readonly IDiagnoseService _diagnoseService;

        public TestsController(ApplicationDbContext context, IMapper mapper, IImageService imageService, IDiagnoseService diagnoseService)
        {
            _context = context;
            _mapper = mapper;
            _imageService = imageService;
            _diagnoseService = diagnoseService;
        }

        [HttpGet]
        public IActionResult Create(int patientId)
        {

            var viewModel = PopulateTestViewModel(patientId);

            return View("CreateTestForm", viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTestFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("CreateTestForm", PopulateTestViewModel(model.PatientId));

            var patient = _context.Patients.Find(model.PatientId);
            if (patient is null)
                return NotFound();

            var test = _mapper.Map<Test>(model);
            var fileName = Path.GetExtension(model.TestPic.FileName);
            var imageName = $"{Guid.NewGuid()}{fileName}";
            var imagePath = "/images/tests";

            var (isUploaded, errorMessage) = await _imageService.UploadAsync(model.TestPic, imageName, imagePath);

           
            test.PicPath = $"{imagePath}/{imageName}";
            test.ThumbNailPath = $"{imagePath}/thumb/{imageName}";
            var picStream = new MemoryStream();
            model.TestPic.CopyTo(picStream);
            var testTypeName = _context.TestTypes.Where(t=> t.Id == model.TestTypeId).Select(t => t.Name);
            test.Diagnosis = await _diagnoseService.Diagnose(picStream, testTypeName.ToString()!, fileName);
            patient.LastDiagnosis = test.Diagnosis;
            
            _context.Tests.Add(test);
            _context.SaveChanges();
            return Ok();
        }

        private CreateTestFormViewModel PopulateTestViewModel(int patientId)
        {
            var viewModel = new CreateTestFormViewModel()
            {
                
                PatientId = patientId,
                TestTypes = _mapper.Map<IEnumerable<SelectListItem>>(_context.TestTypes.ToList())
            };

            return viewModel;
        }
    }
}
