using System.Threading.Tasks;
using Avalonia.Controls;
using KalugaTradeApp;
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

    public void ShowButtons()
    {
        bool status = true;
        var BtnEditProducts = this.FindControl<Button>("BtnEditProducts");
        var BtnEditOrders = this.FindControl<Button>("BtnEditOrders");
        var BtnBack = this.FindControl<Button>("BtnBack");
        var BtnLogin = this.FindControl<Button>("BtnLogin");
        var BtnLogout = this.FindControl<Button>("BtnLogout");
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
        BtnLogin.IsVisible = !status;
        BtnLogout.IsVisible = status;
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
        var BtnLogin = this.FindControl<Button>("BtnLogin");
        var BtnLogout = this.FindControl<Button>("BtnLogout");
        var TextBlockUser = this.FindControl<TextBlock>("TextBlockUser");

        BtnEditOrders.IsVisible = status;
        BtnBack.IsVisible = status;
        BtnEditProducts.IsVisible = status;
        BtnLogin.IsVisible = !status;
        BtnLogout.IsVisible = status;
        TextBlockUser.Text = "";
    }

     private void LoginButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var window = new LoginWindow();
        window.ShowDialog(App.MainWindow);
    }
    private void LogoutButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        App.MainWindow.MainContentControl.Content = new ProductsView();
        App.CurrentUser = null;
        HideButtons();
    }

}
