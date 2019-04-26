using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase.Protocol;

namespace AgpsServer
{
    public class MyRequestInfo : IRequestInfo
    {
        public string Key { get; set; }

        public string value { get; set; }

        public MyRequestInfo()
        {
            this.Key = "ERR";
            this.Key = "123456";
        }

        /*
        // Other properties
        */
    }
}
