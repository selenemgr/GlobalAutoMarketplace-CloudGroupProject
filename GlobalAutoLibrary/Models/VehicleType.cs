using System;
using System.Collections.Generic;

namespace GlobalAutoLibrary.Models;

public partial class VehicleType
{
    public int VehicleTypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
