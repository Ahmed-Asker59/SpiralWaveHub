using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using SpiralWaveHub.Services;
using System.Linq.Dynamic.Core;
using System.Security.Claims;

namespace SpiralWaveHub.Controllers
{
    [Authorize]
    public class PatientsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private IEmailSender _mailSender;
        private readonly IWebHostEnvironment _webHostEnvironement;

        public PatientsController(ApplicationDbContext context, IMapper mapper, IImageService imageService, IEmailSender mailSender, IWebHostEnvironment webHostEnvironement)
        {
            _context = context;
            _mapper = mapper;
            _mailSender = mailSender;
            _webHostEnvironement = webHostEnvironement;
        }

        public  IActionResult Index()
        {
            
          
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var patients = _context.Patients.
                Where(p => p.ApplicationUserId == userId).ToList();
            var viewModel = _mapper.Map<IEnumerable<PatientViewModel>>(patients);
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_Form");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PatientFormViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var patient = _mapper.Map<Patient>(model);
            patient.ApplicationUserId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            _context.Patients.Add(patient);
            _context.SaveChanges();

            return Ok();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var patient = _context.Patients.Find(id);
            if (patient is null)
                return NotFound();
            var viewmodel = _mapper.Map<PatientFormViewModel>(patient);
            
            return PartialView("_Form", viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PatientFormViewModel model)
        {
            if(!ModelState.IsValid)
                return BadRequest(model);

            var patient = _context.Patients.Find(model.Id);

            if(patient is null)
                return NotFound();

            patient = _mapper.Map(model, patient);
            _context.SaveChanges();
             return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var patient = _context.Patients.Find(id);
            if(patient is null)
                return NotFound();

            _context.Patients.Remove(patient);
            _context.SaveChanges();
            return Ok();
        }

       

      

        [HttpPost]
        public IActionResult GetPatients()
        {
            //handle search with payload data
            int skipValue = int.Parse(Request.Form["start"]);
            int pageSize = int.Parse(Request.Form["length"]);

            //handle sorting
            //get the index of the selected column
            var sortColumnIndex = Request.Form["order[0][column]"];
            var sortColumnName = Request.Form[$"columns[{sortColumnIndex}][name]"];
            var sortColumnDirection = Request.Form["order[0][dir]"];

            //handle searching

            var searchValue = Request.Form["search[value]"];
            IQueryable<Patient> patients = _context.Patients;

            if (!string.IsNullOrEmpty(searchValue))
                patients = patients.Where(p => p.FullName.Contains(searchValue) || p.Email.Contains(searchValue) || p.LastDiagnosis.Contains(searchValue) || p.Age == int.Parse(searchValue));

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            patients = patients.Where(p => p.ApplicationUserId == userId)
                .OrderBy($"{sortColumnName} {sortColumnDirection}");
            var data = patients.Skip(skipValue).Take(pageSize).ToList();
            var recordsTotal = _context.Patients.Count();
            var jsonData = new
            {
                recordsFiltered = recordsTotal,
                recordsTotal,
                data
            };
            return Ok(jsonData);
        }


        public IActionResult AllowEmail(PatientFormViewModel model)
        {
            var patient = _context.Patients.SingleOrDefault(p => p.Email == model.Email);
            var isAllowed = patient is null || patient.Id.Equals(model.Id);

            return Json(isAllowed);
        }


    }
}
