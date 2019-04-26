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
    public class TEST : CommandBase<MySession, MyRequestInfo>
    {
        public override void ExecuteCommand(MySession session, MyRequestInfo requestInfo)
        {
            session.Send(requestInfo.Key + requestInfo.value);
        }
    }
}
