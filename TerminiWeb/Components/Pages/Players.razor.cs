using Microsoft.AspNetCore.Components;
using MudBlazor;
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
		private IEnumerable<PlayerDto> _players;
		private int _pageSize = 5;
		private int _pageNumber = 1;
		private int _totalItems;
		private string _searchString = null;
		private PlayerDto? _selectedPlayer;

		private bool _selectOnRowClick = true;
		private MudTable<PlayerDto>? _table;
		private string _selectedItemText = "No row clicked";
		private HashSet<PlayerDto>? _selectedItems;
		private int[] _pageSizeOptions = new int[] { 5, 10, 20 };
		private PlayerFilterModel _filterModel = new PlayerFilterModel();

		#endregion

		#region Injections

		[Inject]
		private IPlayerService _playerService { get; set; }

		[Inject]
		private ILogger<Players> _logger { get; set; }

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

			try
			{
				GetPlayerListResponse response = await _playerService.GetPlayerList(request);

				if (response != null && response.Players != null && response.Players.Any())
				{
					_players = response.Players;
					_totalItems = _players.Count();
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Players.razor.cs.GetPlayers() - Exception message: {Message}", ex.Message);
				return false;
			}

			return true;
		}

		private void CurrentPageNumberChanged(int page)
		{
			_pageNumber = page;
		}

		private void CurrentPageSizeChanged(int size)
		{
			_pageSize = size;
		}

		private async Task FilteredSearch()
		{
			await GetPlayers(_filterModel);
			StateHasChanged();
		}

		void OnRowClick(TableRowClickEventArgs<PlayerDto> args)
		{
			if (args.Item != null)
			{
				_selectedItemText = $"{args.Item.Name} {args.Item.Surname}";
			}
		}

		private void OnSearch(string text)
		{
			_searchString = text;

			if (_table != null)
				_table.ReloadServerData();
		}

		private bool FilterFunction(PlayerDto player) => FilterFunc(player, _searchString);

		private bool FilterFunc(PlayerDto player, string searchString)
		{
			if (string.IsNullOrEmpty(searchString))
				return true;
			if (player.Name.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
				return true;
			if (player.Surname.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
				return true;
			if (player.Sex.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
				return true;
			if (player.Foot.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
				return true;

			return false;
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
			}
			catch (Exception ex)
			{
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
