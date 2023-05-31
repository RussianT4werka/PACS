using System;
using System.Collections.Generic;

namespace PACS.Models
{
    public partial class Event
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Position { get; set; }
        public string? Dec { get; set; }
        public string? W26 { get; set; }
        public string? Hex { get; set; }
        public string? PassDenyId { get; set; }
        public string? DirName { get; set; }
        public int? PointId { get; set; }
        public string? Time { get; set; }
        public DateTime? TimeConverted { get; set; }

        public virtual Translate? DirNameNavigation { get; set; }
        public virtual Translate? PassDeny { get; set; }
        public virtual Point? Point { get; set; }
    }
}
