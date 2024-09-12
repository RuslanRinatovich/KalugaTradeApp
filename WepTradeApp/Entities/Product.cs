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

    public int UnittypeId { get; set; }

    public int ManufacturerId { get; set; }

    public int SupplierId { get; set; }

    public int CategoryId { get; set; }

    public byte[]? Photo { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Manufacturer Manufacturer { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;

    public virtual Unittype Unittype { get; set; } = null!;
}
