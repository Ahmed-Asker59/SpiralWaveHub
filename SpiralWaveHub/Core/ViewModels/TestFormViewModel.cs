namespace SpiralWaveHub.Core.ViewModels
{
    public class TestFormViewModel
    {
        public int Id { get; set; }
        public int PatientId { get; set; }

        [Display(Name = "Test Picture")]
        [RequiredIf("Id == 0")]
        public IFormFile? TestPic { get; set; }

        [Display(Name = "Time Taken")]
        [AssertThat("TestDate <= Today()", ErrorMessage = Errors.NotAllowedDate)]
        public DateTime TestDate { get; set; } = DateTime.Now;

        [Display(Name = "Test Type")]
        public int TestTypeId { get; set; }
        public IEnumerable<SelectListItem>? TestTypes { get; set; }
        public string?  PicPath { get; set; }
        public string? ThumbNailPath { get; set; }
    }
}
