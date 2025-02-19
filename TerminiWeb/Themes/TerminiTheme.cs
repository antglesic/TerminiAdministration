using MudBlazor;

namespace TerminiWeb.Themes
{
	public class TerminiTheme
	{
		#region Fields

		private MudTheme theme;

		#endregion

		#region Constructor

		public TerminiTheme()
		{
			theme = new MudTheme()
			{
				PaletteLight = new PaletteLight()
				{
					Primary = Colors.LightGreen.Darken2,
					Secondary = Colors.Green.Accent4,
					AppbarBackground = Colors.LightGreen.Darken1,
				},
				PaletteDark = new PaletteDark()
				{
					Primary = Colors.Green.Darken1,
					Secondary = Colors.Green.Accent4,
					AppbarBackground = Colors.Green.Darken3,
					Surface = Colors.Green.Darken4
				},

				LayoutProperties = new LayoutProperties()
				{
					DrawerWidthLeft = "260px",
					DrawerWidthRight = "300px"
				}
			};
		}

		#endregion

		#region GetTheme

		public MudTheme Get
		{
			get
			{
				return theme;
			}
		}

		#endregion
	}
}
