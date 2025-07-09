using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using userRegistration.Models.Enums;

namespace userRegistration.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(25)]
        public string Name { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required, DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        [Display(Name = "Contact Number")]
        public string? ContactNumber { get; set; }
        public string? MobileNumber { get; set; }


        [Required]
        public int StateId { get; set; }

        [ValidateNever]
        public State State { get; set; }

        [Required]
        public int? CityId { get; set; }

        [ValidateNever]
        public City City { get; set; }

        public List<string>? Hobbies { get; set; }

    

        public string? PhotoBlob { get; set; } // For storing image as BLOB

        public bool IsTermsAccepted { get; set; }
    }
}
