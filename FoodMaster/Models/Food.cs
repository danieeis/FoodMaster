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
        public List<string> Tips { get; set; }
        public List<string> Preparation { get; set; }
        public Dictionary<string, string> NutritionalValue { get; set; }
        public Dictionary<string, List<string>> Ingredients { get; set; }
        public string DocumentPath { get; set; }
    }
}
