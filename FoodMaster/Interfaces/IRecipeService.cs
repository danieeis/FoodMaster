using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodMaster.Models;

namespace FoodMaster.Interfaces
{
    public interface IRecipeService
    {
        Task<IEnumerable<Gastronomy>> GetNationalCategories();
        Task<IEnumerable<Gastronomy>> GetInternationalCategories();
    }
}
