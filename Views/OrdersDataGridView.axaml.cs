using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Castle.Components.DictionaryAdapter.Xml;
using TradeApp;
using TradeApp.Entities;
using ViewModels;

namespace Views;

public partial class OrdersDataGridView : UserControl
{
   TradeContext context;
   OrdersGridViewModel ordersGridViewModel = new OrdersGridViewModel();
   int _itemcount = 0;
    public OrdersDataGridView()
    {
        InitializeComponent();
        _itemcount = ordersGridViewModel.Orders.Count;
        this.DataContext = ordersGridViewModel;
        var TextBlockCt = this.FindControl<TextBlock>("TextBlockCount");
        TextBlockCt.Text = $" Результат запроса: {ordersGridViewModel.Orders.Count} записей из {_itemcount}";
    }

       private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
     private void EditButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
     
       Order order = ((sender as Button).DataContext as Order);
        if(order != null)
        {
            App.MainWindow.MainContentControl.Content = new OrderEditView(order);
        }
       
    }


    private void UpdateData()
    {
     var ComboDiscont = this.FindControl<ComboBox>("ComboDiscont");
     
        // получаем текущие данные из бд
        var currentOrders = ordersGridViewModel.Orders.OrderBy(p =>p.CreateDate).ToList();
        // выбор только тех товаров, по определенному диапазону скидки
        if (ComboDiscont.SelectedIndex == 1) currentOrders = currentOrders.Where(p => p.GetTotalDiscount < 6).ToList();
        if (ComboDiscont.SelectedIndex == 2) currentOrders = currentOrders.Where(p => p.GetTotalDiscount >= 6 && p.GetTotalDiscount < 11).ToList();
        if (ComboDiscont.SelectedIndex == 3) currentOrders = currentOrders.Where(p => p.GetTotalDiscount >= 11).ToList();
        // выбор тех товаров, в названии которых есть поисковая строка
        var TBoxSearch = this.FindControl<TextBox>("TBoxSearch");
            String text = TBoxSearch.Text;
            if (text != null)
            {currentOrders = currentOrders.Where(p =>p.Id.ToString().Contains(text.ToLower())).ToList();}
// сортировка

    var ComboSort = this.FindControl<ComboBox>("ComboSort");
    if (ComboSort.SelectedIndex >= 0)
    {
    // сортировка по возрастанию цены
    if (ComboSort.SelectedIndex == 0)

    currentOrders = currentOrders.OrderBy(p => p.GetTotalCost).ToList();
    // сортировка по убыванию цены
    if (ComboSort.SelectedIndex == 1)
        currentOrders = currentOrders.OrderByDescending(p =>p.GetTotalCost).ToList();
    }
    // В качестве источника данных присваиваем список данных
    var TextBlockCt = this.FindControl<TextBlock>("TextBlockCount");
    TextBlockCt.Text = $" Результат запроса: {currentOrders.Count} записей из {_itemcount}";
    var OrdersDataGrid = this.FindControl<DataGrid>("OrdersDataGrid");
        OrdersDataGrid.ItemsSource = currentOrders;
    }

     private void ComboBoxSelectionChanged(object sender,        SelectionChangedEventArgs e)
        {
        UpdateData();
        }

        private void TBoxSearchTextChanged(object sender, TextChangedEventArgs e)
            {
            UpdateData();

            }
}