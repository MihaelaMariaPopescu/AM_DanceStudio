using System.ComponentModel.DataAnnotations;

namespace AM_DanceStudio.Models
{
    public class Review
    {
        [Key]   
        public int Id { get; set; }
        [Required(ErrorMessage = "Continutul este obligatoriu")]
//MIHA
        public string Text { get; set; }
        
        public DateTime Date { get; set; }  

        public int ClassId { get; set; }

        public virtual Class Class { get; set; }
    }
}
