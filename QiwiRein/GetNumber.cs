using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QiwiRein
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class EmailSettings
    {
    }

    public class MobilePinInfo
    {
        public DateTime lastMobilePinChange { get; set; }
        public DateTime nextMobilePinChange { get; set; }
        public bool mobilePinUsed { get; set; }
    }

    public class PassInfo
    {
        public DateTime lastPassChange { get; set; }
        public DateTime nextPassChange { get; set; }
        public bool passwordUsed { get; set; }
    }

    public class PinInfo
    {
        public bool pinUsed { get; set; }
    }

    public class AuthInfo
    {
        public object lastLoginDate { get; set; }
        public long personId { get; set; }
        public DateTime registrationDate { get; set; }
        public object boundEmail { get; set; }
        public EmailSettings emailSettings { get; set; }
        public MobilePinInfo mobilePinInfo { get; set; }
        public PassInfo passInfo { get; set; }
        public PinInfo pinInfo { get; set; }
        public string ip { get; set; }
    }

    public class Root
    {
        public object contractInfo { get; set; }
        public AuthInfo authInfo { get; set; }
        public object userInfo { get; set; }
    }
}
