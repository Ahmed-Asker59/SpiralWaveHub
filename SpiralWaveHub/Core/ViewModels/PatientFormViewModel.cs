using System.ComponentModel.DataAnnotations;

namespace SpiralWaveHub.Core.ViewModels
{
    public class PatientFormViewModel
    {
        public int? Id { get; set; }

        [MaxLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = null!;
        public int Age { get; set; }

        [EmailAddress]
        [MaxLength(150)]
        public string? Email { get; set; }
    }
}
