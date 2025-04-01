using Microsoft.AspNetCore.Components;

namespace TerminiWeb.Components.Common
{
	public partial class ContextMenuItem : ComponentBase
	{
		[CascadingParameter(Name = "ContextMenu")]
		public ContextMenu? ContextMenu { get; set; }

		/// <summary>
		/// Text to be displayed in item
		/// </summary>
		[Parameter]
		public string? Text { get; set; }

		/// <summary>
		/// OnClick event callback
		/// </summary>
		[Parameter]
		public EventCallback OnClick { get; set; }

		/// <summary>
		/// Disabled attribute.
		/// </summary>
		[Parameter]
		public bool Disabled { get; set; }

		/// <summary>
		/// Other attributes which will be spread in component
		/// </summary>
		[Parameter(CaptureUnmatchedValues = true)]
		public Dictionary<string, object>? Attributes { get; set; }

		protected override void OnInitialized()
		{
			if (ContextMenu == null)
			{
				throw new InvalidOperationException("ContextMenu is not set.");
			}
			else
			{
				ContextMenu.AddItem(this);
			}
		}
	}
}
