using Avalonia.Controls;
using Views;

namespace TradeApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        App.MainWindow = this;
        App.MainWindow.MainContentControl.Content = new ProductsView();
    }

     private void ProductsButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        App.MainWindow.MainContentControl.Content = new ProductsDataGridView();
    }
       private void BackButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        App.MainWindow.MainContentControl.Content = new ProductsView();
    }
}