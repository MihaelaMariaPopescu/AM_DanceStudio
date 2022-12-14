using System.ComponentModel.DataAnnotations;

namespace AM_DanceStudio.Models
{
    public class Class
    {
        [Key]

        public int Id { get; set; }
        [Required(ErrorMessage = "Numele este obligatoriu")]
        public string Name { get; set; }

        public string Description { get; set; }
        public string Picture { get; set; }
        [Required(ErrorMessage = "Stilul este obligatoriu")]

        public int StyleId { get; set; }

        public virtual Style Style { get; set; }

        public int InstructorId { get; set; }
        public virtual Instructor Instructor { get; set; }
        public int StudioId { get; set; }


        public virtual Studio Studio { get; set; }

        public double Price { get; set; }
        public string Rating { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }


    }
}
