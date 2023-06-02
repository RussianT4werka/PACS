using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PACS.Classes;
using PACS.DB;
using PACS.Models;
using PACS.Tools;

namespace PACS.Pages
{
    public class ListCyclesModel : PageModel
    {
        private readonly pacsContext _pacsContext;
        private Cycle lastCycle;
        public List<Cycle> Cycles { get; set; }
        public List<Event> Events { get; set; }
        public string UserSession { get; set; }

        public ListCyclesModel(pacsContext pacsContext)
        {
            _pacsContext = pacsContext;
        }
        public void OnGet(string handler)
        {
            Admin admin = Session.GetAdmin(handler);
            UserSession = handler;
            try
            {
                Events = _pacsContext.Events.ToList();
                Time.ConvertTime(_pacsContext, Events);
                Cycles = _pacsContext.Cycles.Include( s => s.Event).ToList();
                CreateCycles.CC(_pacsContext, Events, lastCycle, Cycles);
            }
            catch
            {
                return;
            }
            
        }
    }
}
