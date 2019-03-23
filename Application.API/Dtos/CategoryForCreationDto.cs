using System.ComponentModel.DataAnnotations;

namespace Application.API.Dtos
{
    public class CategoryForCreationDto
    {
        [Required]
        public string Name { get; set; }
        public int? ParentId { get; set; }
    }
}