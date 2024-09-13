using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.VisualTree;
using KalugaTradeApp;
using TradeApp;
using TradeApp.Entities;
using Xceed.Document.NET;
using System.Threading.Tasks;
using Xceed.Words.NET;
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
               if (Basket.GetCount == 0)
               {
                    MessageWindow messageWindow = new MessageWindow("Ошибка", "В корзине нет товаров");
                    await messageWindow.ShowDialog(App.MainWindow);
                    return;
               }

                var ComboPickupPoint = this.FindControl<ComboBox>("ComboPickupPoint");
               if (ComboPickupPoint.SelectedItem == null)
                {
                    MessageWindow messageWindow = new MessageWindow("Ошибка", "Не выбран пункт выдачи");
                    await messageWindow.ShowDialog(App.MainWindow);
                    return;
                }
                context = new TradeContext();
                NewOrder.PickuppointId = (ComboPickupPoint.SelectedItem as PickupPoint).Id;
                context.Orders.Add(NewOrder);
                context.SaveChanges();
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
                    await Print(NewOrder);
                    MessageWindow messageWindow1 = new MessageWindow("Информация", "Заказ сохранен");
                    await messageWindow1.ShowDialog(App.MainWindow);

                    Basket.ClearBasket();
                     var topLevel = TopLevel.GetTopLevel(this) as Window;
            topLevel.Close();

        }

     private async Task Print(Order order)
    {
         
        if (order != null)
        {
            var topLevel = TopLevel.GetTopLevel(App.MainWindow);

        // Start async operation to open the dialog.
        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
         {
            Title = "Save Word File"
        });

        if (file is not null)
        {
            if (!Directory.Exists("docs"))
            Directory.CreateDirectory("docs");
            string fileName = Path.Combine("docs",$"{order.Id}.docx");
            var doc = DocX.Create(file.Path.AbsolutePath);
            doc.AddHeaders(); 
            doc.AddFooters(); 
            doc.DifferentFirstPage = true;
            doc.Headers.First.InsertParagraph("ФИРМА ООО РУЧКИ");
            doc.InsertParagraph($"Заказ №{order.Id}"); // 

            doc.InsertParagraph($"Дата заказа: {order.CreateDate.ToLongDateString()}");
            doc.InsertParagraph($"Дата получения заказа: {order.DeliveryDate.ToLongDateString()}");
            PickupPoint pickupPoint = PickupPoints.First(p => p.Id == order.PickuppointId);
            doc.InsertParagraph($"Пункт выдачи: {pickupPoint.Address}");
            doc.InsertParagraph($"Код получения: {order.GetCode}");
           

            Table table = doc.AddTable(order.OrderProducts.Count+1, 7);
            table.Design = TableDesign.ColorfulList;
            table.Alignment = Alignment.center;
            table.AutoFit = AutoFit.Contents;

            // Add headers to the table
            table.Rows[0].Cells[0].Paragraphs[0].Append("№").Bold();
            table.Rows[0].Cells[1].Paragraphs[0].Append("Наименование товара").Bold();
            table.Rows[0].Cells[2].Paragraphs[0].Append("Количество").Bold();
            table.Rows[0].Cells[3].Paragraphs[0].Append("Стоимость товара без скидки").Bold();
            table.Rows[0].Cells[4].Paragraphs[0].Append("Скидка").Bold();
            table.Rows[0].Cells[5].Paragraphs[0].Append("Стоимость товара со скидкой").Bold();
             table.Rows[0].Cells[6].Paragraphs[0].Append("Итого").Bold();
            // Add data to the table
            int row = 1;
            double total = 0;
           foreach(OrderProduct item in order.OrderProducts)
           {
                 table.Rows[row].Cells[0].Paragraphs[0].Append($"{row}").Bold();
                 table.Rows[row].Cells[1].Paragraphs[0].Append($"{item.Product.Title}").Bold();
                 table.Rows[row].Cells[2].Paragraphs[0].Append($"{item.Count}").Bold();
                 table.Rows[row].Cells[3].Paragraphs[0].Append($"{item.Product.Cost} руб.").Bold();
                 table.Rows[row].Cells[4].Paragraphs[0].Append($"{item.Product.DiscountAmount}%").Bold();
                 table.Rows[row].Cells[5].Paragraphs[0].Append($"{item.Product.GetPriceWithDiscount} руб.").Bold();
                 table.Rows[row].Cells[6].Paragraphs[0].Append($"{item.GetPrice} руб.").Bold();
                 total+= item.GetPrice;
                row++;
           }
           doc.InsertTable(table);
           doc.InsertParagraph($"Итого: {total} руб."); // 
            doc.Save(); // save changes to file
        }
        }
    }
}