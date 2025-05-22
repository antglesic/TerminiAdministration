using Microsoft.AspNetCore.Components;
using MudBlazor;
using TerminiWeb.Infrastructure.PlayerService;
using TerminiWeb.Infrastructure.PlayerService.Dtos;
using TerminiWeb.Infrastructure.PlayerService.Models;
using TerminiWeb.Infrastructure.TerminService;

namespace TerminiWeb.Components.Pages
{
	public partial class CreatePlayer : ComponentBase
	{
		#region Injections

		[Inject]
		private IPlayerService? PlayerService { get; set; }

		[Inject]
		private ITerminService? TerminService { get; set; }

		[Inject]
		private ILogger<Players>? Logger { get; set; }

		[Inject]
		private IDialogService? DialogService { get; set; }

		[Inject]
		private NavigationManager? NavigationManager { get; set; }

		#endregion

		#region Fields

		private readonly bool firstLoad = true;
		private string? _name;
		private string? _surname;
		private string? _sex;
		private string? _foot;
		private CreatePlayerDto _createPlayerViewModel = new();

		#endregion

		#region Methods

		protected override async Task OnInitializedAsync()
		{
			_createPlayerViewModel = new();

			if (firstLoad)
			{
				StateHasChanged();
			}

			await base.OnInitializedAsync();
		}

		private async Task HandleSubmit()
		{
			if (!string.IsNullOrEmpty(_name)
				&& !string.IsNullOrEmpty(_surname)
				&& !string.IsNullOrEmpty(_foot)
				&& !string.IsNullOrEmpty(_sex))
			{
				_createPlayerViewModel.Name = _name;
				_createPlayerViewModel.Surname = _surname;
				_createPlayerViewModel.Foot = _foot;
				_createPlayerViewModel.Sex = _sex;

				CreatePlayerRequest request = new CreatePlayerRequest();
				request.CreatePlayer = _createPlayerViewModel;

				if (PlayerService != null)
				{
					CreatePlayerResponse response = await PlayerService.CreatePlayer(request);

					if (response.Success)
					{
						NavigationManager?.NavigateTo("/players");
					}
				}
			}
		}

		#endregion
	}
}
