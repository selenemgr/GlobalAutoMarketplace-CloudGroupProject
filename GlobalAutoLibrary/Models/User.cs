using System;
using System.Collections.Generic;

namespace GlobalAutoLibrary.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string UserRole { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
