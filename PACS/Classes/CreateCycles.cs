using PACS.DB;
using PACS.Models;

namespace PACS.Classes
{
    public class CreateCycles
    {
        public static void CC(pacsContext _pacsContext, List<Event> Events, Cycle lastCycle, List<Cycle> Cycles)
        {
            foreach (var events in Events.Where(s => s.PointId == 1))
            {
                var cycleP2 = _pacsContext.Cycles.FirstOrDefault(s => s.TimeP2 == null);
                if (events != null && cycleP2 == null)
                {
                    lastCycle = _pacsContext.Cycles.ToList().LastOrDefault();
                    if (Cycles.Count() == 0)
                    {
                        var newCycle = new Cycle() { EventId = events.Id, W26 = events.W26, TimeP1 = events.TimeConverted };
                        _pacsContext.Cycles.Add(newCycle);
                        _pacsContext.SaveChanges();
                        return;
                    }
                    bool aa = Cycles.Any(s => s.EventId == events.Id);
                    if (lastCycle.EventId != events.Id && aa == false)
                    {
                        var newCycle = new Cycle() { EventId = events.Id, W26 = events.W26, TimeP1 = events.TimeConverted };
                        _pacsContext.Cycles.Add(newCycle);
                        _pacsContext.SaveChanges();
                        return;
                    }
                }
                else
                {
                    var ff = _pacsContext.Cycles.FirstOrDefault(s => s.TimeP2 == null);

                    if (ff != null)
                    {
                        foreach (var events2 in Events.Where(s => s.PointId == 2 && s.W26 == ff.W26))
                        {
                            bool gg = _pacsContext.Cycles.Any(s => s.TimeP1 > events2.TimeConverted);
                            if (gg == false)
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
    }
}
