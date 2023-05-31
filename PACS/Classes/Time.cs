using Microsoft.Extensions.Logging;
using PACS.DB;
using PACS.Models;
using System.Collections.Generic;

namespace PACS.Classes
{
    public static class Time
    {
        public static void ConvertTime(pacsContext _pacsContext, List<Event> Events)
        {
            foreach (var time in Events)
            {
                var convert = DateTime.Parse(time.Time);
                time.TimeConverted = convert;
                _pacsContext.Events.Update(time);
                _pacsContext.SaveChanges();
            }
        }
    }
}
