namespace TerminiWeb.FilterModels
{
	public class TerminFilterModel
	{
		public DateOnly? ScheduledDateFilter { get; set; }
		public TimeOnly? StartTimeFilter { get; set; }
		public int? DurationMinutesFilter { get; set; }
	}
}
