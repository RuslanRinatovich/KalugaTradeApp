using System;
using System.Collections.Generic;

namespace TradeApp.Entities;

public partial class User
{
     public String GetFIO
    {
        get
        {
            return $"{SecondName} {FirstName} {MiddleName}";
        }
    }
}