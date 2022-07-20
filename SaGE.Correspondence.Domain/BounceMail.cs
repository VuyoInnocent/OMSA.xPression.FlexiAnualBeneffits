using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaGE.Correspondence.Domain
{
    public class BounceMail
    {
        public string EmlFile { get; set; }
        public string Subject { get; set; }
        public string MailAddress { get; set; }
        public string BounceCategory { get; set; }
        public string BounceType { get; set; }
        public string FileDeleted { get; set; }
        public string Action { get; set; }
        public string DiagCode { get; set; }
        public string TimeStamp { get; set; }
    }
}
