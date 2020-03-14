using WebWindows.Blazor;

namespace ForDesk
{
	class Program
	{
		static void Main(string[] args)
		{
			ComponentsDesktop.Run<Startup>("BoxOfBlazor - For Desk", "wwwroot/index.html");
		}
	}
}
