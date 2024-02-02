namespace SpiralWaveHub.Services
{
    public interface IDiagnoseService
    {
        Task<string> Diagnose(MemoryStream pic, string apiKey, string fileName);
    }
}
