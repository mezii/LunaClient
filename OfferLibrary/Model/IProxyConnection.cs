using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfferLibrary.Model
{
    public interface IProxyConnection
    {
        void GetProxies( );
        void GetCountries( );

        void PutCountries();
        void PutProxies();

    }
}
