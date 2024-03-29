﻿using System;
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
        IAnalyticsService _analyticsService;
        public ConfigurationRemote()
        {
            _analyticsService = DependencyService.Get<IAnalyticsService>();
        }

        public async Task<BasicData> GetBasicData()
        {
            try
            {
                var config = await FirebaseFirestore.Instance.Document("configuracion/datos_basicos").Get().ToAwaitableTask();

                if (config is DocumentSnapshot basic)
                {
                    BasicData data = new BasicData();

                    data.Email = basic.GetString("email");
                    data.PhoneNumber = basic.GetString("phone");
                    return data;
                }
            }
            catch (Exception ex)
            {
                _analyticsService.Report(ex);
            }
            

            return new BasicData();
        }
    }
}
