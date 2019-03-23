using System.ComponentModel.DataAnnotations;

namespace Application.API.Dtos {
    public class ItemForCreationDto {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public double Price { get; set; }
    }
}