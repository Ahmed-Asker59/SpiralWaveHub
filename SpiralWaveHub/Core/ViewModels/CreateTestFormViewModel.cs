using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace SpiralWaveHub.Core.ViewModels
{
    public class CreateTestFormViewModel
    {
        public int Id { get; set; }
        public int PatientId { get; set; }

        [Display(Name = "Upload Test Picture")]
        public IFormFile TestPic { get; set; } = null!;

        [Display(Name = "Drawing Type")]
        public string TestType { get; set; } = null!;

        [Display(Name = "Time Taken")]
        public DateTime TestDate { get; set; }
       

    }
}
