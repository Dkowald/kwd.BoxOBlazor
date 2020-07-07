using System.Collections.Generic;
using ForServer.Pages.Shared.Shell;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ForServer.Pages.Shell
{
    public class ShellModel : PageModel
    {
        public ShellModel()
        {
            var longest =
                "Created signs. Fruit second land isn't, moveth land fish greater i he life so you're. Great subdue was land the firmament yielding likeness were ";
            longest = longest + longest.Length + "midst creature it gathering. I in fly third shall is.";

            Tiles = new[]
            {
                new DeskTile
                {
                    Title = "JS Interop",
                    Description = "Demo basic javascript interop, with a prompt"
                }, 

                new DeskTile
                {
                    Title = "Color tool",
                    Description = "Demo richer blazor updates as a simple color tool"
                },
                new DeskTile
                {
                    Title = "Alarm clock",
                    Description = "Demo richer blazor updates as a simple color tool",
                    OpenUrl = "/Demo/AlarmClock"
                },
                new DeskTile
                {
                    Title = "Misbehave",
                    Description = "Behaviour with various errors"
                },
                new DeskTile
                {
                    Title = "Log viewer",
                    Description = "View latest logging events"
                },
                new DeskTile
                {
                    Title = "Local storage",
                    Description = "Using local storage for in-browser data"
                },

                new DeskTile
                {
                    Title = "App 1",
                    Description = "A sample app",
                    OpenUrl = "/Shell/App1"
                },
                new DeskTile
                {
                    Title = "App 2",
                    Description = longest
                },
                new DeskTile
                {
                Title = "App 3",
                Description = "Another"
                }
            };
        }

        public IReadOnlyCollection<DeskTile> Tiles;

        public void OnGet()
        {}
    }
}
