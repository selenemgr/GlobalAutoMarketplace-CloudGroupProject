using System;
using System.Collections.Generic;

namespace GlobalAutoLibrary.Models;

public partial class Car
{
    public int CarId { get; set; }

    public int BrandId { get; set; }

    public int SellerId { get; set; }

    public string Model { get; set; } = null!;

    public int Year { get; set; }

    public decimal Price { get; set; }

    public string? Color { get; set; }

    public string Vin { get; set; } = null!;

    public virtual Brand Brand { get; set; } = null!;

    public virtual User Seller { get; set; } = null!;
}
