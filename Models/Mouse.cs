using System;
using System.Collections.Generic;

#nullable disable

namespace Nghien_Nhua.Models
{
    public partial class Mouse
    {
        public int MouseId { get; set; }
        public string MouseDpi { get; set; }
        public string MouseWireLength { get; set; }
        public string MouseLed { get; set; }
        public string MouseTypeBattery { get; set; }
        public string MouseWeight { get; set; }
        public string MouseCompatibility { get; set; }
        public int? ProId { get; set; }

        public virtual Product Pro { get; set; }
    }
}
