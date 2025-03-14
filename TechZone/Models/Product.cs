﻿namespace TechZone.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name   { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public int StockQuantity { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
    }
}
