using System;
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
        var TextBlockResult = this.FindControl<TextBlock>("TextBlockResult");
        TextBlockResult.Text = ((sender as Button).DataContext as Product).Id;

       
    }

     private void RemoveButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        x++;
        Product product = ((sender as Button).DataContext as Product);
        
        context.Products.Remove(product);
        context.SaveChanges();
        App.MainWindow.MainContentControl.Content = new ProductsDataGridView();
       
    }


}

