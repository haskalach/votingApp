using System;
using System.Collections.Generic;
using Application.API.Models;

namespace Application.API.Dtos {
    public class UserForListDto {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string KnowAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string PhotoUrl { get; set; }
        public int OrganizationId { get; set; }
        public ICollection<UserRoleForReturnDto> UserRoles { get; set; }
    }
}