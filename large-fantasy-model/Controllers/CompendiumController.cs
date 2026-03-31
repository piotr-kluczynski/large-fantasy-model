using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using large_fantasy_model.Models;

namespace large_fantasy_model.Controllers
{
    public class CompendiumController : Controller
    {
        public IActionResult Index()
        {
            var rulebooks = new List<GameModeRulebook>
            {
                new GameModeRulebook
                {
                    Id = 1,
                    Title = "Deklaracja Projektowa",
                    Description = "Nasz Projekt Tworzony przez dwóch cudownych przystojnych dżentelmenów.",
                    IconEmoji = "",
                    PdfFileName = "Aplikacja Internetowa do Gier Fabularnych.pdf"
                },
                new GameModeRulebook
                {
                    Id = 2,
                    Title = "Sprawozdanie z sieci 2",
                    Description = "No równie piekne sprawozdanie tak?.",
                    IconEmoji = "",
                    PdfFileName = "Sprawozdanie02_Kluczyński_Małecki.pdf"
                },
                new GameModeRulebook
                {
                    Id = 3,
                    Title = "Sprawozdanie z sieci 4",
                    Description = "Nie mam pomyslu na opis ten tego tego ten.",
                    IconEmoji = "",
                    PdfFileName = "Sprawozdanie04_Kluczyński_Małecki.pdf"
                }
            };

            return View(rulebooks);
        }
    }
}
