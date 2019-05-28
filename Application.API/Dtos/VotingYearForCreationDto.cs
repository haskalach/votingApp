namespace Application.API.Dtos
{
    public class VotingYearForCreationDto
    {
        public int OrganizationId { get; set; }
        public int VoterId { get; set; }
        public string Year { get; set; }
    }
}