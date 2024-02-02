namespace SpiralWaveHub.Services
{
    public class DiagnoseService:IDiagnoseService
    {
        private const string spiralTestUrl  = "https://parkinsons-disease-detection-88jk.onrender.com/predictspiralsApi";
        private const string waveTestUrl  = "https://parkinsons-disease-detection-88jk.onrender.com/predictwavesApi";
        public async Task<string> Diagnose(MemoryStream pic, string testType, string fileName)
        {
            var targetUrl = spiralTestUrl;
            var apiKey = "spiral";
            if (testType == AppTestTypes.Wave)
            {
                targetUrl = waveTestUrl;
                apiKey = "wave";
            }

            HttpClient client = new HttpClient();
            using var form = new MultipartFormDataContent();

            var imageContent = new ByteArrayContent(pic.ToArray());
            form.Add(imageContent, apiKey, fileName);
            
           
            var response = await client.PostAsync(targetUrl, form);
            var result = await response.Content.ReadAsStringAsync();

            if (result.ToString().Contains("parkinson"))
                return AppTestResults.ParkinsonResult;

            return AppTestResults.HealthyResult;
           
        }
    }
}
