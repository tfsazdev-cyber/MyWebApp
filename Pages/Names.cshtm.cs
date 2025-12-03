using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace MyWebApp.Pages
{
    public class NamesModel : PageModel
    {
        public List<string> Names { get; set; } = new();

        public void OnGet()
        {
            Names = new List<string>
            {
                "Rama",
                "Bob",
                "Charlie",
                "Diana",
                "Eve"
            };
        }
    }
}