using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QiwiRein
{
    public class Sum
    {
        public double amount { get; set; }
        public string currency { get; set; }
    }
    public class PaymentMethod
    {
        public string type { get; set; }
        public string accountId { get; set; }
    }
    public class Fields
    {
        public string account { get; set; }
    }
    public class Root_2
    {
        public string id { get; set; }
        public Sum sum { get; set; }
        public PaymentMethod paymentMethod { get; set; }
        public string comment { get; set; }
        public Fields fields { get; set; }
    }
}
