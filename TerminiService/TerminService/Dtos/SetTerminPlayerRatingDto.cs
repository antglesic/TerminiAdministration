namespace TerminiService.TerminService.Dtos;

public class SetTerminPlayerRatingDto
{
	public int PlayerId { get; set; }
	public int TerminId { get; set; }
	public int Rating { get; set; }
}
