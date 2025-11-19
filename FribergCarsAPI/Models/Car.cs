using System.ComponentModel.DataAnnotations;

namespace FribergCars.Models
{
    public class Car
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Insert Brand")]
        public string Brand { get; set; }
        
        [Required(ErrorMessage = "Insert Model")]
        public string Model { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Atleast one Image is required")]
        public string ImageUrl { get; set; }
        public string ImageUrl2 { get; set; }
        public string ImageUrl3 { get; set; }
        
        public bool IsAvailable { get; set; }
    }
}
