using System.Threading.Tasks;
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
}