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

}