using System;
using MediaManager;
using Xamarin.Forms;

namespace FoodMaster.Views
{
    public partial class OnlineClassPage : ContentPage, IDisposable
    {
        public OnlineClassPage()
        {
            InitializeComponent();
            CrossMediaManager.Current.Notification.Enabled = false;
            CrossMediaManager.Current.Notification.ShowNavigationControls = false;
            CrossMediaManager.Current.Notification.ShowPlayPauseControls = false;
            CrossMediaManager.Current.MediaPlayer.ShowPlaybackControls = false;
            CrossMediaManager.Current.Init();
        }

        protected async override void OnDisappearing()
        {
            base.OnDisappearing();
            await CrossMediaManager.Current.Stop();
            videoPlayer?.Dispose();
            CrossMediaManager.Current.Dispose();
        }

        public void Dispose()
        {
            videoPlayer?.Dispose();
            CrossMediaManager.Current.Dispose();
        }
    }
}
