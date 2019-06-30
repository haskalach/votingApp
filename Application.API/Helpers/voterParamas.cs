namespace Application.API.Helpers {
    public class VoterParams {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        public int voterTypeId { get; set; } = 0;
        private int pageSize = 10;
        public string religion { get; set; }
        public string politic { get; set; }
        public int code { get; set; }
        public string firstNameArabic { get; set; }
        public string familyArabic { get; set; }
        public string fatherNameArabic { get; set; }
        public string subChapter { get; set; }
        public bool? contacted { get; set; }
        public bool? voted { get; set; }
        public bool? attend { get; set; }
        public bool? abroad { get; set; }
        public string reiligion { get; set; }
        public string school { get; set; }
        public string civilIdMouhavaza { get; set; }
        public string civilIdKadaa { get; set; }
        public string civilIdRegion { get; set; }
        public string civilIdPlace { get; set; }
        public int PageSize {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

    }
}