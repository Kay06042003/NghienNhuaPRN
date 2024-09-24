using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nghien_Nhua.Models
{
    public class ProductKeyboard
    {
        public Product Product { get; set; }
        public KeyBoard Keyboard { get; set; }
        public Mouse mouse { get; set; }
        public Earphone earphone { get; set; }
        public Switch switchs { get; set; }
        public Kit kit { get; set; }
        public Keycap keycap { get; set; }

    }
}