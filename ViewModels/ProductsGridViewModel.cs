using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TradeApp.Entities;

namespace ViewModels
{
    public class ProductsGridViewModel
    {
        public ObservableCollection<Product> Products { get; }
        TradeContext context;
        public ProductsGridViewModel()
        {
            context = new TradeContext();
            List<Product> products = context.Products.Include(x => x.Manufacturer).
            Include(x => x.Category).
            Include(x => x.Supplier).
            Include(x=>x.Unittype).ToList();
            Products = new ObservableCollection<Product>(products);
        }
    }
}