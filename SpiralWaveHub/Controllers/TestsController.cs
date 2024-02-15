using SixLabors.ImageSharp;
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TestsController(ApplicationDbContext context, IMapper mapper, IImageService imageService, IDiagnoseService diagnoseService, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _imageService = imageService;
            _diagnoseService = diagnoseService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index(int patientId)
        {
            var allTests = _context.Tests
                .Include(t => t.TestType).Where(t => t.PatientId == patientId).
                OrderByDescending(t => t.Id).ToList();

            var testViewModel = _mapper.Map<IEnumerable<TestViewModel>>(allTests);
            var viewModel = new TestDetailsViewModel
            {
                Tests = testViewModel,
                PatientId = patientId
            };
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create(int patientId)
        {

            var viewModel = PopulateTestViewModel(null, patientId);

            return View("TestForm", viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TestFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("TestForm", PopulateTestViewModel(null,model.PatientId));

            var patient = _context.Patients.Find(model.PatientId);
            if (patient is null)
                return NotFound();

            var test = _mapper.Map<Test>(model);
            var fileName = Path.GetExtension(model.TestPic!.FileName);
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
            patient.NumberOfTests += 1;
            patient.LastTestDate = model.TestDate;
            
            _context.Tests.Add(test);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index), new {patientId =  model.PatientId});
        }

        [HttpGet]
        public  IActionResult Edit(int id)
        {
            
            var test = _context.Tests.Find(id);

            if (test is null)
                return NotFound();

            var viewModel = _mapper.Map<TestFormViewModel>(test);

            viewModel = PopulateTestViewModel(viewModel);
            return View("TestForm",viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TestFormViewModel model)
        {

            var test = _context.Tests.Find(model.Id);
            if (test is null)
                return NotFound();
            var patient = _context.Patients.Find(model.PatientId);
           
            if (patient is null)
                return NotFound();

            if(model.TestPic is not null)
            {
                var imgExtension = Path.GetExtension(model.TestPic.FileName);
                var imageName = $"{Guid.NewGuid()}{imgExtension}";
                var imagePath = "/images/tests";

                var (isUploaded, errorMessage) = await _imageService.UploadAsync(model.TestPic, imageName, imagePath);


                model.PicPath = $"{imagePath}/{imageName}";
                model.ThumbNailPath = $"{imagePath}/thumb/{imageName}";
                var picStream = new MemoryStream();
                model.TestPic.CopyTo(picStream);
                var testTypeName = _context.TestTypes.Where(t => t.Id == model.TestTypeId).Select(t => t.Name);
                test.Diagnosis = await _diagnoseService.Diagnose(picStream, testTypeName.ToString()!, imgExtension);
                patient.LastDiagnosis = test.Diagnosis;

                patient.LastTestDate = model.TestDate;
            }
            else
            {
                model.PicPath = test.PicPath;
                model.ThumbNailPath = test.ThumbNailPath;
                if (model.TestTypeId != test.TestTypeId)
                {
                    var picStream = _imageService.GetImageInStream(model.PicPath);
                    
                    var imgExtension = Path.GetExtension(model.PicPath);
                    var testTypeName = _context.TestTypes.Where(t => t.Id == model.TestTypeId).Select(t => t.Name);
                    test.Diagnosis = await _diagnoseService.Diagnose(picStream, testTypeName.ToString()!, imgExtension);
                    patient.LastDiagnosis = test.Diagnosis;

                    patient.LastTestDate = model.TestDate;

                        
                 }
                     
                    
                   
                }
            

           
            
            test = _mapper.Map(model, test);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index), new { patientId = model.PatientId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var test = _context.Tests.Find(id);
            if (test is null)
                return NotFound();
            var patientId = test.PatientId;
            _imageService.DeleteImage(test.PicPath, test.ThumbNailPath);
 
            _context.Tests.Remove(test);
            _context.SaveChanges();

            //update patient with latest test

           var numPatients =  UpdatePatient(patientId);

            
            return Ok(numPatients);
        }

        private int UpdatePatient(int  patientId)
        {
           var patient =  _context.Patients.Find(patientId);
           var test = _context.Tests.OrderBy(
              t => t.Id ).LastOrDefault();
            if (test is not null)
            {
                patient!.LastDiagnosis = test.Diagnosis;
                patient.LastTestDate = test.TestDate;
                patient.NumberOfTests -= 1;
            }

            //if we get there, that means there is not tests
            else
            {
                patient!.LastDiagnosis = null;
                patient.LastTestDate = null;
                patient.NumberOfTests = 0;
            }

            _context.SaveChanges();

            return patient.NumberOfTests;
        }
        private TestFormViewModel PopulateTestViewModel(TestFormViewModel? model = null, int patientId = 0)
        {
            var viewModel = model is null ? new TestFormViewModel() : model;


            viewModel.TestTypes = _mapper.Map<IEnumerable<SelectListItem>>(_context.TestTypes.ToList());
            
            if(patientId != 0 )
                viewModel.PatientId = patientId;

            return viewModel;
        }
    }
}
