using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.EnvironmentVariables
{
    public class EnvVariable
    {
        public string key { get; set; }
        public string value { get; set; }
        public EnvironmentVariableTarget varType { get; set; }

        public EnvVariable()
        { }

        public EnvVariable(EnvVariable enVar)
        {
            this.key = enVar.key;
            this.value = enVar.value;
            this.varType = enVar.varType;
        }
    }
}
