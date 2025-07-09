
using System.ComponentModel.DataAnnotations;

namespace userRegistration.Models
{
    public class City
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        public int StateId { get; set; }
        public State State { get; set; }
    }
}

