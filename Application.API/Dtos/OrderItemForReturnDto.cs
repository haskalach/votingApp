using Application.API.Models;

namespace Application.API.Dtos {
    public class OrderItemForReturnDto {
        public int ItemId { get; set; }
        public int Count { get; set; }
        public double TotalPrice { get; set; }
    }   
}



