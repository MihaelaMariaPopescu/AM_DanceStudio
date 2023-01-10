using System.ComponentModel.DataAnnotations;

namespace AM_DanceStudio.Models
{
    public class Item
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }  

        public int Total { get { return (int)(Quantity * Price); } }

        public Item()
        {}

        public Item(Class cls)
        {
           Id=cls.Id;
            Name=cls.Name;
            Price = cls.Price;
            Quantity = 1;
        }

    }
}