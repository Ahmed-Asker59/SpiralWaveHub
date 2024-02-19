using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SpiralWaveHub.Core.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class Patient
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string FullName { get; set; } = null!;

        public int Age { get; set; }

        [MaxLength(150)]
        public string? Email { get; set; }
        public int NumberOfTests { get; set; } = 0;

        public string? LastDiagnosis { get; set; }
        public DateTime? LastTestDate { get; set; }

        public ICollection<Test> Tests { get; set; } = new List<Test>();

        public string ApplicationUserId { get; set; } = null!;
        public ApplicationUser ApplicationUser { get; set; } = null!;
    }
}
