using System;
namespace FoodMaster.ViewModels
{
    public class OnlineClassViewModel : BaseViewModel
    {
        public string VideoSource { get; set; }
        public string ImageSource { get; set; }
        public OnlineClassViewModel()
        {
            Title = "Clases online";
            ImageSource = "https://firebasestorage.googleapis.com/v0/b/recetapp-4501c.appspot.com/o/images%2FDise%C3%B1o%20sin%20t%C3%ADtulo.png?alt=media&token=d6710bbe-d5ef-41bb-8a31-6f573e9dbee5";
            VideoSource = "https://firebasestorage.googleapis.com/v0/b/recetapp-4501c.appspot.com/o/videos%2FCLASES%20ONLINE%20CON%20FOOD%20MASTER.mp4?alt=media&token=2c9c786d-10ca-4d00-b8fc-6fc38936323f";
        }
    }
}
