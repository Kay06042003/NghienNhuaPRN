using System;
using System.Collections.Generic;

#nullable disable

namespace Nghien_Nhua.Models
{
    public partial class Keycap
    {
        public int KcId { get; set; }
        public string KcMaterial { get; set; }
        public string KcLayout { get; set; }
        public string KcThickness { get; set; }
        public string KcReliability { get; set; }
        public int? ProId { get; set; }

        public virtual Product Pro { get; set; }
    }
}
