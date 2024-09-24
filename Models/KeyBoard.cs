using System;
using System.Collections.Generic;

#nullable disable

namespace Nghien_Nhua.Models
{
    public partial class KeyBoard
    {
        public int KbId { get; set; }
        public string KbLed { get; set; }
        public string KbMode { get; set; }
        public string KbSwitch { get; set; }
        public string KbKeycap { get; set; }
        public string KbPlate { get; set; }
        public string KbCase { get; set; }
        public int ProId { get; set; }

        public virtual Product Pro { get; set; }
    }
}
