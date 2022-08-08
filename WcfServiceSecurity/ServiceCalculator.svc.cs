using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.ServiceModel;
using System.Text;

namespace WcfServiceSecurity
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ServiceCalculator" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ServiceCalculator.svc or ServiceCalculator.svc.cs at the Solution Explorer and start debugging.
    public class ServiceCalculator : IServiceCalculator
    {
        [PrincipalPermission(SecurityAction.Demand, Role = @"GFT\jsra")]
        public decimal Calculate(decimal value1, decimal value2)
        {
            return value1 * value2;
        }
    }
}
