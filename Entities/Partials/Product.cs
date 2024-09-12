using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;


namespace TradeApp.Entities;

public partial class Product
{
     public Bitmap GetImage
    {
        get{
            if (Photo is null)
                return new Avalonia.Media.Imaging.Bitmap("Assets/picture.png");
            using (var ms = new MemoryStream(Photo))
                {
                    return new Bitmap(ms);;
                }
        }
               
    }

    /// <summary>
/// Задает цвет фона элемента "#7fff00", если скидка больше 15%
/// </summary>
    public string GetColor
    {
        get
            {
                if (DiscountAmount > 15)
                    return "#7fff00";
                else
                    return "#fff";
            }   
    }
/// <summary>
/// Стоимость товара с учетом скидки
/// </summary>
    public double GetPriceWithDiscount
    {
        get
            {
                double p = Convert.ToDouble(Cost);
                byte d = Convert.ToByte(DiscountAmount);
                return p * (100 - d) / 100;
            }
    }

    /// <summary>
/// Стиль текста - перечеркнутый для товаров со скидкой
/// </summary>
    /*public TextDecorationCollection GetTextDecoration
    {
        get
            {

            
                if (DiscountAmount > 0)
                         return TextDecorations.Strikethrough;
                    return TextDecorations.Baseline;
            }
    }*/
    /// <summary>
    /// Если скидка есть то отображаем компонент
    /// </summary>
    public Boolean GetVisibility
    {
        get
    {
        return (DiscountAmount > 0);
        }
    }
    public Boolean GetCostWithDiscontVisibility
    {
        get
    {
        return (DiscountAmount > 0);
        }
    }

    public Boolean GetCostWithoutDiscontVisibility
    {
        get
    {
        return (DiscountAmount == 0);
        }
    }

        public string GetCountInStock
        {
            get
                    {           return $"в наличии на складе {QuantityInStock}  {Unittype.Title}";        }
        }
    
}