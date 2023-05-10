﻿namespace MVC.Models.Requests
{
    public class BasketTotal
    {
        public decimal TotalCost { get; set; } = 0;
        public List<BasketItem> BasketList { get; set; } = new List<BasketItem>();
    }
}
