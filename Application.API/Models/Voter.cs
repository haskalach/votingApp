using System;

namespace Application.API.Models
{
    public class Voter
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string FirstName { get; set; }
        public string Speciality { get; set; }
        public string SubChapter { get; set; }
        public string Religion { get; set; }
        public string Politic { get; set; }
        public string Reference { get; set; }
        public string VotedYear { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthCountry { get; set; }
        public string BirthPlace { get; set; }
        public string CivilIdMother { get; set; }
        public string CivilIdKad { get; set; }
        public string CivilIdRegion { get; set; }
        public string RegisteryNumber { get; set; }
        public string CivilIdPlace { get; set; }
        public DateTime Registration { get; set; }
        public DateTime Graduation { get; set; }
        public string School { get; set; }
        public string GraduationLocation { get; set; }
        public string AddressWork { get; set; }
        public string MobileWork { get; set; }
        public string PhoneWork { get; set; }
        public string AddressHome { get; set; }
        public string MobileHome { get; set; }
        public string PhoneHome { get; set; }
        public string Email { get; set; }
        public int Vote { get; set; }
        public int Attend { get; set; }
        public int Transport { get; set; }
        public int Voted { get; set; }

    }
}