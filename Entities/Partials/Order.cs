using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TradeApp.Entities;

public partial class Order
{

   
    public double GetTotalCost
    {
        get
            {
            if (OrderProducts.Count == 0)
                return 0;
               double? total = 0;
               foreach(OrderProduct item in OrderProducts)
               {
                total += item.Count * item.Product.GetPriceWithDiscount;
               }
            return (double)total;
            }   
    }
     public double GetCostWithoutDiscont
    {
        get
            {
            if (OrderProducts.Count == 0)
                return 0;
               double? total = 0;
               foreach(OrderProduct item in OrderProducts)
               {
                total += item.Count * (double) item.Product.Cost;
               }
            return (double)total;
            }   
    }

    public int GetTotalDiscount
    {
        get
        {
            int discount = (int) Math.Round((GetCostWithoutDiscont - GetTotalCost) / GetTotalCost * 100);
            if (discount < 0)
                discount = 0;
            return discount;
        }
    }

       public String GetUserFIO
    {
        get
        {
            if (String.IsNullOrEmpty(Username))
                return "";
            return UsernameNavigation.GetFIO;
        }
    }

    public string GetColor
        {
            get
            {
                
                 return "#20b2aa";
            }
        }

    public DateTimeOffset GetDeliveryDate {
        get
        {
            return new(DeliveryDate.Year, DeliveryDate.Month, DeliveryDate.Day, 0, 0, 0, TimeSpan.FromHours(10));
        }
    }

}