using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataBaseContext;
using DatabaseEntityLib;

namespace eUcionica.Pages.OblastPage
{
    public class OblastModel : PageModel
    {
        private readonly DataBaseContext.DB_Context_Class _context;

        public OblastModel(DataBaseContext.DB_Context_Class context)
        {
            _context = context;
        }

        [BindProperty]
        public string SearchText { get; set; }
        static bool reversePredmet = false;

        public IList<Oblast> Oblast { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Oblast != null)
            {
                Oblast = await _context.Oblast
                .Include(o => o.Predmet).ToListAsync();
            }
        }

        public async void OnGetSortByPredmet()
        {
            if (reversePredmet)
            {
                Oblast = await _context.Oblast.OrderBy(s => s.PredmetID)
                .Include(p => p.Predmet).ToListAsync();
            }
            else
            {
                Oblast = await _context.Oblast.OrderByDescending(s => s.PredmetID)
               .Include(p => p.Predmet).ToListAsync();
            }
            reversePredmet = !reversePredmet;
        }

        public async void OnPost()
        {
            if (SearchText == null)
            {
                Oblast = await _context.Oblast
                .Include(p => p.Predmet).ToListAsync();
            }
            else
            {
                Oblast = await _context.Oblast.Where(s => EF.Functions.Like(s.Predmet.Name, $"%{SearchText}%") || EF.Functions.Like(s.Name, $"%{SearchText}%"))
                .Include(p => p.Predmet).ToListAsync();
            }
        }
    }
}
