using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Firebase.Firestore;
using FoodMaster.Droid.Services;
using FoodMaster.Interfaces;
using FoodMaster.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(RecipeService))]
namespace FoodMaster.Droid.Services
{
    public class RecipeService : IRecipeService
    {
        public async Task<IEnumerable<Gastronomy>> GetInternationalCategories()
        {
            var categories = await FirebaseFirestore.Instance.Collection("internacionales").Get().ToAwaitableTask();

            List<Gastronomy> gastronomies = new List<Gastronomy>();
            if (categories is QuerySnapshot cats)
            {
                foreach(DocumentSnapshot item in cats.Documents)
                {
                    Gastronomy gastronomy = new Gastronomy();
                    gastronomy.Id = item.Id;
                    gastronomy.Order = int.Parse(item.GetString("order"));
                    gastronomy.Type = item.GetString("gastronomia");
                    gastronomy.Name = item.GetString("name");
                    gastronomy.Image = item.GetString("image");
                    gastronomy.DocumentPath = $"internacionales/{item.Id}";

                    gastronomies.Add(gastronomy);
                }
                

                return gastronomies;
            }

            return Enumerable.Empty<Gastronomy>();
        }

        public async Task<IEnumerable<Gastronomy>> GetNationalCategories()
        {
            var categories = await FirebaseFirestore.Instance.Collection("nacionales").Get().ToAwaitableTask();

            List<Gastronomy> gastronomies = new List<Gastronomy>();
            if (categories is QuerySnapshot cats)
            {
                foreach (DocumentSnapshot item in cats.Documents)
                {
                    Gastronomy gastronomy = new Gastronomy();
                    gastronomy.Id = item.Id;
                    gastronomy.Order = int.Parse(item.GetString("order"));
                    gastronomy.Type = item.GetString("gastronomia");
                    gastronomy.Name = item.GetString("name");
                    gastronomy.Image = item.GetString("image");
                    gastronomy.DocumentPath = $"nacionales/{item.Id}";

                    gastronomies.Add(gastronomy);
                }


                return gastronomies;
            }

            return Enumerable.Empty<Gastronomy>();
        }

        public async Task<IEnumerable<Food>> GetFoods(string documentPath)
        {
            var foods = await FirebaseFirestore.Instance.Collection(documentPath).Get().ToAwaitableTask();

            List<Food> foodsList = new List<Food>();
            if (foods is QuerySnapshot cats)
            {
                foreach (DocumentSnapshot item in cats.Documents)
                {
                    Food food = new Food();
                    food.Id = item.Id;
                    food.Type = item.GetString("gastronomia");
                    food.Category = item.GetString("categoria");
                    food.Name = item.GetString("name");
                    food.Image = item.GetString("image");
                    food.DocumentPath = $"{documentPath}/{item.Id}";

                    foodsList.Add(food);
                }


                return foodsList;
            }

            return Enumerable.Empty<Food>();
        }

        public async Task<Food> GetFood(string documentPath)
        {
            var foodDoc = await FirebaseFirestore.Instance.Document(documentPath).Get().ToAwaitableTask();

            if (foodDoc is DocumentSnapshot item)
            {
                Food food = new Food();
                food.Id = item.Id;
                food.Type = item.GetString("gastronomia");
                food.Category = item.GetString("categoria");
                food.Name = item.GetString("name");
                food.Image = item.GetString("image");
                food.Level = item.GetString("nivel_preparacion");
                food.Timing = item.GetString("tiempo_preparacion");
                
                if (item.Get("consejos") is Android.Runtime.JavaList consejos)
                {
                    food.Tips = consejos.ToList<string>();
                }
                if (item.Get("preparacion") is Android.Runtime.JavaList preparacion)
                {
                    food.Preparation = preparacion.ToList<string>();
                }
                if (item.Get("valor_nutricional") is Android.Runtime.JavaDictionary nutritional)
                {
                    food.NutritionalValue = nutritional.ToDictionary<string>();
                }
                if (item.Get("ingredientes") is Android.Runtime.JavaDictionary ingredients)
                {
                    food.Ingredients = ingredients.ToListDictionary<string>();
                }
                food.DocumentPath = $"{documentPath}";

                

                return food;
            }

            return new Food();
        }

        public async Task<Gastronomy> GetGastronomy(string documentPath)
        {
            var categories = await FirebaseFirestore.Instance.Document(documentPath).Get().ToAwaitableTask();

            if (categories is DocumentSnapshot item)
            {
                Gastronomy gastronomy = new Gastronomy();
                gastronomy.Id = item.Id;
                gastronomy.Type = item.GetString("gastronomia");
                gastronomy.Name = item.GetString("name");
                gastronomy.Image = item.GetString("image");
                gastronomy.DocumentPath = $"{documentPath}";


                return gastronomy;
            }

            return new Gastronomy();
        }
    }

    public static class ObjectToDictionaryHelper
    {
        public static List<T> ToList<T>(this Android.Runtime.JavaList source)
        {
            var list = new List<T>();
            foreach (var element in source)
            {
                if (element is T value)
                {
                    list.Add(value);
                }
            }

            return list;
        }

        public static Dictionary<string, List<T>> ToListDictionary<T>(this Android.Runtime.JavaDictionary source)
        {
            if (source == null) ThrowExceptionWhenSourceArgumentIsNull();

            var dictionary = new Dictionary<string, List<T>>();
            
            foreach (System.Collections.DictionaryEntry item in source)
            {
                if (item.Value is Android.Runtime.JavaList value)
                {
                    dictionary.Add(item.Key.ToString(), value.ToList<T>());
                }
            }

            return dictionary;
        }

        public static Dictionary<string, T> ToDictionary<T>(this Android.Runtime.JavaDictionary source)
        {
            if (source == null) ThrowExceptionWhenSourceArgumentIsNull();

            var dictionary = new Dictionary<string, T>();
            foreach (System.Collections.DictionaryEntry item in source)
            {
                if (item.Value is T value)
                {
                    dictionary.Add(item.Key.ToString(), value);
                }
            }
            
            return dictionary;
        }

        public static IDictionary<string, object> AsDictionary(this object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            return source.GetType().GetProperties(bindingAttr).ToDictionary
            (
                propInfo => propInfo.Name,
                propInfo => propInfo.GetValue(source, null)
            );
        }

        private static void ThrowExceptionWhenSourceArgumentIsNull()
        {
            throw new NullReferenceException("Unable to convert object to a dictionary. The source object is null.");
        }
    }
}
