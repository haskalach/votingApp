using System.ComponentModel.DataAnnotations;

namespace Application.API.Dtos {
    public class UserForReferenceRegisterDto {
        [Required]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}