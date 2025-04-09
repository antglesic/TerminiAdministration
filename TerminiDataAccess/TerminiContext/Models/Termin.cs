using System;
using System.Collections.Generic;

namespace TerminiDataAccess.TerminiContext.Models;

public partial class Termin
{
    public int Id { get; set; }

    public bool Active { get; set; }

    public DateTime DateCreated { get; set; }

    public DateOnly ScheduledDate { get; set; }

    public TimeOnly StartTime { get; set; }

    public int DurationMinutes { get; set; }

    public virtual ICollection<TerminPlayers> TerminPlayers { get; set; } = new List<TerminPlayers>();
}
