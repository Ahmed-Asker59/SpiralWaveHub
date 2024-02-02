using System.ComponentModel.DataAnnotations;

namespace SpiralWaveHub.Core.Models
{
    public class Test
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;
        public int TestTypeId { get; set; }
        public TestType TestType { get; set; } = null!;

        [MaxLength(20)]
        public string Diagnosis { get; set; } = null!;
        public DateTime TestDate { get; set; }

        [MaxLength(500)]
        public string PicPath { get; set; } = null!;

        [MaxLength(500)]
        public string ThumbNailPath { get; set; } = null!;

     
    }
}
