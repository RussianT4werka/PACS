using System;
using System.Collections.Generic;

namespace PACS.Models
{
    public partial class Point
    {
        public Point()
        {
            Events = new HashSet<Event>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Event> Events { get; set; }
    }
}
