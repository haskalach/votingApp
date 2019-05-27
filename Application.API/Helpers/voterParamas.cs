namespace Application.API.Helpers {
    public class VoterParams {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        public int voterTypeId { get; set; } = 0;
        private int pageSize = 10;
        public string religion { get; set; }
        public string politic { get; set; }
        public int PageSize {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

    }
}