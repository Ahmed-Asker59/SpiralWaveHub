namespace SpiralWaveHub.Core.ViewModels
{
    public class TestViewModel
    {
        public int Id { get; set; }
      
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
