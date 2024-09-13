using System;
using System.Collections.Generic;

namespace TradeApp.Entities;

/// <summary>
/// Класс корзина, содержит список покупок
/// </summary>
public class Basket
{
    public  Dictionary<Product, BuyItem> GetBasket { get; } = new  Dictionary<Product, BuyItem>();
    /// <summary>
    /// Value словаря - количество товара и стоимость
    /// </summary>
    
    public struct BuyItem
        {
            public int Count { get; set; }
            public double Cost { get; set; }
             public double Total { get; set; }
        }
    /// <summary>
    /// Словарь хранит товар в качестве ключа и BuyItem в качестве значения
    /// </summary>
    
    

    // очистка корзины
    public void ClearBasket()
    {
        this.GetBasket.Clear();
    }
    /// <summary>
    /// Добавление товара в корзину
    /// </summary>
    /// <param name="product">Добавляемый товар</param>
    public void AddProductInBasket(Product product)
    {
    // если такой товар есть в корзине
        if (this.GetBasket.ContainsKey(product))
        {
            // увеличиваем его количество на +1
            int k = this.GetBasket[product].Count + 1;
            // пересчистваем стоимость
            double p = Convert.ToDouble(product.GetPriceWithDiscount) * k;
            double c = Convert.ToDouble(product.Cost) * k;
            this.GetBasket[product] = new BuyItem { Count = k, Cost = c, Total = p };
        }
    
        else
        {
            // добавляем новый товар в корзину в количесьве 1 шт
            double p = Convert.ToDouble(product.GetPriceWithDiscount);
            double c = Convert.ToDouble(product.Cost);
            this.GetBasket[product] = new BuyItem { Count = 1, Cost = c, Total = p };
        }
    }
    /// <summary>
    /// Изменяет количество товара product в корзине
    /// </summary>
    /// <param name="product">Товар</param>
    /// <param name="count">количество товара</param>
    public  void SetCount(Product product, int count)
    {
        if (this.GetBasket.ContainsKey(product))
        {
            int k = count;
            double p = Convert.ToDouble(product.GetPriceWithDiscount) * k;
            double c = Convert.ToDouble(product.Cost) * k;
            this.GetBasket[product] = new BuyItem { Count = k, Cost = c, Total = p };
        // если количество 0 или меньше 0 удаляем товар из корзины
            if (k <= 0)
                {
                    GetBasket.Remove(product);
                }
        }
    }
    /// <summary>
    /// Удаляем товар product из корзины
    /// </summary>
    /// <param name="product">Удаляемый товар</param>
    public  void DeleteProductFromBasket(Product product)
    {
    if (this.GetBasket.ContainsKey(product))
            {
                GetBasket.Remove(product);
            }
    }
    /// <summary>
    /// Cтоимость всех товаров в корзине
    /// </summary>
    public  double GetTotalCost
    {
        get
        {
            double sum = 0;
            foreach (var item in GetBasket)
                {
                sum += item.Value.Total;
                }
            return sum;
        }
    }

    public  double GetCostWithoutDiscont
    {
        get
        {
            double sum = 0;
            foreach (var item in GetBasket)
                {
                sum += item.Value.Cost;
                }
            return sum;
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
    /// <summary>
    /// Количество товаров в корзине
    /// </summary>
    public  int GetCount
        {
            get
                {
                    return GetBasket.Count;
                }
        }
    /// <summary>
    /// Возвращает true, если на складе каждого товара не меньше 3 единиц
    /// </summary>
    public bool IsOnStock
    {
        get
        {
            foreach (var item in GetBasket)
            {
                if (item.Key.QuantityInStock < 3)
                {
                        return false;
            }
            }    return true;
        }
    }
}
