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
            ImageSource = "https://firebasestorage.googleapis.com/v0/b/recetapp-4501c.appspot.com/o/images%2Fonline_class.png?alt=media&token=2a55abd1-5ec8-452c-bb76-3f922a919007";
            VideoSource = "https://firebasestorage.googleapis.com/v0/b/recetapp-4501c.appspot.com/o/videos%2FCLASES%20ONLINE%20CON%20FOOD%20MASTER.mp4?alt=media&token=2c9c786d-10ca-4d00-b8fc-6fc38936323f";
        }
    }
}
