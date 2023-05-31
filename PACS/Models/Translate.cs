using System;
using System.Collections.Generic;

namespace PACS.Models
{
    public partial class Translate
    {
        public Translate()
        {
            EventDirNameNavigations = new HashSet<Event>();
            EventPassDenies = new HashSet<Event>();
        }

        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;

        public virtual ICollection<Event> EventDirNameNavigations { get; set; }
        public virtual ICollection<Event> EventPassDenies { get; set; }
    }
}
