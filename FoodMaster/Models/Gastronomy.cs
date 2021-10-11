using System;
namespace FoodMaster.Models
{
    public class Gastronomy
    {
        public string Id { get; set; }
        public int Order { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string DocumentPath { get; set; }
    }
}
