using Application.API.Data;
using Application.API.Models;
using AutoMapper;

namespace Application.API.Dtos {
    public class OrderItemForCreationDto {
        public int ItemId { get; set; }
        // public ItemToReturnDto Item { get; set; }
        public int Count { get; set; }
        public int OrderId { get; set; }
        public double TotalPrice { get; set; }

    }
}