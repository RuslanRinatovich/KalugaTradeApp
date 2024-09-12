using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using KalugaTradeApp;
using Microsoft.EntityFrameworkCore;
using TradeApp;
using TradeApp.Entities;

namespace KalugaTradeApp;

public partial class LoginWindow : Window
{

     public List<User> Users {get; set;}

    public LoginWindow()
    {
        InitializeComponent();
        
    }

     private void BtnOkClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        TradeContext context = new TradeContext();;
        User user = context.Users.FirstOrDefault(p => p.Password == TbPass.Text && p.Username == TbLogin.Text);
        if (user != null)
        {
            App.CurrentUser = user;
            App.CurrentUser.Role = context.Roles.FirstOrDefault(x=> x.Id == user.RoleId);
            App.MainWindow.ShowButtons();
            this.Close();
        }
    }
     private void BtnCancelClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close();
    }
}