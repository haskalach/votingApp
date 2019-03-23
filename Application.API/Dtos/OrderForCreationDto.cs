using System;
using System.Collections.Generic;
using System.Linq;
using Application.API.Models;

namespace Application.API.Dtos {
    public class OrderForCreationDto {
        public ICollection<OrderItemForCreationDto> OrderItems { get; set; }
        public double TotalPrice { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public OrderForCreationDto () {
            CreatedAt = DateTime.Now;
            Status = "new";
            // TotalPrice = OrderItems.Sum (x => x.TotalPrice);
        }
    }
}