using System.ComponentModel.DataAnnotations;

namespace AM_DanceStudio.Models
{
    public class Review
    {
        [Key]   
        public int Id { get; set; }
        [Required(ErrorMessage = "Continutul este obligatoriu")]
        public string Text { get; set; }
        
        public DateTime Date { get; set; }  

        public int ClassId { get; set; }

        public string? UserId { get; set; }
        public virtual Class Class { get; set; }

        public virtual ApplicationUser? User { get; set; }
    }
}
