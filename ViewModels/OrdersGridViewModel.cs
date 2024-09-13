using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TradeApp.Entities;

namespace ViewModels
{
    public class OrdersGridViewModel
    {
        public ObservableCollection<Order> Orders { get; }
        TradeContext context;
        public OrdersGridViewModel()
        {
            context = new TradeContext();
            List<Order> orders = context.Orders.Include(x => x.Status).
            Include(x => x.UsernameNavigation).
            Include(x => x.Pickuppoint).ToList();
            Orders = new ObservableCollection<Order>(orders);
        }
    }
}