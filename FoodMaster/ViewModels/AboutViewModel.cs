using System;
using System.Web;
using System.Windows.Input;
using Acr.UserDialogs;
using FoodMaster.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoodMaster.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        RemoteConfig _remoteConfig;

        public string ContactEmail { get; set; }
        public string PhoneNumber { get; set; }
        public AboutViewModel()
        {
            _remoteConfig = DependencyService.Get<RemoteConfig>();
            Title = "Contáctanos";
            ContactEmail = _remoteConfig.BasicData.Email;
            PhoneNumber = _remoteConfig.BasicData.PhoneNumber;

            OpenWhatsAppCommand = new Command(async () => await Browser.OpenAsync($"https://wa.me/{PhoneNumber}"));
            OpenEmailCommand = new Command(async () => {
                await Xamarin.Essentials.Clipboard.SetTextAsync(ContactEmail);
                ToastConfig tc = new ToastConfig("¡Correo eléctronico copiado!").SetPosition(ToastPosition.Top);
                UserDialogs.Instance.Toast(tc);
                string mailto = HttpUtility.HtmlEncode($"{ContactEmail}?subject=FoodMaster Contacto");
                await Browser.OpenAsync($"mailto:{mailto}");
            });
        }

        public ICommand OpenWhatsAppCommand { get; }
        public ICommand OpenEmailCommand { get; }
    }
}
