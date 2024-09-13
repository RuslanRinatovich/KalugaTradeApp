using System;
using System.Collections.Generic;

namespace TradeApp.Entities;

public partial class OrderProduct
{
      public double GetPrice
    {
        get
            {
                return (double) Count * Product.GetPriceWithDiscount;
            }   
    }
}