using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AM_DanceStudio.Models
{
    public class Class
    {
        [Key]

        public int Id { get; set; }

        [Required(ErrorMessage = "Numele este obligatoriu")]
        [StringLength(100, ErrorMessage = "Numele nu poate avea mai mult de 100 de caractere")]
        [MinLength(5, ErrorMessage = "Numele trebuie sa aiba mai mult de 5 caractere")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Descrierea este obligatorie")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Imaginea este obligatorie")]
        public string Picture { get; set; }

        [Required(ErrorMessage = "Stilul este obligatoriu")]

        public int? StyleId { get; set; }

        public virtual Style? Style { get; set; }

        public int? InstructorId { get; set; }
        public virtual Instructor? Instructor { get; set; }

        [Required(ErrorMessage = "Studio-ul este obligatoriu")]
        public int? StudioId { get; set; }

        public virtual Studio? Studio { get; set; }
        public string? UserId { get; set; }

        public double Price { get; set; }
        public string Rating { get; set; }

        public bool Valid { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
        public virtual ApplicationUser? User { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? Stil { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? Ins { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? St { get; set; }


    }
}
