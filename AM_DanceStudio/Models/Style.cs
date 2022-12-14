using System.ComponentModel.DataAnnotations;

namespace AM_DanceStudio.Models
{
    public class Style
    {
        [Key]

        public int Id { get; set; }
        [Required(ErrorMessage = "Numele stilului este obligatoriu")]
    
        public string Name { get; set; }

        public virtual ICollection<Class> Classes { get; set; } 
    }

}
