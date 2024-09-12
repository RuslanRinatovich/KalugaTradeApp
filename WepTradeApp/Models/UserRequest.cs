using System;
using System.Collections.Generic;

namespace WepTradeApp.Entities;
public partial class User
{
    public string FirstName { get; set; } = null!;

    public string SecondName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? RoleId { get; set; }
}
