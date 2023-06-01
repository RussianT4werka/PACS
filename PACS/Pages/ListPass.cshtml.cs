using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PACS.Classes;
using PACS.DB;
using PACS.Models;
using PACS.Tools;
using System.Security.Cryptography.X509Certificates;

namespace PACS.Pages
{
    public class ListPassModel : PageModel
    {
        public string UserSession { get; set; }
        public List<Event> Events { get; set; }
        public pacsContext _pacsContext { get; }
        public Cycle Cycle { get; set; }
        public List<Cycle> Cycles { get; set; }

        private Event lastEventP1;
        private Event lastEventP2;
        private Cycle lastCycle;
        public ListPassModel(pacsContext pacsContext)
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
            }
            catch
            {
                return;
            }
            
            Events = _pacsContext.Events.Include(s => s.Point).Include(s => s.DirNameNavigation).Include(s => s.PassDeny).Where(s => s.TimeConverted.Value.Date == DateTime.Now.Date).ToList();

        }
    }
}
