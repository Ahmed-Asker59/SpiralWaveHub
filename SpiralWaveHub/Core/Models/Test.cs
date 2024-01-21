using System.ComponentModel.DataAnnotations;

namespace SpiralWaveHub.Core.Models
{
    public class Test
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string TestType { get; set; } = null!;
        [MaxLength(20)]
        public string Diagnosis { get; set; } = null!;
        public DateTime TestDate { get; set; }
        [MaxLength(500)]
        public string TestPicPath { get; set; } = null!;
        [MaxLength(500)]
        public string TestThumbNailPath { get; set; } = null!;

        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;
    }
}
