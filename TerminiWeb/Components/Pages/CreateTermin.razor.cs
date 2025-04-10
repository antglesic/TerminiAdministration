using Microsoft.AspNetCore.Components;
using MudBlazor;
using TerminiWeb.Infrastructure.PlayerService;
using TerminiWeb.Infrastructure.PlayerService.Dtos;
using TerminiWeb.Infrastructure.PlayerService.Models;
using TerminiWeb.Infrastructure.TerminService;
using TerminiWeb.Infrastructure.TerminService.Dtos;
using TerminiWeb.Infrastructure.TerminService.Models;

namespace TerminiWeb.Components.Pages
{
	public partial class CreateTermin : ComponentBase
	{
		#region Injections

		[Inject]
		private IPlayerService? _playerService { get; set; }

		[Inject]
		private ITerminService? _terminService { get; set; }

		[Inject]
		private ILogger<Players>? _logger { get; set; }

		[Inject]
		private IDialogService? _dialogService { get; set; }

		[Inject]
		private NavigationManager? NavigationManager { get; set; }

		#endregion

		#region Fields

		private bool firstLoad = true;
		private DateTime? _scheduledDate;
		private TimeSpan? _startTime;
		private int? _durationMinutes;
		private CreateTerminDto _createTerminViewModel = new CreateTerminDto();
		private IEnumerable<PlayerDto> _players = new List<PlayerDto>();
		private List<PlayerDto> _selectedPlayers = new List<PlayerDto>();

		#endregion

		#region Methods

		protected override async Task OnInitializedAsync()
		{
			_createTerminViewModel = new CreateTerminDto();

			if (firstLoad && (_players == null || !_players.Any()))
			{
				bool isLoaded = await GetPlayers();
				firstLoad = false;

				if (_selectedPlayers.Any())
				{
					_selectedPlayers = new List<PlayerDto>();
				}

				StateHasChanged();
			}

			await base.OnInitializedAsync();
		}

		private async Task<bool> GetPlayers()
		{
			GetPlayerListRequest request = new GetPlayerListRequest();

			try
			{
				GetPlayerListResponse response = await _playerService.GetPlayerList(request);

				if (response != null && response.Players != null && response.Players.Any())
				{
					_players = response.Players;
				}
			}
			catch (Exception ex)
			{
				_logger?.LogError(ex, "CreateTermin.razor.cs.GetPlayers() - Exception message: {Message}", ex.Message);
				return false;
			}

			return true;
		}

		public void OnSelectedItemsChanged(IEnumerable<PlayerDto> item)
		{
			try
			{
				_selectedPlayers = item.ToList();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		private async Task HandleSubmit()
		{
			if (_scheduledDate.HasValue
				&& _startTime.HasValue
				&& (_durationMinutes.HasValue && _durationMinutes.Value > 0)
				&& _selectedPlayers.Count > 0)
			{
				_createTerminViewModel.ScheduleDate = DateOnly.TryParse(_scheduledDate.Value.ToShortDateString(), out DateOnly outputDate) ? outputDate : DateOnly.MinValue;

				_createTerminViewModel.StartTime = TimeOnly.TryParse(_startTime.Value.ToString(), out TimeOnly outputTime) ? outputTime : TimeOnly.MinValue;

				_createTerminViewModel.DurationMinutes = _durationMinutes.HasValue ? _durationMinutes.Value : 0;

				_createTerminViewModel.Players = _selectedPlayers;

				CreateTerminRequest request = new CreateTerminRequest();
				request.CreateTermin = _createTerminViewModel;

				CreateTerminResponse response = await _terminService.CreateTermin(request);

				if (response.Success)
				{
					NavigationManager?.NavigateTo("/termins");
				}
			}
		}

		#endregion
	}
}
