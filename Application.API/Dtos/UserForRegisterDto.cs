using System;
using System.ComponentModel.DataAnnotations;

namespace Application.API.Dtos {
    public class UserForRegisterDto {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength (8, MinimumLength = 4, ErrorMessage = "You must specify password betwen 4 and 8 characters")]
        public string Password { get; set; }
        public string Gender { get; set; }
        public string KnowAs { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }

        public UserForRegisterDto () {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }
    }
}