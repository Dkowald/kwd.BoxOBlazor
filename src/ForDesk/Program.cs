using WebWindows.Blazor;

namespace ForDesk
{
	static class Program
	{
		static void Main()
		{
			ComponentsDesktop.Run<Startup>("BoxOfBlazor - For Desk", "wwwroot/index.html");
		}
	}
}
