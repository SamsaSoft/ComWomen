using Admin.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Admin.Pages.Medias
{
    public class IndexModel : BasePageModel
    {
        [BindProperty]
        public IEnumerable<Media> Medias { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await Task.CompletedTask;
            return Page();
        }
    }
}
