using System;
using System.Threading.Tasks;
using FoodMaster.Models;

namespace FoodMaster.Interfaces
{
    public interface IConfigurationRemote
    {
        Task<BasicData> GetBasicData();
    }
}
