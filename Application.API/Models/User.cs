using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Application.API.Models {
    public class User: IdentityUser<int> {

        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Name { get; set; }  
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }    
        public string Introduction { get; set; }    
        public string LookingFor { get; set; }  
        public string Interests { get; set; }
        public string city { get; set; }
        public string country { get; set; } 
        public ICollection<Photo> Photos { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public int? OrganizationId { get; set; }
        public Organization Organization { get; set; }

    }
}