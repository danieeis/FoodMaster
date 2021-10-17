using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FoodMaster.Interfaces;
using FoodMaster.Models;
using FoodMaster.Services;
using Xamarin.Forms;

namespace FoodMaster.ViewModels
{
    public class HistoryViewModel : BaseViewModel
    {
        public Command RefreshCommand { get; }
        IOrderService _orderService;
        UserService _userService;
        ObservableCollection<Order> _orders;
        public ObservableCollection<Order> Orders
        {
            get => _orders;
            set => SetProperty(ref _orders, value);
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }

        public HistoryViewModel()
        {
            Title = "Historial";
            _orderService = DependencyService.Get<IOrderService>();
            _userService = DependencyService.Get<UserService>();
            RefreshCommand = new Command(async() => await GetOrders());
            Task.Run(GetOrders);
            Name = _userService.User.Names;
            Email = _userService.User.Email;
            Avatar = _userService.User.PhotoUrl ?? "chef.png";
        }

        async Task GetOrders()
        {
            IsBusy = true;
            var orders = await _orderService.GetOrdersByUser(_userService.User?.Email);

            Orders = new ObservableCollection<Order>(orders.OrderByDescending(x=> x.OrderAt));

            IsBusy = false;
        }
    }
}
