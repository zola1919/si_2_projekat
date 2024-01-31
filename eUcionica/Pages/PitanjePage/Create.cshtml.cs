using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataBaseContext;
using DatabaseEntityLib;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using System.IO;

namespace eUcionica.Pages.PitanjePage
{
    public class CreateModel : PageModel
    {
        private readonly DataBaseContext.DB_Context_Class _context;
        private IWebHostEnvironment _environment;

        public CreateModel(DataBaseContext.DB_Context_Class context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public Pitanje Pitanje { get; set; } = default!;

        public SelectList PredmetOptions { get; set; }
        public SelectList OblastOptions { get; set; }

        public SelectList DifficultyOptions { get; set; }

        [BindProperty]
        public IFormFile? Upload { get; set; }

        public IActionResult OnGet()
        {
            PredmetOptions = new SelectList(_context.Predmet, "ID", "Name");
            ViewData["PredmetID"] = PredmetOptions;

            OblastOptions = new SelectList(Enumerable.Empty<Oblast>(), "ID", "Name");
            ViewData["OblastID"] = OblastOptions;

            var uniqueDifficulties = _context.Pitanje.Select(p => p.Difficulty).Distinct().ToList();
            DifficultyOptions = new SelectList(uniqueDifficulties);
            ViewData["Difficulty"] = DifficultyOptions;



            return Page();
        }

        public JsonResult OnGetIzaberiOblasti(int predmetID)
        {

            var oblasti = _context.Oblast
                .Where(o => o.PredmetID == predmetID)
                .Select(o => new { id = o.ID, name = o.Name })
                .ToList();

            return new JsonResult(oblasti);
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if ( _context.Pitanje == null || Pitanje == null)
            {
                return Page();
            }
            

            if (Upload != null)
            {
                var file = Path.Combine(_environment.ContentRootPath, "wwwroot\\images", Upload.FileName);
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await Upload.CopyToAsync(fileStream);

                    Pitanje.Image = Upload.FileName;
                }
            }



            _context.Pitanje.Add(Pitanje);
             await _context.SaveChangesAsync();
            
             return RedirectToPage("./Pitanja");
        }
    }
}
