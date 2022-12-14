using System.ComponentModel.DataAnnotations;

namespace AM_DanceStudio.Models
{
    public class Studio
    {
        [Key]

        public int Id { get; set; }
        [Required(ErrorMessage = "Numele studioului este obligatoriu")]

        public string Name { get; set; }

        public string Bio { get; set; }

        public virtual ICollection<Class> Classes { get; set; }

    }
}
