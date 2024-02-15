namespace SpiralWaveHub.Core.ViewModels
{
    public  class TestDetailsViewModel
    {
        public  IEnumerable<TestViewModel> Tests { get; set; } = null!;
        public  int PatientId { get; set; }

    }
}
