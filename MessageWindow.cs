using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TradeApp.Entities;
using Views;

namespace KalugaTradeApp;

public partial class MessageWindow : Window
{


    public MessageWindow(String header, String message)
    {
        InitializeComponent();
        var TextBlockHeader = this.FindControl<TextBlock>("TextBlockHeader");
        TextBlockHeader.Text = header;
        var TextBlockMessage = this.FindControl<TextBlock>("TextBlockMessage");
        TextBlockMessage.Text = message;

    }
       private void BtnOkClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close();
    }
}