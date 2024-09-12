using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TradeApp.Entities;

namespace Views;

public partial class NewOrderView : UserControl
{
    TradeContext context;
    public List<PickupPoint> PickupPoints  {get; set;}

    public NewOrderView()
    {
        InitializeComponent();
        context = new TradeContext();
        PickupPoints = context.PickupPoints.OrderBy(c => c.Address).ToList();
        DataContext = this;
    }
     private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

}