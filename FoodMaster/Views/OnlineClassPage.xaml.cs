using System;
using MediaManager;
using Xamarin.Forms;

namespace FoodMaster.Views
{
    public partial class OnlineClassPage : ContentPage
    {
        public OnlineClassPage()
        {
            InitializeComponent();
            CrossMediaManager.Current.Notification.Enabled = false;
            CrossMediaManager.Current.Notification.ShowNavigationControls = false;
            CrossMediaManager.Current.Notification.ShowPlayPauseControls = true;
            CrossMediaManager.Current.MediaPlayer.ShowPlaybackControls = false;
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
