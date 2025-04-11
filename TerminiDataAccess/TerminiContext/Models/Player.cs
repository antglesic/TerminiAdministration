using System;
using System.Collections.Generic;

namespace TerminiDataAccess.TerminiContext.Models;

public partial class Player
{
    public int Id { get; set; }

    public bool Active { get; set; }

    public DateTime DateCreated { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string? Foot { get; set; }

    public string? Sex { get; set; }

    public int? Rating { get; set; }

    public virtual ICollection<TerminPlayers> TerminPlayers { get; set; } = new List<TerminPlayers>();
}
