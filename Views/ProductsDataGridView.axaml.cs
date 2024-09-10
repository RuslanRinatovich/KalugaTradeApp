using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using TradeApp.Entities;

namespace Views;

public partial class ProductsDataGridView : UserControl
{
    
    public ProductsDataGridView()
    {
        InitializeComponent();
        //this.DataContext = new ProductsGridViewModel();
    }

       private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

