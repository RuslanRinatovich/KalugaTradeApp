using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TradeApp.Entities;
using Views;

namespace KalugaTradeApp;

public partial class OrderWindow : Window
{
     TradeContext context;
 public List<PickupPoint> PickupPoints  {get; set;}

    public OrderWindow()
    {
        InitializeComponent();
        OrderContentControl.Content = new NewOrderView();
    }
}