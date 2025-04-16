using Microsoft.AspNetCore.Components;
using MudBlazor;
using TerminiWeb.FilterModels;
using TerminiWeb.Infrastructure.TerminService;
using TerminiWeb.Infrastructure.TerminService.Dtos;

namespace TerminiWeb.Components.Modals
{
	public partial class SetPlayerRatingsOnTermin : ComponentBase
	{
		#region Injections

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
		private int _pageSize = 5;
		private int _pageNumber = 1;
		private int[] _pageSizeOptions = new int[] { 5, 10, 20 };

		#endregion

		#region Methods

		protected override async Task OnInitializedAsync()
		{
			if (firstLoad)
			{
				if (TerminData != null)
				{
					_terminData = TerminData;
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

		#endregion

	}
}