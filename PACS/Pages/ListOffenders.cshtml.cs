using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Design;
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

        //private void Timer()
        //{
        //    var timer = new System.Timers.Timer(10000);
        //    timer.Elapsed += OnGet();
        //    timer.AutoReset = true;
        //    timer.Enabled = true;
        //}
        public void OnGet(string handler)
        {
            Admin admin = Session.GetAdmin(handler);
            UserSession = handler;
            Events = _pacsContext.Events.ToList();
            foreach(var time in Events)
            {
                var convert = DateTime.Parse(time.Time);
                time.TimeConverted = convert;
                _pacsContext.Events.Update(time);
                _pacsContext.SaveChanges();
            }
            Events = _pacsContext.Events.Where( s => s.TimeConverted.Value.Hour > 9 && s.TimeConverted.Value.Minute > 30).ToList();
        }
    }
}
