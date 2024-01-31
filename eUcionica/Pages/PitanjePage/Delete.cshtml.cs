using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataBaseContext;
using DatabaseEntityLib;

namespace eUcionica.Pages.PitanjePage
{
    public class DeleteModel : PageModel
    {
        private readonly DataBaseContext.DB_Context_Class _context;

        public DeleteModel(DataBaseContext.DB_Context_Class context)
        {
            _context = context;
        }

        [BindProperty]
      public Pitanje Pitanje { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Pitanje == null)
            {
                return NotFound();
            }

            var pitanje = await _context.Pitanje
                .Include(p => p.Oblast)
                .Include(p => p.Predmet)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (pitanje == null)
            {
                return NotFound();
            }
            else 
            {
                Pitanje = pitanje;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Pitanje == null)
            {
                return NotFound();
            }
            var pitanje = await _context.Pitanje
                .FindAsync(id);

            if (pitanje != null)
            {
                Pitanje = pitanje;
                _context.Pitanje.Remove(Pitanje);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Pitanja");
        }
    }
}
