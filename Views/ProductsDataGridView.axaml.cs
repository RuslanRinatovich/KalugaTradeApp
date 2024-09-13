using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using TradeApp;
using TradeApp.Entities;

namespace Views;

public partial class ProductsDataGridView : UserControl
{
    int x = 0;
     TradeContext context;
   
    public ProductsDataGridView()
    {
        InitializeComponent();
    }

       private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void EditButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        x++;
       Product product = ((sender as Button).DataContext as Product);
        if(product != null)
        {
            App.MainWindow.MainContentControl.Content = new ProductsEditView(product);
        }
       
    }

     private void RemoveButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        x++;
        var TextBlockResult = this.FindControl<TextBlock>("TextBlockResult");
        String Id = ((sender as Button).DataContext as Product).Id;
        Product product = ((sender as Button).DataContext as Product);
        TextBlockResult.Text = product.Id;
        context = new TradeContext();
        //List<Product> products = context.Products.ToList();

        if (product != null)
            context.Products.Remove(product);
        context.SaveChanges();
        App.MainWindow.MainContentControl.Content = new ProductsDataGridView();
       
    }
 private void AddButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
            App.MainWindow.MainContentControl.Content = new ProductsEditView(new Product());
       
    }


}

