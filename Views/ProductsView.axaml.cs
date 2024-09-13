using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using KalugaTradeApp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TradeApp;
using TradeApp.Entities;

namespace Views;

public partial class ProductsView : UserControl
{
       public List<Product> Products {get; set;}
       public String PageTitle = "ProductsView";
    int _itemcount = 0;
     TradeContext context;
         public ProductsView()
    {
        
        this.InitializeComponent();
        App.PagesStack.Add(PageTitle);

        context = new TradeContext();
        Products = context.Products.Include(x => x.Manufacturer).
        Include(x => x.Category).
        Include(x => x.Supplier).
        Include(x=>x.Unittype).OrderBy(x=> x.Title).ToList();
        DataContext = this;
        //ProductsListBox.ItemsSource = Products;
        var TextBlockCt = this.FindControl<TextBlock>("TextBlockCount");
        _itemcount = Products.Count;
        TextBlockCt.Text = $" Результат запроса: {Products.Count} записей из {_itemcount}";
    }

     private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

        private void ComboBoxSelectionChanged(object sender,        SelectionChangedEventArgs e)
        {
        UpdateData();
        }

        private void TBoxSearchTextChanged(object sender, TextChangedEventArgs e)
            {
            UpdateData();
            }

    /// <summary>
/// Метод для фильтрации и сортировки данных
/// </summary>
private void UpdateData()
{

     var ComboDiscont = this.FindControl<ComboBox>("ComboDiscont");
        // получаем текущие данные из бд
        var currentProducts = context.Products.OrderBy(p =>p.Title).ToList();
        // выбор только тех товаров, по определенному диапазону скидки
        if (ComboDiscont.SelectedIndex == 1) currentProducts = currentProducts.Where(p => p.DiscountAmount < 10).ToList();
        if (ComboDiscont.SelectedIndex == 2) currentProducts = currentProducts.Where(p => p.DiscountAmount >= 10 && p.DiscountAmount < 15).ToList();
        if (ComboDiscont.SelectedIndex == 3) currentProducts = currentProducts.Where(p => p.DiscountAmount >= 15).ToList();
        // выбор тех товаров, в названии которых есть поисковая строка
    var TBoxSearch = this.FindControl<TextBox>("TBoxSearch");
    String text = TBoxSearch.Text;
    if (text != null)
    {currentProducts = currentProducts.Where(p =>p.Title.ToLower().Contains(text.ToLower())).ToList();}
// сортировка

    var ComboSort = this.FindControl<ComboBox>("ComboSort");
    if (ComboSort.SelectedIndex >= 0)
    {
    // сортировка по возрастанию цены
    if (ComboSort.SelectedIndex == 0)

    currentProducts = currentProducts.OrderBy(p => p.Cost).ToList();
    // сортировка по убыванию цены
    if (ComboSort.SelectedIndex == 1)
        currentProducts = currentProducts.OrderByDescending(p =>p.Cost).ToList();
    }
    // В качестве источника данных присваиваем список данных
    Products = currentProducts;
    // отображение количества записей

    var TextBlockCt = this.FindControl<TextBlock>("TextBlockCount");
    TextBlockCt.Text = $" Результат запроса: {Products.Count} записей из {_itemcount}";
    var ProductsListBox = this.FindControl<ListBox>("ProductsListBox");
    ProductsListBox.ItemsSource = Products;

    }

    private void MenuItem_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        // контекстное меню по нажатию правой кнопки мыши
        // если товар не выбран, завершаем работу
        var ProductsListBox = this.FindControl<ListBox>("ProductsListBox");
        if(ProductsListBox.SelectedItem is Product product)
        {
            
        // добавляем товар в корзину
            Basket.AddProductInBasket(product);
            // отображаем кнопку и текстовое поле
            if (Basket.GetCount > 0)
            {
                var BtnBasket = this.FindControl<Button>("BtnBasket");
                BtnBasket.IsVisible = true;
               var TextBlockBasketInfo = this.FindControl<TextBlock>("TextBlockBasketInfo");
               TextBlockBasketInfo.IsVisible = true;
                TextBlockBasketInfo.Text = $"В корзине {Basket.GetCount} товаров";
            }
        }
    }

    private async void BtnBasket_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var window = new OrderWindow();
        await window.ShowDialog(App.MainWindow);
        context = new TradeContext();
        Products = context.Products.Include(x => x.Manufacturer).
        Include(x => x.Category).
        Include(x => x.Supplier).
        Include(x=>x.Unittype).OrderBy(x=> x.Title).ToList();
        var ProductsListBox = this.FindControl<ListBox>("ProductsListBox");
        ProductsListBox.ItemsSource = null;
        ProductsListBox.ItemsSource = Products;
        var TextBlockBasketInfo = this.FindControl<TextBlock>("TextBlockBasketInfo");
        TextBlockBasketInfo.Text = $"В корзине {Basket.GetCount} товаров";
        if (Basket.GetCount == 0)
        {
            var BtnBasket = this.FindControl<Button>("BtnBasket");
                BtnBasket.IsVisible = false;
                TextBlockBasketInfo.IsVisible = false;
        }

        /* if (App.MainWindow.IsFocused)
        {
            var TextBlockBasketInfo = this.FindControl<TextBlock>("TextBlockBasketInfo");
               TextBlockBasketInfo.IsVisible = true;
                TextBlockBasketInfo.Text = $"В корзине {Basket.GetCount} товаров";
        } */
    }

}