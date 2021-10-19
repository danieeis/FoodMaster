using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace FoodMaster.Views
{
    public partial class OnlineClassPage : ContentPage, IDisposable
    {
        public OnlineClassPage()
        {
            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            videoPlayer?.Stop();
        }

        public void Dispose()
        {
            videoPlayer?.Stop();
        }
    }
}
