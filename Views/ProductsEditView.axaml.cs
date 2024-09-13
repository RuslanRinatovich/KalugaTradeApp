using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Avalonia.VisualTree;
using KalugaTradeApp;
using Microsoft.EntityFrameworkCore;
using TradeApp;
using TradeApp.Entities;

namespace Views;

public partial class ProductsEditView : UserControl
{
    public bool isNew = false;
    public Product Product { get; set; }
    public List<Category> Categories  {get; set;}
    public List<Manufacturer> Manufacturers  {get; set;}
    public List<Supplier> Suppliers  {get; set;}
    public List<Unittype> Unittypes  {get; set;}
    TradeContext context;
    public ProductsEditView(Product product)
    {
        InitializeComponent();
        context = new TradeContext();
        Categories = context.Categories.OrderBy(c => c.Title).ToList();
        Manufacturers = context.Manufacturers.OrderBy(c => c.Title).ToList();
        Suppliers = context.Suppliers.OrderBy(c => c.Title).ToList();
        Unittypes = context.Unittypes.OrderBy(c => c.Title).ToList();
        Product = product;
        product.Category = Categories.FirstOrDefault(x => x.Id == product.CategoryId);
        product.Manufacturer = Manufacturers.FirstOrDefault(x => x.Id == product.ManufacturerId);
        product.Supplier = Suppliers.FirstOrDefault(x => x.Id == product.SupplierId);
        product.Unittype = Unittypes.FirstOrDefault(x => x.Id == product.UnittypeId);



        DataContext = this;
        isNew = String.IsNullOrEmpty(product.Id);
        var TextBoxArtikul = this.FindControl<TextBox>("TextBoxArtikul");
        TextBoxArtikul.IsReadOnly = !isNew;

    }
       private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private async void SaveButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
            StringBuilder _error = CheckFields();
            // если ошибки есть, то выводим ошибки в MessageBox
            // и прерываем выполнение
            if (_error.Length > 0)
            {
                 MessageWindow messageWindow = new MessageWindow( "Ошибка" ,_error.ToString());
                await messageWindow.ShowDialog(App.MainWindow);

                return;
            }


        if (isNew)
        {
            context.Products.Add(Product);
        }
        else
            context.Entry(Product).State = EntityState.Modified;
            context.SaveChanges();
            App.MainWindow.MainContentControl.Content = new ProductsDataGridView();
    }

    
     private void CancelButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        App.MainWindow.MainContentControl.Content = new ProductsDataGridView();;
    }

    private async void BtnLoadClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        await promptForFileThatExists();

    }
    private async Task promptForFileThatExists()
        {
            var ImagePhoto = this.FindControl<Image>("ImagePhoto");
            var dialog = new Avalonia.Controls.OpenFileDialog();
            var win = (Window)this.GetVisualRoot();

            string[] result = await dialog.ShowAsync(win);

            if (result != null && result.Any())
            {
                var FilePath = result.First();
                Product.Photo = File.ReadAllBytes(FilePath);
                ImagePhoto.Source = new Avalonia.Media.Imaging.Bitmap(FilePath);
                ;
            }
        }

    /// <summary>
/// Проверка полей ввод на корректыне данные
/// </summary>
/// <returns>текст StringBuilder содержащий ошибки, если они есть</returns>
private StringBuilder CheckFields()
{
    StringBuilder s = new StringBuilder();
    // проверка полей на содержимое
    if (string.IsNullOrWhiteSpace(Product.Id))
    s.AppendLine("Поле артикул пустое");
    if (string.IsNullOrWhiteSpace(Product.Title))
    s.AppendLine("Поле название пустое");
    if (Product.Category == null)
    s.AppendLine("Выберите категорию продукции");
    if (Product.Manufacturer == null)
    s.AppendLine("Выберите производителя");
    if (Product.Supplier == null)
    s.AppendLine("Выберите поставщика");
    if (Product.Unittype == null)
    s.AppendLine("Выберите единицу измерения");

    var TextBoxCost = this.FindControl<TextBox>("TextBoxCost");
    if (string.IsNullOrWhiteSpace(TextBoxCost.Text))
    s.AppendLine("Поле стоимость пустое");

    if (string.IsNullOrWhiteSpace(Product.Description))
    s.AppendLine("Поле описание пустое");

    var TextBoxQuantityInStock = this.FindControl<TextBox>("TextBoxQuantityInStock");
    if (string.IsNullOrWhiteSpace(TextBoxQuantityInStock.Text))
    s.AppendLine("Поле количество пустое");
     var TextBoxDiscountAmount = this.FindControl<TextBox>("TextBoxDiscountAmount");
    if (string.IsNullOrWhiteSpace(TextBoxDiscountAmount.Text))
    s.AppendLine("Поле скидка пустое");
     var TextBoxDiscountAmountMax = this.FindControl<TextBox>("TextBoxDiscountAmountMax");
    if (string.IsNullOrWhiteSpace(TextBoxDiscountAmountMax.Text))
    s.AppendLine("Поле максимальная скидка пустое");
    // если поле стоимость не пустое,
    // проверяем данные на корректность
    if (!string.IsNullOrWhiteSpace(TextBoxCost.Text))
    {
    double x = 0;
    if (!double.TryParse(TextBoxCost.Text, out x))
    s.AppendLine("Стоимость только число");
    else if (x < 0)
    {
    s.AppendLine("Стоимость не может быть отрицательной");
    }
    }
    // если поле количество не пустое,
    // проверяем данные на корректность
    if (!string.IsNullOrWhiteSpace(TextBoxQuantityInStock.Text))
    {
    int x = 0;
    if (!int.TryParse(TextBoxQuantityInStock.Text, out x))
    s.AppendLine("Количество только число");
    else if (x < 0)
    {
    s.AppendLine("Количество не может быть отрицательной");
    }
    }
    // если поле скидка не пустое,
    // проверяем данные на корректность
    if (!string.IsNullOrWhiteSpace(TextBoxDiscountAmount.Text))
    {
    int x = 0;
    if (!int.TryParse(TextBoxDiscountAmount.Text, out x))
    s.AppendLine("Скидка только число");
    else if (x < 0)
    {
    s.AppendLine("Скидка не может быть отрицательной");
    }
    }
    // если поле максимальная скидка не пустое,
    // проверяем данные на корректность
    if (!string.IsNullOrWhiteSpace(TextBoxDiscountAmountMax.Text))
    {
    int x = 0;
    if (!int.TryParse(TextBoxDiscountAmountMax.Text, out x))
    s.AppendLine("Максимальная скидка только число");
    else if (x < 0)
    {
    s.AppendLine("Максимальная скидка не может быть отрицательной");
    }
    }
    // максимальная скидка не может быть меньше, чем текущая скидка
    if (!string.IsNullOrWhiteSpace(TextBoxDiscountAmount.Text) &&
    !string.IsNullOrWhiteSpace(TextBoxDiscountAmountMax.Text))
    {
    int x, y;
    if (int.TryParse(TextBoxDiscountAmountMax.Text, out x) &&
    int.TryParse(TextBoxDiscountAmount.Text, out y))
    {
    if (x < y)
    {
        s.AppendLine("Максимальная скидка не может быть меньше действующей скидки");
    }
    }
    }
    return s;
    }
    }