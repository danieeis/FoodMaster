﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using FoodMaster.Interfaces;
using FoodMaster.Models;
using Xamarin.Forms;

namespace FoodMaster.ViewModels
{
    [QueryProperty("FoodId", "id")]
    public class FoodDetailViewModel : BaseViewModel, IQueryAttributable
    {
        double _collectionViewMaxHeight;
        public double CollectionViewMaxHeight
        {
            get => _collectionViewMaxHeight;
            set => SetProperty(ref _collectionViewMaxHeight, value);
        }
        
        string _category;
        public string Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }

        string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        string _level;
        public string Level
        {
            get => _level;
            set => SetProperty(ref _level, value);
        }

        string _timing;
        public string Timing
        {
            get => _timing;
            set => SetProperty(ref _timing, value);
        }

        string _image;
        public string Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        PortionType _portionSelected;
        public PortionType PortionSelected
        {
            get => _portionSelected;
            set
            {
                if (_portionSelected != value)
                {
                    SetProperty(ref _portionSelected, value);
                    ChangePortion(value);
                }
            }
        }

        IEnumerable<TipDTO> _tips;
        public IEnumerable<TipDTO> Tips
        {
            get => _tips;
            set => SetProperty(ref _tips, value);
        }

        IEnumerable<PortionDTO> _portions;
        public IEnumerable<PortionDTO> Portions
        {
            get => _portions;
            set => SetProperty(ref _portions, value);
        }

        List<string> _preparation;
        public List<string> Preparation
        {
            get => _preparation;
            set => SetProperty(ref _preparation, value);
        }

        ObservableCollection<PortionType> _availablePortions;
        public ObservableCollection<PortionType> AvailablePortions
        {
            get => _availablePortions;
            set => SetProperty(ref _availablePortions, value);
        }

        ObservableCollection<string> _ingredients;
        public ObservableCollection<string> Ingredients
        {
            get => _ingredients;
            set => SetProperty(ref _ingredients, value);
        }

        IRecipeService _recipeService;
        public Command<string> ChangePortionCommand { get; }
        public Command OpenWhatsappCommand { get; }

        public FoodDetailViewModel()
        {
            _recipeService = DependencyService.Get<IRecipeService>();
            ChangePortionCommand = new Command<string>(ChangePortion);
            OpenWhatsappCommand = new Command(OpenWhatsapp);
        }

        private async void OpenWhatsapp(object obj)
        {
            string text = HttpUtility.UrlEncode($"Estoy interesado en el plato {Name}");
            await Xamarin.Essentials.Browser.OpenAsync($"https://wa.me/584123079532?text={text}");
        }

        private void ChangePortion(object obj)
        {
            if (obj is PortionType portion)
            {
                Ingredients = new ObservableCollection<string>(portion.Values);
                foreach (var item in AvailablePortions)
                {
                    if(item.Key == PortionSelected.Key)
                    {
                        item.SelectedColor = Color.FromHex("#FB9F1C");
                        item.Icon = item.GetIcon(true);
                    }
                    else
                    {
                        item.SelectedColor = Color.FromHex("#4B4A4A");
                        item.Icon = item.GetIcon();
                    }
                    
                }
            }
        }

        public async void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            string id = HttpUtility.UrlDecode(query["id"]);
            Food food = await _recipeService.GetFood(id);
            Name = Title = food.Name;
            Category = food.Category;
            Level = food.Level;
            Timing = food.Timing;
            Image = food.Image;
            Tips = food.Tips.Select((text, i) => new TipDTO() { Index = i, Text = text });
            AvailablePortions = new ObservableCollection<PortionType>(food.Ingredients.Select((x) => new PortionType()
            {
                Key = x.Key,
                Values = x.Value
            }).OrderBy(x => x.Index));
            CollectionViewMaxHeight = AvailablePortions.Count() * 60;
            PortionType selectedPortions = AvailablePortions.FirstOrDefault();
            Ingredients = new ObservableCollection<string>(selectedPortions.Values);
            PortionSelected = selectedPortions;
            Portions = food.NutritionalValue.Select((x) => new PortionDTO() { Title = x.Key, Value = x.Value });
            Preparation = food.Preparation;
        }
    }
}
