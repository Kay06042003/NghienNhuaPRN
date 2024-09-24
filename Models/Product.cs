using System;
using System.Collections.Generic;

#nullable disable

namespace Nghien_Nhua.Models
{
    public partial class Product
    {
        public Product()
        {
            Carts = new HashSet<Cart>();
        }

        public int ProId { get; set; }
        public string ProName { get; set; }
        public int? ProQuantity { get; set; }
        public string ProPrice { get; set; }
        public string ProImage { get; set; }
        public string ProDescription { get; set; }
        public string ProDiscount { get; set; }
        public DateTime? ProDate { get; set; }
        public string ProCategory { get; set; }
        public string ProBrand { get; set; }
        public string ProOrigin { get; set; }

        public virtual Earphone Earphone { get; set; }
        public virtual KeyBoard KeyBoard { get; set; }
        public virtual Keycap Keycap { get; set; }
        public virtual Kit Kit { get; set; }
        public virtual Mouse Mouse { get; set; }
        public virtual Switch Switch { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
    }
}
