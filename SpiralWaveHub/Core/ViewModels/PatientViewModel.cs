using System.ComponentModel.DataAnnotations;

namespace SpiralWaveHub.Core.ViewModels
{
    public class PatientViewModel
    {
        public int Id { get; set; }

        
        public string FullName { get; set; } = null!;

        public int Age { get; set; }

        
        public string? Email { get; set; }
        public int NumberOfTests { get; set; } = 0;

    }
}
