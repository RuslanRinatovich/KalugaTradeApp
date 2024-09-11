using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
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
        DataContext = this;
        isNew = String.IsNullOrEmpty(product.Id);
        var TextBoxArtikul = this.FindControl<TextBox>("TextBoxArtikul");
        TextBoxArtikul.IsReadOnly = !isNew;

    }
       private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void SaveButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {

        if (isNew)
        {
            SaveImage();
            context.Products.Add(Product);
             context.SaveChanges();
        }
        else
            {
               SaveImage();
                context.Entry(Product).State = EntityState.Modified;
                 context.SaveChanges();
            }
           
            App.MainWindow.MainContentControl.Content = new ProductsDataGridView();
    }

    public void SaveImage()
    {
         var Image = this.FindControl<Image>("ImagePhoto");
            if (Image.Source != null)
            using (var ms = new MemoryStream())
            {
                (Image.Source as Bitmap).Save(ms);
                Product.Photo = ms.ToArray();
            }
    }

     private void CancelButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        App.MainWindow.MainContentControl.Content = new ProductsDataGridView();;
    }

    private async void BtnLoadClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open image File",
            AllowMultiple = false
        });

        if (files.Count >= 1)
        {
            // Open reading stream from the first file.
            String path = files[0].Path.AbsolutePath.ToString();
            var bytes = File.ReadAllBytes(path);
            // Reads all the content of file as a text.
            using (var ms = new MemoryStream(bytes))
                {
                    Bitmap bitmap = new Bitmap(ms);;
                    var Image = this.FindControl<Image>("ImagePhoto");
                    Image.Source = bitmap;
                }
            
        }


    }
}