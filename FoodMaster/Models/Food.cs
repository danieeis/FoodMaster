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

    public class TipDTO
    {
        public int Index { get; set; }
        
        public string Text { get; set; }

        public string Icon
        {
            get
            {
                switch (Index)
                {
                    case 0:
                        return "hands.png";
                    case 1:
                        return "book.png";
                    case 2:
                        return "phone.png";
                    default:
                        return "book.png";
                }
            }
        }
    }

    public class PortionDTO
    {
        public string Title { get; set; }

        public string Value { get; set; }
    }

    public class PortionType
    {
        public string Key { get; set; }

        public List<string> Values { get; set; }

        public string Icon
        {
            get
            {
                switch (Key)
                {
                    case "1_porcion":
                        return "person.png";
                    default:
                        return "bi_person.png";
                }
            }
        }

        public string DisplayValue
        {
            get
            {
                switch (Key)
                {
                    case "1_porcion": return "1 persona";
                    case "2_porcion": return "2 personas";
                    case "3_porcion": return "3 personas";
                    case "4_porcion": return "4 personas";
                    case "5_porcion": return "5 personas";
                    case "6_porcion": return "6 personas";
                    case "7_porcion": return "7 personas";
                    case "8_porcion": return "8 personas";
                    case "9_porcion": return "9 personas";
                    case "10_porcion": return "10 personas";
                    default:
                        return "Porciones";
                }
            }
        }
    }
}
