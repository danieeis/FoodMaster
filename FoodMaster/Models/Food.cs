using System;
using System.Collections.Generic;

namespace FoodMaster.Models
{
    public class Food
    {
        public string Id { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Level { get; set; }
        public string Timing { get; set; }
        public string[] Tips { get; set; }
        public string[] Preparation { get; set; }
        public Dictionary<string,string> NutritionalValue { get; set; }
        public Dictionary<string, string> Ingredients { get; set; }
        public string DocumentPath { get; set; }
    }
}
