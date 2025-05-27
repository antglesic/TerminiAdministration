using Microsoft.AspNetCore.Components;
using TerminiWeb.Infrastructure.PlayerService.Dtos;
using TerminiWeb.Infrastructure.TerminService.Dtos;

namespace TerminiWeb.Components.Modals;

public partial class ViewTerminModal : ComponentBase
{
	#region Parameters

	[Parameter]
	public TerminDto? TerminData { get; set; }

	#endregion

	#region Fields

	private TerminDto? _termin;
	private List<int?>? _teamNumbers = new();

	#endregion

	#region Injections

	[Inject]
	private ILogger<ViewTerminModal>? Logger { get; set; }

	[Inject]
	private NavigationManager? NavigationManager { get; set; }

	#endregion

	#region Methods

	protected override async Task OnInitializedAsync()
	{
		if (TerminData == null)
		{
			Logger?.LogError("Termin is null in ViewTerminModal.");
			Close();
		}
		else
		{
			_termin = TerminData;
			Logger?.LogInformation("Initialized ViewTerminModal with Termin ID: {TerminId}", _termin.Id);

			if (_termin != null && _termin.Players != null && _termin.Players.Any())
			{
				_teamNumbers = _termin.Players
					.Select(p => p.TeamNumber)
					.Distinct()
					.OrderBy(t => t)
					.ToList();

				Logger?.LogInformation("Team numbers for the termin: {TeamNumbers}", string.Join(", ", _teamNumbers));
			}
			else
			{
				if (_termin != null && _termin.Players == null)
					Logger?.LogWarning("No players found for the termin with ID: {TerminId}", _termin.Id);
			}
		}

		await base.OnInitializedAsync();
	}

	private void Close()
	{
		NavigationManager?.NavigateTo("/termins");
	}

	private IEnumerable<PlayerDto> GetTeamPlayers(int teamNumber)
	{
		return _termin?
			.Players?
			.Where(p => p.TeamNumber == teamNumber) ?? [];
	}

	#endregion
}
