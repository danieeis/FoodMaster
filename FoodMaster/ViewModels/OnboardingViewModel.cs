using System;
using System.Collections.Generic;
using FoodMaster.Models;
using FoodMaster.Services;
using Xamarin.Forms;

namespace FoodMaster.ViewModels
{
    public class OnboardingViewModel : BaseViewModel
    {
        public List<Onboarding> Onboardings { get; set; }
        public Command GetStartedCommand { get; }

        UserService _userService;

        public OnboardingViewModel()
        {
            _userService = DependencyService.Get<UserService>();
            Onboardings = new List<Onboarding>()
            {
                new Onboarding(){ Title = "¿Como funciona?", Subtitle = "1. Escoge tus recetas", Text = "Tendrás la opción de elegir la receta que más se adecúe a lo que deseas, entre la gran cantidad de opciones separadas por categorias.", Image = "onboarding_1.gif", IsAnimated = true },
                new Onboarding(){ Title = "¿Como funciona?", Subtitle = "2. Recibe tu box", Text = "Te llevamos a casa un box que contiene todos los ingredientes necesarios para la preparacion de tu receta.", Image = "onboarding_2.gif", IsAnimated = true },
                new Onboarding(){ Title = "¿Como funciona?", Subtitle = "3. Cocina y disfruta", Text = "Descubre tu potencial y prepara las recetas más interesantes que puedas encontrar con nosotros. Olvídate de las cantidades, te enviamos las medidas exactas para que tus comidas sean iguales a la receta.", Image = "onboarding_3.png" },
            };
            GetStartedCommand = new Command(GetStartedClicked);
        }

        private void GetStartedClicked()
        {
            _userService.PassThroughOnboarding = true;
            App.Current.MainPage = new AppShell();
        }

    }
}
