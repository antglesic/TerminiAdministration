using Microsoft.AspNetCore.Components;
using MudBlazor;
using TerminiWeb.Components.Modals;
using TerminiWeb.FilterModels;
using TerminiWeb.Infrastructure.PlayerService;
using TerminiWeb.Infrastructure.PlayerService.Dtos;
using TerminiWeb.Infrastructure.PlayerService.Models;

namespace TerminiWeb.Components.Pages
{
	public partial class Players : ComponentBase
	{
		#region Fields

		private bool firstLoad = true;
		private IEnumerable<PlayerDto>? _players;
		private int _pageSize = 5;
		private int _pageNumber = 1;
		private int[] _pageSizeOptions = [5, 10, 20];
		private PlayerFilterModel _filterModel = new PlayerFilterModel();

		#endregion

		#region Injections

		[Inject]
		private IPlayerService? _playerService { get; set; }

		[Inject]
		private ILogger<Players>? _logger { get; set; }

		[Inject] IDialogService? _dialogService { get; set; }

		#endregion

		#region Methods

		protected override async Task OnInitializedAsync()
		{
			if (firstLoad)
			{
				bool isLoaded = await GetPlayers(_filterModel);
				firstLoad = false;
				StateHasChanged();
			}

			await base.OnInitializedAsync();
		}

		private async Task<bool> GetPlayers(PlayerFilterModel filter)
		{
			GetPlayerListRequest request = new GetPlayerListRequest();
			request.Name = filter.Name;
			request.Surname = filter.Surname;
			request.FullName = filter.FullName;
			request.PlayerRating = filter.PlayerRating;

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
				_logger?.LogError(ex, "Players.razor.cs.GetPlayers() - Exception message: {Message}", ex.Message);
				return false;
			}

			return true;
		}

		private async Task FilteredSearch()
		{
			await GetPlayers(_filterModel);
			StateHasChanged();
		}

		public void OnSelectedItemChanged(PlayerDto item)
		{
			try
			{
				Console.WriteLine("Selected player from dropdown: {FullName}", item.FullName);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		public void OnSelectedItemsChanged(IEnumerable<PlayerDto> item)
		{
			try
			{
				Console.WriteLine("Selected players:");
				foreach (var player in item)
				{
					Console.WriteLine(player.FullName);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		private async Task EditPlayer(PlayerDto data)
		{
			try
			{
				Console.WriteLine(data);
				Console.WriteLine("Edit player called {0} with player: {1}", DateTime.Now.ToString(), data?.FullName ?? string.Empty);
				var options = new DialogOptions { CloseOnEscapeKey = true };

				if (_dialogService != null)
				{
					await _dialogService.ShowAsync<EditPlayerModal>($"Edit Player {data?.FullName}", options);
				}
			}
			catch (Exception ex)
			{
				if (_logger != null)
					_logger.LogError($"Error in EditPlayer: {ex.Message}", ex);

				throw;
			}

			await InvokeAsync(StateHasChanged);
		}

		private async Task DeletePlayer(PlayerDto data)
		{
			Console.WriteLine("Delete player called {0} with player: {1}", DateTime.Now.ToString(), data?.FullName ?? string.Empty);
			await InvokeAsync(StateHasChanged);
		}

		private async Task ViewPlayer(PlayerDto data)
		{
			Console.WriteLine("View player called {0} with player: {1}", DateTime.Now.ToString(), data?.FullName ?? string.Empty);
			await InvokeAsync(StateHasChanged);
		}

		#endregion
	}
}
