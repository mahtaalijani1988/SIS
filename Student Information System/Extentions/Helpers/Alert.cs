using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Student_Information_System.Extentions.Helpers
{
    public enum AlertMode
    {
        Info,
        Success,
        Error
    }
    public class Alert
    {
        public AlertMode Mode { get; set; }
        public string Message { get; set; }
    }
}