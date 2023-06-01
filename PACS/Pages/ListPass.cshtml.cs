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

            //lastEventP1 = _pacsContext.Events.ToList().LastOrDefault(s => s.PointId == 1);
            //lastEventP2 = _pacsContext.Events.ToList().LastOrDefault(s => s.PointId == 2);
            //lastCycle = _pacsContext.Cycles.ToList().LastOrDefault(s => s.TimeP2 == null && s.W26 == lastEventP1.W26);

            //if (lastEventP1 != null && lastCycle == null)
            //{
            //    var newCycle = new Cycle() { EventId = lastEventP1.Id, W26 = lastEventP1.W26, TimeP1 = lastEventP1.TimeConverted };
            //    _pacsContext.Cycles.Add(newCycle);
            //    _pacsContext.SaveChanges();
            //}
            //else
            //{

            //}
            Cycles = _pacsContext.Cycles.ToList();
            
            foreach(var events in Events.Where( s => s.PointId == 1))
            {
                var cycleP2 = _pacsContext.Cycles.FirstOrDefault(s => s.TimeP2 == null);
                if(events != null && cycleP2 == null)
                {
                    lastCycle = _pacsContext.Cycles.ToList().LastOrDefault();
                    if (Cycles.Count() == 0)
                    {
                        var newCycle = new Cycle() { EventId = events.Id, W26 = events.W26, TimeP1 = events.TimeConverted };
                        _pacsContext.Cycles.Add(newCycle);
                        _pacsContext.SaveChanges();
                        return;
                    }
                    else if(lastCycle.EventId != events.Id)
                    {
                        var newCycle = new Cycle() { EventId = events.Id, W26 = events.W26, TimeP1 = events.TimeConverted };
                        _pacsContext.Cycles.Add(newCycle);
                        _pacsContext.SaveChanges();
                        return;
                    }
                }
                else
                {
                    foreach (var events2 in Events.Where(s => s.PointId == 2))
                    {
                        foreach (var cycle in Cycles.Where(s => s.TimeP1 != null && s.TimeP2 == null && s.W26 == events2.W26))
                        {
                            cycle.TimeP2 = events2.TimeConverted;
                            cycle.Delta = cycle.TimeP2 - cycle.TimeP1;
                            _pacsContext.Cycles.Update(cycle);
                            _pacsContext.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}
