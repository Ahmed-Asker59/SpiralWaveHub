using Microsoft.AspNetCore.Hosting;
using System.Text.Encodings.Web;

namespace SpiralWaveHub.Services
{
    public class EmailBodyBuilder : IEmailBodyBuilder
    {
        private readonly IWebHostEnvironment _webHostEnvironement;

        public EmailBodyBuilder(IWebHostEnvironment webHostEnvironement)
        {
            _webHostEnvironement = webHostEnvironement;
        }

        public string GetEmailBody(string imageUrl, string header, string body, string url, string linkTitle)
        {
            var tempPath = $"{_webHostEnvironement.WebRootPath}/templates/email.html";
            StreamReader str = new StreamReader(tempPath);
            var template = str.ReadToEnd();
            str.Close();
            return template.Replace("[ImageUrl]", imageUrl)
                .Replace("[Header]", header)
                .Replace("[Body]", body)
                .Replace("[url]", url)
                .Replace("[linkTitle]", linkTitle);
        }
    }
}
