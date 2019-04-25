using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace AgpsServer
{
    public class TEST : StringCommandBase
    {
        public override void ExecuteCommand(AppSession session, StringRequestInfo requestInfo)
        {
            session.Send((int.Parse(requestInfo[0] + int.Parse(requestInfo[1])).ToString()));
        }
    }
}
