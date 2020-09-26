using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC11.Interfaces
{
    public interface ITransaction
    {
        //string CreateHashString();
        string ComputeTransactionHash();
    }
}
