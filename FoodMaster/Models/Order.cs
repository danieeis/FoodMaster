using System;
namespace FoodMaster.Models
{
    public class Order
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Portion { get; set; }
        public string OrderAt { get; set; }
    }
}
