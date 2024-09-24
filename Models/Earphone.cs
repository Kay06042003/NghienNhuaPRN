using System;
using System.Collections.Generic;

#nullable disable

namespace Nghien_Nhua.Models
{
    public partial class Earphone
    {
        public int EarId { get; set; }
        public string EarType { get; set; }
        public string EarPlug { get; set; }
        public string EarCompatibility { get; set; }
        public string EarWireLength { get; set; }
        public string EarUtility { get; set; }
        public string EarConnect { get; set; }
        public string EarControl { get; set; }
        public string EarChargingPort { get; set; }
        public string EarConnectTech { get; set; }
        public int ProId { get; set; }

        public virtual Product Pro { get; set; }
    }
}
