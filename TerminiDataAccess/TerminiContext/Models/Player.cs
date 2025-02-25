using System;
using System.Collections.Generic;

namespace TerminiDataAccess.TerminiContext.Models;

public partial class Player
{
    public int Id { get; set; }

    public bool Active { get; set; }

    public DateTime DateCreated { get; set; }
}
