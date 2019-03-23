using System.Collections.Generic;

namespace Application.API.Dtos {
    public class ItemToReturnDto {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string PhotoUrl { get; set; }
        public ICollection<PhotosForDetailedDto> Photos { get; set; }
        public double Price { get; set; }
    }
}