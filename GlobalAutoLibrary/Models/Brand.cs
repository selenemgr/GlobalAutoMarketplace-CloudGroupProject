using System;
using System.Collections.Generic;

namespace GlobalAutoLibrary.Models;

public partial class Brand
{
    public int BrandId { get; set; }

    public string BrandName { get; set; } = null!;

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
