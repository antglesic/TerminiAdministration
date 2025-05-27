using System;
using System.Collections.Generic;

namespace TerminiDataAccess.TerminiContext.Models;

public partial class TerminPlayers
{
    public int Id { get; set; }

    public bool Active { get; set; }

    public DateTime DateCreated { get; set; }

    public int TerminId { get; set; }

    public int PlayerId { get; set; }

    public int? PlayerRating { get; set; }

    public int? TeamNumber { get; set; }

    public virtual Player Player { get; set; } = null!;

    public virtual Termin Termin { get; set; } = null!;
}
