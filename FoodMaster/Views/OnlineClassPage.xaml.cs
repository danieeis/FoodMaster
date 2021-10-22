using System;
using MediaManager;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodMaster.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OnlineClassPage : ContentPage
    {
        public OnlineClassPage()
        {
            InitializeComponent();
            CrossMediaManager.Current.Notification.Enabled = true;
            CrossMediaManager.Current.Notification.ShowNavigationControls = false;
            CrossMediaManager.Current.Notification.ShowPlayPauseControls = true;
            CrossMediaManager.Current.MediaPlayer.ShowPlaybackControls = true;
            CrossMediaManager.Current.Init();
        }

        protected async override void OnDisappearing()
        {
            base.OnDisappearing();
            await CrossMediaManager.Current.Stop();
            CrossMediaManager.Current.Dispose();
        }
    }
}
