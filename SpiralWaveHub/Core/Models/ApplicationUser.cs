using Microsoft.AspNetCore.Identity;

namespace SpiralWaveHub.Core.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class ApplicationUser:IdentityUser
    {
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [MaxLength(50)] 
        public string LastName { get; set; } = null!;

        public ICollection<Patient> Patients { get; set; } = new List<Patient>();


    }
}
