using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.Sooil.IServiceBase
{
    [Serializable]
    public class ContributorType
    {
        public const string Sponsor = "Sponsor";
        public const string Creator = "Creator";
        public const string Auditor = "Auditor";
        public const string Publisher = "Publisher";
        public const string Organization = "Organization";
    }
}
