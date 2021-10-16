using System;
using System.Threading.Tasks;
using Firebase.Firestore;
using FoodMaster.Droid.Services;
using FoodMaster.Interfaces;
using FoodMaster.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(ConfigurationRemote))]
namespace FoodMaster.Droid.Services
{
    public class ConfigurationRemote : IConfigurationRemote
    {
        public async Task<BasicData> GetBasicData()
        {
            var config = await FirebaseFirestore.Instance.Document("configuracion/datos_basicos").Get().ToAwaitableTask();

            if (config is DocumentSnapshot basic)
            {
                BasicData data = new BasicData();

                data.Email = basic.GetString("email");
                data.PhoneNumber = basic.GetString("phone");
                return data;
            }

            return new BasicData();
        }
    }
}
