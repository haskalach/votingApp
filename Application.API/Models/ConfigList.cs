using System.Collections.Generic;

namespace Application.API.Models {
    public class ConfigList {
        public ICollection<string> religion { get; set; }
        public ICollection<string> politics { get; set; }
        public ICollection<string> subChapter { get; set; }
        public ICollection<string> civilIdMouhavaza { get; set; }
        public ICollection<string> civilIdKadaa { get; set; }
        public ICollection<string> civilIdRegion { get; set; }
        public ICollection<string> civilIdPlace { get; set; }
    }
}