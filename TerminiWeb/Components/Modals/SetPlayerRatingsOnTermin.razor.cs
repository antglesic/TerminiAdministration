using Microsoft.AspNetCore.Components;
using MudBlazor;
using TerminiWeb.Infrastructure.PlayerService.Dtos;
using TerminiWeb.Infrastructure.TerminService;
using TerminiWeb.Infrastructure.TerminService.Dtos;
using TerminiWeb.Infrastructure.TerminService.Models;

namespace TerminiWeb.Components.Modals
{
	public partial class SetPlayerRatingsOnTermin : ComponentBase
	{
		#region Injections

		[CascadingParameter]
		private IMudDialogInstance? MudDialog { get; set; }

		[Inject]
		private ITerminService? _terminService { get; set; }

		[Inject]
		private ILogger<SetPlayerRatingsOnTermin>? _logger { get; set; }

		[Inject]
		private IDialogService? _dialogService { get; set; }

		#endregion

		#region Parameters 

		[Parameter]
		public TerminDto? TerminData { get; set; }

		#endregion

		#region Fields

		private bool firstLoad = true;
		private TerminDto? _terminData;
		private IEnumerable<PlayerDto>? _players;
		private int _pageSize = 5;
		private int _pageNumber = 1;
		private int[] _pageSizeOptions = [5, 10, 20];
		private PlayerRatingListDto[] _playerRatingOptions =
		[
			new PlayerRatingListDto { RatingValue = 1 },
			new PlayerRatingListDto { RatingValue = 2 },
			new PlayerRatingListDto { RatingValue = 3 },
			new PlayerRatingListDto { RatingValue = 4 },
			new PlayerRatingListDto { RatingValue = 5 },
			new PlayerRatingListDto { RatingValue = 6 },
			new PlayerRatingListDto { RatingValue = 7 },
			new PlayerRatingListDto { RatingValue = 8 },
			new PlayerRatingListDto { RatingValue = 9 },
			new PlayerRatingListDto { RatingValue = 10 }
		];
		private Dictionary<PlayerDto, PlayerRatingListDto> _selectedPlayerRatings = new Dictionary<PlayerDto, PlayerRatingListDto>();

		#endregion

		#region Methods

		protected override async Task OnInitializedAsync()
		{
			if (firstLoad)
			{
				if (TerminData != null)
				{
					_terminData = TerminData;

					if (_terminData.Players != null && _terminData.Players.Any())
					{
						_players = TerminData.Players;
					}
					else
					{
						// Handle the case when TerminData.Players list is null, if necessary
						_logger?.LogWarning("TerminData.Players is null.");
					}
				}
				else
				{
					// Handle the case when TerminData is null, if necessary
					_logger?.LogWarning("TerminData is null.");
				}

				firstLoad = false;
				StateHasChanged();
			}

			await base.OnInitializedAsync();
		}

		public void OnSelectedItemChanged(PlayerDto player, PlayerRatingListDto value)
		{
			try
			{
				Console.WriteLine("Selected player from dropdown: {0} with rating: {1}", player.FullName, value.RatingDisplay);
				_selectedPlayerRatings[player] = value;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				_logger?.LogError(ex, "SetPlayerRatingsOnTermin.OnSelectedItemChanged() - Exception message: {Message}", ex.Message);
			}
		}

		public async Task Save()
		{
			if (_selectedPlayerRatings == null || _selectedPlayerRatings.Count() == 0)
			{
				_logger?.LogWarning("No player ratings selected.");
				return;
			}
			else
			{
				if (_terminData != null)
				{
					IEnumerable<TerminPlayerDto> playerRatings = _selectedPlayerRatings
						.Select(x => new TerminPlayerDto
						{
							PlayerId = x.Key.Id,
							TerminId = _terminData.Id,
							Rating = x.Value.RatingValue
						});

					SetPlayerRatingsRequest request = new SetPlayerRatingsRequest
					{
						TerminPlayers = playerRatings
					};

					if (_terminService != null)
					{
						SetPlayerRatingsResponse? response = await _terminService.SetPlayerRatings(request);
						_logger?.LogInformation("Player ratings to be saved: {PlayerRatings}", playerRatings);
					}
				}
			}

			await InvokeAsync(StateHasChanged);
			MudDialog?.Close(DialogResult.Ok(_selectedPlayerRatings));
		}

		#endregion

	}
}