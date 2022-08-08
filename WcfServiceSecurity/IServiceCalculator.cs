using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServiceSecurity
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IServiceCalculator" in both code and config file together.
    [ServiceContract]
    public interface IServiceCalculator
    {
        [OperationContract]
        decimal Calculate(decimal value1, decimal value2);
    }
}
