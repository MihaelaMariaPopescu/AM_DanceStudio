using System.ComponentModel.DataAnnotations;

namespace AM_DanceStudio.Models
{
    public class Instructor
    {
        [Key]

        public int Id { get; set; }

        [Required(ErrorMessage = "Numele instructorului este obligatoriu")]

        public string Name { get; set; }

        public int Age { get; set; }    

        public string Picture { get; set; }

        public string Bio { get; set; }

    }
}
