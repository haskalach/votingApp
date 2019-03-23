using System;
using System.Collections.Generic;
using Application.API.Models;

namespace Application.API.Dtos {
    public class OrderForReturnDto {
        public ICollection<OrderItemForReturnDto> OrderItems { get; set; }
        public double TotalPrice { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}