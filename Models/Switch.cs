using System;
using System.Collections.Generic;

#nullable disable

namespace Nghien_Nhua.Models
{
    public partial class Switch
    {
        public int SwitchId { get; set; }
        public string SwitchPin { get; set; }
        public string SwitchType { get; set; }
        public string SwitchSpring { get; set; }
        public string SwitchReliability { get; set; }
        public string SwitchDepth { get; set; }
        public int? ProId { get; set; }

        public virtual Product Pro { get; set; }
    }
}
