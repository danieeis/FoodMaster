using System;
namespace FoodMaster.ViewModels
{
    public class OnlineClassViewModel : BaseViewModel
    {
        public string VideoSource { get; set; }
        public OnlineClassViewModel()
        {
            Title = "Clases online";
            VideoSource = "https://firebasestorage.googleapis.com/v0/b/recetapp-4501c.appspot.com/o/videos%2FCLASES%20ONLINE%20CON%20FOOD%20MASTER.mp4?alt=media&token=2c9c786d-10ca-4d00-b8fc-6fc38936323f";
        }
    }
}
