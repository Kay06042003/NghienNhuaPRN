using System;
using System.Collections.Generic;

#nullable disable

namespace Nghien_Nhua.Models
{
    public partial class Kit
    {
        public int KitId { get; set; }
        public string KitLayout { get; set; }
        public string KitCircuit { get; set; }
        public string KitPlate { get; set; }
        public string KitMode { get; set; }
        public string KitCase { get; set; }
        public int? ProId { get; set; }

        public virtual Product Pro { get; set; }
    }
}
