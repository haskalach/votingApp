using System.Collections.Generic;

namespace Application.API.Models
{
    public class Organization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
        public int VoterTypeId { get; set; }
        public VoterType VoterType { get; set; }
    }
}