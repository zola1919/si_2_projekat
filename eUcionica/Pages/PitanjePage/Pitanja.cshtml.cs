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
    public class PitanjeModel : PageModel
    {
        private readonly DataBaseContext.DB_Context_Class _context;

        [BindProperty]
        public string SearchText { get; set; }

        static bool reversePredmet = false;
        static bool reverseOblast = false;
        static bool reverseDiff = false;

        public PitanjeModel(DataBaseContext.DB_Context_Class context)
        {
            _context = context;
        }

        public IList<Pitanje> Pitanje { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Pitanje != null)
            {
                Pitanje = await _context.Pitanje
                .Include(p => p.Oblast)
                .Include(p => p.Predmet).ToListAsync();
            }
        }

        public async void OnGetSortByPredmet()
        {
            if (reversePredmet)
            {
                Pitanje = await _context.Pitanje.OrderBy(s => s.PredmetID)
                .Include(p => p.Oblast)
                .Include(p => p.Predmet).ToListAsync();
            }
            else
            {
                Pitanje = await _context.Pitanje.OrderByDescending(s => s.PredmetID)
               .Include(p => p.Oblast)
               .Include(p => p.Predmet).ToListAsync();
            }
            reversePredmet = !reversePredmet;
        }
        public async void OnGetSortByOblast()
        {
            if (reverseOblast)
            {
                Pitanje = await _context.Pitanje.OrderBy(s => s.OblastID)
                .Include(p => p.Oblast)
                .Include(p => p.Predmet).ToListAsync();
            }
            else
            {
                Pitanje = await _context.Pitanje.OrderByDescending(s => s.OblastID)
                .Include(p => p.Oblast)
                .Include(p => p.Predmet).ToListAsync();
            }
            reverseOblast = !reverseOblast;
        }
        public async void OnGetSortByDiff()
        {
            if (reverseDiff)
            {
                Pitanje = await _context.Pitanje.OrderBy(s => s.Difficulty)
                .Include(p => p.Oblast)
                .Include(p => p.Predmet).ToListAsync();
            }
            else
            {
                Pitanje = await _context.Pitanje.OrderByDescending(s => s.Difficulty)
                .Include(p => p.Oblast)
                .Include(p => p.Predmet).ToListAsync();
            }
                reverseDiff = !reverseDiff;
        }
        public async void OnPost()
        {
            if (SearchText == null)
            {
                Pitanje = await _context.Pitanje
                .Include(p => p.Oblast)
                .Include(p => p.Predmet).ToListAsync();
            }
            else
            {
                Pitanje = await _context.Pitanje.Where(s => EF.Functions.Like(s.Predmet.Name, $"%{SearchText}%") || EF.Functions.Like(s.Oblast.Name, $"%{SearchText}%") || EF.Functions.Like(s.Difficulty, $"%{SearchText}%"))
                .Include(p => p.Oblast)
                .Include(p => p.Predmet).ToListAsync();
            }
        }
    }
}
