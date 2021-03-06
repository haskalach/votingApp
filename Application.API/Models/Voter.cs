using System;
using System.Collections.Generic;

namespace Application.API.Models {
    public class Voter {
        public int Id { get; set; }
        public int Code { get; set; }
        public string FirstNameArabic { get; set; }
        public string FatherNameArabic { get; set; }
        public string FamilyArabic { get; set; }
        public string FirstName { get; set; }
        public string FatherName { get; set; }
        public string Family { get; set; }
        public string Nationality { get; set; }
        public string Speciality { get; set; }
        public string SubChapter { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthCountry { get; set; }
        public string BirthPlace { get; set; }
        public string CivilIdMouhavaza { get; set; }
        public string CivilIdKadaa { get; set; }
        public string CivilIdRegion { get; set; }
        public string RegisteryNumber { get; set; }
        public string CivilIdPlace { get; set; }
        public DateTime Registration { get; set; }
        public string LastCoveredYear { get; set; }
        public DateTime Graduation { get; set; }
        public string School { get; set; }
        public string GraduationCountry { get; set; }
        public string AddressWork { get; set; }
        public string MobileWork { get; set; }
        public string PhoneWork { get; set; }
        public string AddressHome { get; set; }
        public string MobileHome { get; set; }
        public string PhoneHome { get; set; }
        public string Email { get; set; }
        public string Religion { get; set; }
        public string Politic { get; set; }
        public bool Attend { get; set; }
        public bool Contacted { get; set; }
        public bool Abroad { get; set; }
        public int? ReferenceId { get; set; }
        public User Reference { get; set; }
        public int VoterTypeId { get; set; }
        public VoterType VoterType { get; set; }
        public ICollection<VotingYears> VotingYears { get; set; }
    }
}