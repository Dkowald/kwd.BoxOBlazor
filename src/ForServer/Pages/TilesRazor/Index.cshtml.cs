using ForServer.Pages.Shared.Shell;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ForServer.Pages.Tiles
{
    public class TilesModel : PageModel
    {
        public TilesModel()
        {
            var longest =
                "Created signs. Fruit second land isn't, moveth land fish greater i he life so you're. Great subdue was land the firmament yielding likeness were ";
            longest = longest + longest.Length + "midst creature it gathering. I in fly third shall is.";
            
            Desk = new Desk
            {
                Items = new[]
                {
                    new DeskTile
                    {
                        Title = "JS Interop", 
                        Description = "Demo basic javascript interop, with a prompt",
                        OpenUrl = "Tiles/Interop"
                    },
                    new DeskTile
                    {
                        Title = "Color tool",
                        Description = "Demo richer blazor updates as a simple color tool",
                        OpenUrl = "Tiles/ColorTool"
                    },
                    new DeskTile
                    {
                        Title = "Alarm clock",
                        Description = "Demo richer blazor updates as a simple color tool",
                        Icon = "/img/alarmClock.svg",
                        OpenUrl = "Tiles/AlarmClock"
                    },
                    new DeskTile {
                        Title = "Misbehave", 
                        Description = "Behaviour with various errors",
                        OpenUrl = "Tiles/Misbehave"
                    },
                    new DeskTile
                    {
                        Title = "Log viewer", 
                        Description = "View latest logging events",
                        OpenUrl = "Tiles/LogView"
                    },
                    new DeskTile
                    {
                        Title = "Local storage", 
                        Description = "Using local storage for in-browser data",
                        OpenUrl = "Tiles/LocalStorage"
                    //},
                    //new DeskTile {Title = "App 1", Description = "A sample app", OpenUrl = TileUrl("App1")},
                    //new DeskTile {Title = "App 2", Description = longest},
                    //new DeskTile {Title = "App 3", Description = "Another"}
                    }
                }
            };
        }

        public readonly Desk Desk;

        private string TileUrl(string name)
            => "/Tiles/" + name;
    }
}
