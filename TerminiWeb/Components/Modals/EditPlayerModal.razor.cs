using Microsoft.AspNetCore.Components;
using MudBlazor;
using TerminiWeb.Infrastructure.PlayerService.Dtos;

namespace TerminiWeb.Components.Modals
{
	public partial class EditPlayerModal : ComponentBase
	{
		[CascadingParameter]
		public IMudDialogInstance? MudDialog { get; set; } = default!;

		[Parameter]
		public PlayerDto? Player { get; set; }

		private PlayerDto? _player;
		private string? _newPlayerName;
		private string? _newPlayerSurname;

		protected override async Task OnParametersSetAsync()
		{
			if (Player == null)
			{
				_player = new PlayerDto();
			}
			else
			{
				_player = Player;
			}

			await InvokeAsync(StateHasChanged);
		}

		private void Save()
		{

		}

		private void Close()
		{
			MudDialog?.Close(DialogResult.Cancel());
		}
	}
}
