using System;
using System.Threading.Tasks;
using FoodMaster.Interfaces;
using FoodMaster.Models;
using Xamarin.Forms;

namespace FoodMaster.Services
{
    public class RemoteConfig
    {
        public BasicData BasicData { get; private set; }

        IConfigurationRemote _configurationRemote;

        public RemoteConfig()
        {
            _configurationRemote = DependencyService.Get<IConfigurationRemote>();
        }

        public async Task Initialize()
        {
            BasicData = await _configurationRemote.GetBasicData();
        }
    }
}
