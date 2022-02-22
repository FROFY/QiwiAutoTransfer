using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QiwiRein
{
    public class Type
    {
        public string id { get; set; }
        public string title { get; set; }
    }

    public class Balance
    {
        public double amount { get; set; }
        public int currency { get; set; }
    }

    public class Account
    {
        public string alias { get; set; }
        public string fsAlias { get; set; }
        public string bankAlias { get; set; }
        public string title { get; set; }
        public Type type { get; set; }
        public bool hasBalance { get; set; }
        public Balance balance { get; set; }
        public int currency { get; set; }
    }

    public class Root_1
    {
        public List<Account> accounts { get; set; }
    }


}
