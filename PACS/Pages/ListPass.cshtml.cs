using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PACS.Classes;
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
        public Cycle Cycle { get; set; }
        private TimeSpan? delta = new TimeSpan(0, 30, 0);
        private Cycle lastCycle;
        private Event lastCarTimeP2;
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
            
            var lastCarTimeP1 = _pacsContext.Events.ToList().LastOrDefault(s => s.PointId == 1);
            lastCarTimeP2 = _pacsContext.Events.ToList().LastOrDefault(s => s.PointId == 2);

            var cycle = _pacsContext.Cycles.ToList().LastOrDefault(s => s.W26 == lastCarTimeP1.W26 && s.TimeP2 == null);
            var last = _pacsContext.Cycles.ToList().LastOrDefault();
            if (cycle == null )
            {
                if(lastCarTimeP1 != null)
                {
                    if (last != null && last.W26 == lastCarTimeP1.W26 && last.TimeP1 == lastCarTimeP1.TimeConverted && last.TimeP2 == lastCarTimeP2.TimeConverted)
                    {
                        return;
                    }
                    else
                    {
                        Cycle = new Cycle() { W26 = lastCarTimeP1.W26, TimeP1 = lastCarTimeP1.TimeConverted };
                        _pacsContext.Cycles.Add(Cycle);
                        _pacsContext.SaveChanges();
                    }
                    return;
                }
                return;
            }
            else
            {
                var lastCyclePer = _pacsContext.Cycles.ToList().LastOrDefault(s => s.TimeP2 != null);
                if(lastCarTimeP2 == null)
                {
                    return;
                }
                else
                {
                    if (lastCyclePer == null)
                    {
                        var lastCycleP1 = _pacsContext.Cycles.ToList().LastOrDefault(s => s.TimeP1 != null);
                        lastCycleP1.TimeP2 = lastCarTimeP2.TimeConverted;
                        _pacsContext.Cycles.Update(lastCycleP1);
                        _pacsContext.SaveChanges();
                        lastCycleP1.Delta = lastCycleP1.TimeP2 - lastCycleP1.TimeP1;
                        _pacsContext.Cycles.Update(lastCycleP1);
                        _pacsContext.SaveChanges();
                    }
                    else
                    {
                        if (lastCarTimeP2.TimeConverted == lastCyclePer.TimeP2)
                        {
                            return;
                        }
                        else
                        {
                            lastCycle = _pacsContext.Cycles.ToList().LastOrDefault(s => s.TimeP2 == null);
                            if (lastCycle != null && lastCycle.TimeP2 != lastCarTimeP2.TimeConverted)
                            {
                                lastCycle.TimeP2 = lastCarTimeP2.TimeConverted;
                                _pacsContext.Cycles.Update(lastCycle);
                                _pacsContext.SaveChanges();
                                lastCycle.Delta = lastCycle.TimeP2 - lastCycle.TimeP1;
                                _pacsContext.Cycles.Update(lastCycle);
                                _pacsContext.SaveChanges();
                            }
                            else
                            {
                                return;
                            }
                            return;
                        }
                    }
                }
                
                
            }
            
            
        }
    }
}
