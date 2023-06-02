using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PACS.Classes;
using PACS.DB;
using PACS.Models;
using PACS.Tools;
using System;

namespace PACS.Pages
{
    public class ListOffendersModel : PageModel
    {
        public string UserSession { get; set; }
        private readonly pacsContext _pacsContext;
        public List<Event> Events { get; set; }
        public Event Event { get; set; }
        public ListOffendersModel(pacsContext pacsContext)
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
                Events = _pacsContext.Events.Include(s => s.Point).Include(s => s.DirNameNavigation).Include(s => s.PassDeny).ToList();
            }
            catch
            {
                return;
            }
            
        }
    }
}
