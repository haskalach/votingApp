namespace Application.API.Models {
    public class VotingYears {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public int VoterId { get; set; }
        public Voter Voter { get; set; }
        public string Year { get; set; }
    }
}