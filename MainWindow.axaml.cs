using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using KalugaTradeApp;
using TradeApp.Entities;
using Views;

namespace TradeApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        App.MainWindow = this;
        App.MainWindow.MainContentControl.Content = new ProductsView();
        //App.MainWindow.Hide();
        //ShowLoginWindow();
        if (App.CurrentUser != null)
        {
            ShowButtons();
        }
        else
        {
            TextBlockUser.Text = "";
        }
        
    }

     private void ProductsButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        App.MainWindow.MainContentControl.Content = new ProductsDataGridView();
    }

     private void OrdersButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        App.MainWindow.MainContentControl.Content = new OrdersDataGridView();
    }
       private void BackButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        App.MainWindow.MainContentControl.Content = new ProductsView();
    }

    public async Task<string> GetImageFile()
    {
        OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filters.Add(new FileDialogFilter() { Name = "Images", Extensions =  { "jpg", "png"} });
            string[] result = await dialog.ShowAsync(this);
            if (result != null)
            {
                      return string.Join(" ", result);
            }
            return null;

    }

    public void ShowButtons()
    {
        bool status = true;
        var BtnEditProducts = this.FindControl<Button>("BtnEditProducts");
        var BtnEditOrders = this.FindControl<Button>("BtnEditOrders");
        var BtnBack = this.FindControl<Button>("BtnBack");
        var TextBlockUser = this.FindControl<TextBlock>("TextBlockUser");

        if (App.CurrentUser.RoleId == 2)
        {
            BtnEditOrders.IsVisible = status;
            BtnBack.IsVisible = status;
            BtnEditProducts.IsVisible = status;
        }

        if (App.CurrentUser.RoleId == 3)
        {
            BtnEditOrders.IsVisible = status;
            BtnBack.IsVisible = status;
        }
        if (status)
            {
            TextBlockUser.Text = App.CurrentUser.Role.Title +":"+ App.CurrentUser.SecondName+ " " + App.CurrentUser.FirstName;
            }
        else
            TextBlockUser.Text = "";
    }

     public void HideButtons()
    {
        bool status = false;
        var BtnEditProducts = this.FindControl<Button>("BtnEditProducts");
        var BtnEditOrders = this.FindControl<Button>("BtnEditOrders");
        var BtnBack = this.FindControl<Button>("BtnBack");
        var TextBlockUser = this.FindControl<TextBlock>("TextBlockUser");

        BtnEditOrders.IsVisible = status;
        BtnBack.IsVisible = status;
        BtnEditProducts.IsVisible = status;
        TextBlockUser.Text = "";
    }

     

    private void Window_Closing(object? senders, WindowClosingEventArgs e)
    {
       // Basket.ClearBasket();
        App.StartWindow.Show();
    }
}
