namespace TerminiWeb.Infrastructure.TerminService.Dtos;

public class PlayerRatingListDto
{
	public int RatingValue { get; set; }
	public string RatingDisplay
	{
		get
		{
			return RatingValue.ToString() ?? string.Empty;
		}
	}
}
