using System.Collections.Generic;

namespace CommonUtils.Email
{
    public class EmailObject
    {
        public string FromAddress { get; set; }
        public string EmailSubject { get; set; }
        public string EmailTo { get; set; }
        public List<string> BccAddresses { get; set; }
        public List<string> CcAddresses { get; set; }
        public string EmailBody { get; set; }
        public string EmailUsername { get; set; }
        public string EmailPassword { get; set; }
        public List<string> ReplyToList { get; set; }
        public string HostAddress { get; set; }
        public int HostPort { get; set; }
        public List<string> ImagePaths {get; set;}
    }
}
