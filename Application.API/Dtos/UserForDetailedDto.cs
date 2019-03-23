using System;
using System.Collections.Generic;
using Application.API.Models;

namespace Application.API.Dtos {
    public class UserForDetailedDto {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string KnowAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string PhotoUrl { get; set; }
        public ICollection<PhotosForDetailedDto> Photos { get; set; }

        public ICollection<OrderForReturnDto> Orders { get; set; }
    }
}