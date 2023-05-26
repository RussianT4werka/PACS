using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PACS.DB;
using PACS.Models;
using PACS.Tools;

namespace PACS.Pages
{
    public class ListPassModel : PageModel
    {
        public string UserSession { get; set; }
        public List<Event> Events { get; set; }
        public pacsContext _pacsContext { get; }
        public ListPassModel(pacsContext pacsContext)
        {
            _pacsContext = pacsContext;
        }
        public void OnGet(string handler)
        {
            Admin admin = Session.GetAdmin(handler);
            UserSession = handler;
            Events = _pacsContext.Events.Include(s => s.Point).ToList();
        }
    }
}
