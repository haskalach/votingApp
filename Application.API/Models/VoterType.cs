using System.Collections.Generic;

namespace Application.API.Models {
    public class VoterType {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Voter> Voters { get; set; }
        public ICollection<Organization> Organizations { get; set; }
    }
}