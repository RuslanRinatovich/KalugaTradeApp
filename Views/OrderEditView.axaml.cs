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
using Microsoft.EntityFrameworkCore;


namespace Views;

public partial class OrderEditView : UserControl
{

    public Order Order {get;set;}
     
     public List<PickupPoint> PickupPoints  {get; set;}
      public List<Status> Statuses  {get; set;}
       public Dictionary<Product, BuyItem> CurrentBasket {get; set;}
    
    TradeContext context;

    Basket basket = new Basket();
       

    public OrderEditView(Order order)
    {
        InitializeComponent();
        Order = order;
        CurrentBasket = basket.GetBasket;
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
        var DatePickerDeliveryDate = this.FindControl<DatePicker>("DatePickerDeliveryDate");
        var TextBlockOrderGetCode = this.FindControl<TextBlock>("TextBlockOrderGetCode");
        var TextBlockTotalDiscount = this.FindControl<TextBlock>("TextBlockTotalDiscount");


        // источник данных корзина
        context = new TradeContext();
        PickupPoints = context.PickupPoints.OrderBy(c => c.Address).ToList();

        Statuses = context.Statuses.OrderBy(c => c.Id).ToList();

        Order.Pickuppoint = PickupPoints.FirstOrDefault(x => x.Id == Order.PickuppointId);
        Order.Status = Statuses.FirstOrDefault(x => x.Id == Order.StatusId);
        foreach (OrderProduct item in Order.OrderProducts)
        {
            basket.AddProductInBasket(item.Product);
            basket.SetCount(item.Product, (int) item.Count);
        }
       
        if (Order.UsernameNavigation != null)
        {
        TextBlockOrderNumber.Text = $"Заказ №{Order.Id} на имя " +
        $"{ Order.UsernameNavigation.SecondName} {Order.UsernameNavigation.FirstName} ";
        }
        else
        {
        TextBlockOrderNumber.Text = $"Заказ №{Order.Id}";
        }
            TextBlockTotalCost.Text = $"Общая сумма заказа {Order.GetTotalCost:C}";
            TextBlockTotalDiscount.Text = $"Общий размер скидки {Order.GetTotalDiscount} %";
        }

    

        private async void BtnOkClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            context = new TradeContext();
           var ComboPickupPoint = this.FindControl<ComboBox>("ComboPickupPoint");
               if (ComboPickupPoint.SelectedItem == null)
                {
                    MessageWindow messageWindow = new MessageWindow("Ошибка", "Не выбран пункт выдачи");
                    await messageWindow.ShowDialog(App.MainWindow);
                    return;
                }

            var ComboStatus = this.FindControl<ComboBox>("ComboStatus");
               if (ComboStatus.SelectedItem == null)
                {
                    MessageWindow messageWindow = new MessageWindow("Ошибка", "Не указан статус заказа");
                    await messageWindow.ShowDialog(App.MainWindow);
                    return;
                }
                Order.PickuppointId = (ComboPickupPoint.SelectedItem as PickupPoint).Id;
                 var DatePickerDeliveryDate = this.FindControl<DatePicker>("DatePickerDeliveryDate");
                Order.DeliveryDate = DateOnly.FromDateTime(DatePickerDeliveryDate.SelectedDate.Value.Date);
                Order.StatusId = (ComboStatus.SelectedItem as Status).Id;
                context.Entry(Order).State = EntityState.Modified;
                context.SaveChanges();
                MessageWindow messageWindow1 = new MessageWindow("Информация", "Заказ сохранен");
                await messageWindow1.ShowDialog(App.MainWindow);
        }

      
        private async void BtnPrint_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
         
                    await Print(Order);
                    MessageWindow messageWindow1 = new MessageWindow("Информация", "Заказ сохранен");
                    await messageWindow1.ShowDialog(App.MainWindow);
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
            if (!String.IsNullOrEmpty(order.Username))
            {
                 doc.InsertParagraph($"на имя {order.GetUserFIO}"); // 
            }
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
           foreach(OrderProduct item in order.OrderProducts)
           {
                 table.Rows[row].Cells[0].Paragraphs[0].Append($"{row}").Bold();
                 table.Rows[row].Cells[1].Paragraphs[0].Append($"{item.Product.Title}").Bold();
                 table.Rows[row].Cells[2].Paragraphs[0].Append($"{item.Count}").Bold();
                 table.Rows[row].Cells[3].Paragraphs[0].Append($"{item.Product.Cost} руб.").Bold();
                 table.Rows[row].Cells[4].Paragraphs[0].Append($"{item.Product.DiscountAmount}%").Bold();
                 table.Rows[row].Cells[5].Paragraphs[0].Append($"{item.Product.GetPriceWithDiscount} руб.").Bold();
                 table.Rows[row].Cells[6].Paragraphs[0].Append($"{item.GetPrice} руб.").Bold();
                row++;
           }
            doc.InsertTable(table);
           doc.InsertParagraph($"Общая стоимость товара: {order.GetTotalCost} руб."); // 
            doc.InsertParagraph($"Общий размер скидки: {order.GetTotalDiscount} %"); // 
            doc.Save(); // save changes to file
        }
        }
    }
}