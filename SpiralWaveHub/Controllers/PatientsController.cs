using SpiralWaveHub.Services;
using System.Linq.Dynamic.Core;

namespace SpiralWaveHub.Controllers
{
    public class PatientsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PatientsController(ApplicationDbContext context, IMapper mapper, IImageService imageService)
        {
            _context = context;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var patients = _context.Patients.ToList();
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
                patients = patients.Where(p => p.FullName.Contains(searchValue) || p.Email.Contains(searchValue) || p.LastDiagnosis.Contains(searchValue) );


            patients = patients.OrderBy($"{sortColumnName} {sortColumnDirection}");
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

       
    }
}
