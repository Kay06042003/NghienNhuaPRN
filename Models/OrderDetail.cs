using System;
using System.Collections.Generic;

#nullable disable

namespace Nghien_Nhua.Models
{
    public partial class OrderDetail
    {
        public int OdId { get; set; }
        public int? OrderId { get; set; }
        public int? ProId { get; set; }
        public string OdPrice { get; set; }
        public int? OdQuantity { get; set; }
        public string OdTotalMoney { get; set; }

        public virtual Order Order { get; set; }
    }
}
