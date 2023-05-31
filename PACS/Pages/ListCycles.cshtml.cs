using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PACS.DB;
using PACS.Models;
using PACS.Tools;

namespace PACS.Pages
{
    public class ListCyclesModel : PageModel
    {
        private readonly pacsContext _pacsContext;
        public List<Cycle> Cycles { get; set; }
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
                Cycles = _pacsContext.Cycles.ToList();
            }
            catch
            {
                return;
            }
        }
    }
}
