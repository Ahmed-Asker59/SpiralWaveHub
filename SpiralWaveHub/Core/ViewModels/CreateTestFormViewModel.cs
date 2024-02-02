namespace SpiralWaveHub.Core.ViewModels
{
    public class CreateTestFormViewModel
    {
        public int Id { get; set; }
        public int PatientId { get; set; }

        [Display(Name = "Test Picture")]
        public IFormFile TestPic { get; set; } = null!;

        [Display(Name = "Time Taken")]
        [AssertThat("TestDate <= Today()", ErrorMessage = Errors.NotAllowedDate)]
        public DateTime TestDate { get; set; } = DateTime.Now;

        [Display(Name = "Test Type")]
        public int TestTypeId { get; set; }
        public IEnumerable<SelectListItem>? TestTypes { get; set; }
    }
}
