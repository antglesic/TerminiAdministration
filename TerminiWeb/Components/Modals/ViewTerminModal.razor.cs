using Microsoft.AspNetCore.Components;
using TerminiWeb.Infrastructure.TerminService;
using TerminiWeb.Infrastructure.TerminService.Dtos;

namespace TerminiWeb.Components.Modals;

public partial class ViewTerminModal : ComponentBase
{
	#region Parameters

	[Parameter]
	public TerminDto? Termin { get; set; }

	#endregion

	#region Injections

	[Inject]
	private ILogger<ViewTerminModal>? Logger { get; set; }

	[Inject]
	private ITerminService? TerminService { get; set; }

	[Inject]
	private NavigationManager? NavigationManager { get; set; }

	#endregion

	private void Close()
	{
		if (NavigationManager != null)
		{
			NavigationManager.NavigateTo("/termins");
		}
	}
}
