using System.ComponentModel.DataAnnotations;

namespace Application.API.Dtos {
    public class UserForReferenceRegisterDto {

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Name { get; set; }
    }
}