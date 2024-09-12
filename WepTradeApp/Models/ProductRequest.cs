using System;
using System.Collections.Generic;

namespace WepTradeApp.Entities;

public partial class Product
{
    public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public decimal Cost { get; set; }

    public int? MaxDiscountAmount { get; set; }

    public int? DiscountAmount { get; set; }

    public int QuantityInStock { get; set; }

    public string? Description { get; set; }

    public string UnittypeId { get; set; }

    public string ManufacturerId { get; set; }

    public string SupplierId { get; set; }

    public string CategoryId { get; set; }

    public IFormFile? Photo { get; set; }

    
}
