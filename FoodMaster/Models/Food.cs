using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

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

    public class PortionType : INotifyPropertyChanged
    {
        public string Key { get; set; }

        public List<string> Values { get; set; }

        Color _selectedColor = Color.FromHex("#4B4A4A");
        public Color SelectedColor
        {
            get => _selectedColor;
            set => SetProperty(ref _selectedColor, value);
        }

        string _icon;
        public string Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }

        public int Index
        {
            get
            {
                return Key?.Length > 0 ? Key[0] : 0;
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

        public PortionType()
        {
            Icon = GetIcon();
        }

        public string GetIcon(bool selected = false)
        {
            string icon;
            switch (Key)
            {
                case "1_porcion":
                    icon = "person";
                    break;
                default:
                    icon = "bi_person";
                    break;
            }

            if (selected) icon += "_selected";

            return icon + ".png";
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
