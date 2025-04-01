using Microsoft.AspNetCore.Components;

namespace TerminiWeb.Components.Common
{
	public partial class ContextMenu : ComponentBase
	{
		[Parameter]
		public RenderFragment? ChildContent { get; set; }

		/// <summary>
		/// Data for which context menu is displayed
		/// </summary>
		[Parameter]
		public object? Data { get; set; }

		/// <summary>
		/// Other attributes which will be spread in component
		/// </summary>
		[Parameter(CaptureUnmatchedValues = true)]
		public Dictionary<string, object>? Attributes { get; set; }

		/// <summary>
		/// Css class for styling
		/// </summary>
		[Parameter]
		public string CssClass
		{
			set
			{
				cssClass = value;
			}
		}

		/// <summary>
		/// Caption
		/// </summary>
		[Parameter]
		public string Caption
		{
			set
			{
				caption = value;
			}
		}

		/// <summary>
		/// Context button Css class for styling
		/// </summary>
		[Parameter]
		public string ContextButtonCssClass
		{
			set
			{
				contextButtonCssClass = value;
			}
		}

		/// <summary>
		/// List of items in context menu
		/// </summary>
		public List<ContextMenuItem> Items { get; } = new List<ContextMenuItem>();

		public bool Show { get; set; }
		public string ShowClass { get { return Show ? "show" : string.Empty; } }
		private string? cssClass;
		private string? caption;
		private string contextButtonCssClass = "context-menu-button";

		/// <summary>
		/// Adds an item to list
		/// </summary>
		/// <param name="column"></param>
		public void AddItem(ContextMenuItem item)
		{
			item.ContextMenu = this;
			Items.Add(item);
			StateHasChanged();
		}

		private async Task CloseContextMenu()
		{
			await Task.Delay(200);
			Show = false;
		}
	}
}
