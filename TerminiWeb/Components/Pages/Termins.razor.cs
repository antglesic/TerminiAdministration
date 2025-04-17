using Microsoft.AspNetCore.Components;
using MudBlazor;
using TerminiWeb.Components.Modals;
using TerminiWeb.FilterModels;
using TerminiWeb.Infrastructure.TerminService;
using TerminiWeb.Infrastructure.TerminService.Dtos;
using TerminiWeb.Infrastructure.TerminService.Models;

namespace TerminiWeb.Components.Pages
{
	public partial class Termins : ComponentBase
	{
		#region Injections

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
		private IEnumerable<TerminDto>? _termins;
		private int _pageSize = 5;
		private int _pageNumber = 1;
		private int[] _pageSizeOptions = new int[] { 5, 10, 20 };
		private TerminFilterModel _filterModel = new TerminFilterModel();

		#endregion

		#region Methods

		protected override async Task OnInitializedAsync()
		{
			if (firstLoad)
			{
				bool isLoaded = await GetTermins(_filterModel);
				firstLoad = false;
				StateHasChanged();
			}

			await base.OnInitializedAsync();
		}

		private async Task<bool> GetTermins(TerminFilterModel filter)
		{
			GetTerminsRequest request = new GetTerminsRequest();
			//request.Name = filter.Name;
			//request.Surname = filter.Surname;
			//request.FullName = filter.FullName;

			try
			{
				GetTerminsResponse response = await _terminService.GetTermins(request);

				if (response != null && response.Termins != null && response.Termins.Any())
				{
					_termins = response.Termins;
				}
				else
				{
					_termins = new List<TerminDto>();
				}
			}
			catch (Exception ex)
			{
				_logger?.LogError(ex, "Termins.razor.cs.GetTermins() - Exception message: {Message}", ex.Message);
				return false;
			}

			return true;
		}

		private async Task FilteredSearch()
		{
			await GetTermins(_filterModel);
			StateHasChanged();
		}

		private async Task EditTermin(TerminDto data)
		{
			try
			{
				Console.WriteLine(data);
				Console.WriteLine("Edit termin {0}", data?.Id.ToString() ?? string.Empty);
			}
			catch (Exception ex)
			{
				if (_logger != null)
					_logger.LogError($"Error in EditTermin: {ex.Message}", ex);

				throw;
			}

			await InvokeAsync(StateHasChanged);
		}

		private async Task DeleteTermin(TerminDto data)
		{
			Console.WriteLine("Delete termin {0}", data?.Id.ToString() ?? string.Empty);
			await InvokeAsync(StateHasChanged);
		}

		private async Task ViewTermin(TerminDto data)
		{
			Console.WriteLine("View termin {0} ", data?.Id.ToString() ?? string.Empty);
			await InvokeAsync(StateHasChanged);
		}

		private async Task FinishTermin(TerminDto data)
		{
			Console.WriteLine("Finish termin {0} ", data?.Id.ToString() ?? string.Empty);

			if (_dialogService != null)
			{
				var result = await _dialogService.ShowAsync<SetPlayerRatingsOnTermin>(
								"Set player ratings",
								new DialogParameters { { "TerminData", data } },
								new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraExtraLarge, FullWidth = true });

				if (result != null)
				{
					Console.WriteLine("Dialog result: {0}", result.Result.ToString() ?? string.Empty);
				}
				else
				{
					Console.WriteLine("Dialog result is null");
				}
			}

			await InvokeAsync(StateHasChanged);
		}

		private void CreateNewTermin()
		{
			NavigationManager?.NavigateTo("/termins/create");
		}

		#endregion
	}
}
