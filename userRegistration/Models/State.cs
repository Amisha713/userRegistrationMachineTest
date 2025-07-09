using System.ComponentModel.DataAnnotations;

namespace userRegistration.Models
{
    public class State
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        public ICollection<City> Cities { get; set; }
    }
}
