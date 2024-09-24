using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace Nghien_Nhua.MyUtil
{
    public class ConvertFunction
    {
        public int getPriceBeforeDiscount(string discount, string price)
        {
            int discountConvert = int.Parse(discount);
            int priceConvert = int.Parse(price);
            int priceBeforeDiscount = (int)(priceConvert / ((100 - discountConvert) / 100.0));

            return priceBeforeDiscount;
        }

        public string converterNumber(string number)
        {
            // convert number xxx.xxx
            String numberStr = number.ToString();
            String result = "";
            int count = 0;
            for (int i = numberStr.Length - 1; i >= 0; i--)
            {
                count++;
                result = numberStr[i] + result;
                if (count % 3 == 0 && i != 0)
                {
                    result = "," + result;
                }
            }
            return result;
        }
        public string converterNumber(int number)
        {
            // convert number xxx.xxx
            String numberStr = number.ToString();
            String result = "";
            int count = 0;
            for (int i = numberStr.Length - 1; i >= 0; i--)
            {
                count++;
                result = numberStr[i] + result;
                if (count % 3 == 0 && i != 0)
                {
                    result = "," + result;
                }
            }
            return result;
        }
    }
}