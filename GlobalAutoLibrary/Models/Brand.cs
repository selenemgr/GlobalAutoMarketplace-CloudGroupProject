using System;
using System.Collections.Generic;

namespace GlobalAutoLibrary.Models;

public partial class Brand
{
    public int BrandId { get; set; }

    public string Bname { get; set; } = null!;

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
