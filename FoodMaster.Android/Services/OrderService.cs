using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Firestore;
using FoodMaster.Droid.Services;
using FoodMaster.Interfaces;
using FoodMaster.Models;
using Java.Util;
using Xamarin.Forms;

[assembly: Dependency(typeof(OrderService))]
namespace FoodMaster.Droid.Services
{
    public class OrderService : IOrderService
    {
        public async Task<IEnumerable<Order>> GetOrdersByUser(string userEmail)
        {
            var ordersStore = await FirebaseFirestore.Instance.Collection("Orders").WhereEqualTo("UserEmail", userEmail).Get().ToAwaitableTask();

            List<Order> orders = new List<Order>();
            if (ordersStore is QuerySnapshot element)
            {
                foreach (DocumentSnapshot item in element.Documents)
                {
                    Order order = new Order();
                    bool parseDate = DateTime.TryParse(item.GetString("OrderAt"), out DateTime orderAt);
                    order.Type = item.GetString("Type");
                    order.Name = item.GetString("Name");
                    order.Image = item.GetString("Image");
                    order.Portion = item.GetString("Portion");
                    order.OrderAt = parseDate ? orderAt : DateTime.Now;

                    orders.Add(order);
                }


                return orders;
            }

            return Enumerable.Empty<Order>();
        }

        public async Task StoreOrder(Order order)
        {
            HashMap map = new HashMap();
            foreach (var item in order.AsDictionary())
            {
                map.Put(item.Key, item.Value.ToString());
            }

            DocumentReference docRef = FirebaseFirestore.Instance.Collection("Orders").Document();
            await docRef.Set(map).ToAwaitableTask().ConfigureAwait(false);

        }
    }

    

    
}
