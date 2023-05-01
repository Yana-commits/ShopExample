﻿namespace Basket.Host.Models
{
    public class BasketItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
    }
}
