using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using KalugaTradeApp;
using TradeApp;
using TradeApp.Entities;
using static TradeApp.Entities.Basket;


namespace Views;

public partial class NewOrderView : UserControl
{

    public static Dictionary<Product, BuyItem> CurrentBasket { get; set;} 
   
    
    
    TradeContext context;

    public Order NewOrder {get; set;}
    public List<PickupPoint> PickupPoints  {get; set;}

    public NewOrderView()
    {
        InitializeComponent();
        LoadDataAndInit();
        DataContext = this;

    }
     private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

        /// <summary>
        /// Загрузка и инициализация полей
        /// </summary>
        void LoadDataAndInit()
        {

        var TextBlockOrderNumber = this.FindControl<TextBlock>("TextBlockOrderNumber");
        var TextBlockTotalCost = this.FindControl<TextBlock>("TextBlockTotalCost");
        var TextBlockOrderCreateDate = this.FindControl<TextBlock>("TextBlockOrderCreateDate");
        var TextBlockOrderDeliveryDate = this.FindControl<TextBlock>("TextBlockOrderDeliveryDate");
        var TextBlockOrderGetCode = this.FindControl<TextBlock>("TextBlockOrderGetCode");


        // источник данных корзина
        context = new TradeContext();
        PickupPoints = context.PickupPoints.OrderBy(c => c.Address).ToList();
        CurrentBasket = Basket.GetBasket;
        NewOrder = CreateNewOrder();
       
        if (App.CurrentUser != null)
        {
        TextBlockOrderNumber.Text = $"Заказ No{NewOrder.Id} на имя " +
        $"{ App.CurrentUser.SecondName} {App.CurrentUser.FirstName} ";
        }
        else
        {
        TextBlockOrderNumber.Text = $"Заказ No{NewOrder.Id}";
        }
        TextBlockTotalCost.Text = $"Итого {Basket.GetTotalCost:C}";
        TextBlockOrderCreateDate.Text = NewOrder.CreateDate.ToLongDateString();
        TextBlockOrderDeliveryDate.Text = NewOrder.DeliveryDate.ToLongDateString();
        TextBlockOrderGetCode.Text = NewOrder.GetCode.ToString();
        }

    public Order CreateNewOrder()
    {
        Order order = new Order();
        context = new TradeContext();
        order.Id = context.Orders.Max(p => p.Id) + 1;
        order.CreateDate = DateOnly.FromDateTime(DateTime.Now);
        order.StatusId = 1;
    if (Basket.IsOnStock)
        order.DeliveryDate = DateOnly.FromDateTime(DateTime.Now.AddDays(3));
    else
        order.DeliveryDate = DateOnly.FromDateTime(DateTime.Now.AddDays(6));
        Random rnd = new Random();
        order.GetCode = rnd.Next(100, 1000);
        return order;
    }

    private void BtnOkClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var topLevel = TopLevel.GetTopLevel(this) as Window;
            topLevel.Close();
        }

        private void BtnDeleteClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {

                var ProductsListBox = this.FindControl<ListBox>("ProductsListBox");
                if (ProductsListBox.SelectedIndex == -1)
                    return;
                KeyValuePair<Product, BuyItem> x = (KeyValuePair<Product, BuyItem>) ProductsListBox.SelectedItem;
                if (x.Key is Product product)
                {
                    var TextBlockTotalCost = this.FindControl<TextBlock>("TextBlockTotalCost");
                    Basket.DeleteProductFromBasket(product);
                    CurrentBasket = Basket.GetBasket;
                    ProductsListBox.ItemsSource = null;
                    ProductsListBox.ItemsSource = CurrentBasket;
                    TextBlockTotalCost.Text = $"Итого {Basket.GetTotalCost:C}";
                }
        }

        private async void BtnBuyItem_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
               
                var ComboPickupPoint = this.FindControl<ComboBox>("ComboPickupPoint");
               if (ComboPickupPoint.SelectedItem == null)
                {
                    MessageWindow messageWindow = new MessageWindow("Не выбран пункт выдачи");
                    await messageWindow.ShowDialog(App.MainWindow);
                    return;
                }
                context = new TradeContext();
                NewOrder.PickuppointId = (ComboPickupPoint.SelectedItem as PickupPoint).Id;
                context.Orders.Add(NewOrder);
                foreach (var item in Basket.GetBasket)
                {
                    OrderProduct orderProduct = new OrderProduct();
                    orderProduct.OrderId = NewOrder.Id;
                    orderProduct.ProductId = item.Key.Id;
                    orderProduct.Count = item.Value.Count;

                    Product product = context.Products.FirstOrDefault(p => p.Id == item.Key.Id);
                    if (item.Value.Count >= product.QuantityInStock)
                        product.QuantityInStock = 0;
                    else product.QuantityInStock -= item.Value.Count;
                    
                    context.OrderProducts.Add(orderProduct);

                }
                    context.SaveChanges();
                    MessageWindow messageWindow1 = new MessageWindow("Заказ сохранен");
                    await messageWindow1.ShowDialog(App.MainWindow);

                    Basket.ClearBasket();
                     var topLevel = TopLevel.GetTopLevel(this) as Window;
            topLevel.Close();

        }
}