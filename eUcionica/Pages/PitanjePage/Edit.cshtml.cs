using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataBaseContext;
using DatabaseEntityLib;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using System.IO;
using System.Net;

namespace eUcionica.Pages.PitanjePage
{
    public class EditModel : PageModel
    {
        private readonly DataBaseContext.DB_Context_Class _context;
        private IWebHostEnvironment _environment;

        public EditModel(DataBaseContext.DB_Context_Class context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public Pitanje Pitanje { get; set; } = default!;

        public SelectList DifficultyOptions { get; set; }

        [BindProperty]
        public IFormFile? Upload { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Pitanje == null)
            {
                return NotFound();
            }

            var pitanje =  await _context.Pitanje
                .Include(p => p.Oblast)
                .Include(p => p.Predmet)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (pitanje == null)
            {
                return NotFound();
            }
            Pitanje = pitanje;
           ViewData["OblastID"] = new SelectList(_context.Oblast, "ID", "Name");
           ViewData["PredmetID"] = new SelectList(_context.Predmet, "ID", "Name");
            var uniqueDifficulties = _context.Pitanje.Select(p => p.Difficulty).Distinct().ToList();
            DifficultyOptions = new SelectList(uniqueDifficulties);
            ViewData["Difficulty"] = DifficultyOptions;
            return Page();
        }

        
        public async Task<IActionResult> OnPostAsync()
        {
            /*if (!ModelState.IsValid)
            {
                return Page();
            }*/

            if (Upload != null)
            {
                var file = Path.Combine(_environment.ContentRootPath, "wwwroot\\images", Upload.FileName);
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                        await Upload.CopyToAsync(fileStream);

                        Pitanje.Image = Upload.FileName;
                    
                }
            }

            _context.Attach(Pitanje).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PitanjeExists(Pitanje.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Pitanja");
        }

        private bool PitanjeExists(int id)
        {
          return (_context.Pitanje?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
