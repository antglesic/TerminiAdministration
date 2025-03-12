using Microsoft.AspNetCore.Components;
using MudBlazor;
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
				bool isLoaded = await GetPlayers();
				firstLoad = false;
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

		#endregion
	}
}
