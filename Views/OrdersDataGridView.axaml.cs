using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TradeApp.Entities;

namespace Views;

public partial class OrdersDataGridView : UserControl
{
   TradeContext context;
    public OrdersDataGridView()
    {
        InitializeComponent();
    }

       private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}