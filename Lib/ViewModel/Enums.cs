using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class Enums
    {
        public enum ModifyResult
        {
            Created,
            Updated,
            Removed,
            Failed,
            AccessDenied,
            NotFound,
            Unknown
        }
    }
}
