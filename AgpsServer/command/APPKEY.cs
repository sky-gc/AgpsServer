using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase.Command;

namespace AgpsServer.command
{
    public class APPKEY : CommandBase<MySession, MyRequestInfo>
    {
        public override void ExecuteCommand(MySession session, MyRequestInfo requestInfo)
        {
            session.Send(new ArraySegment<byte>(AgpsClient.AgpsData));
            Console.WriteLine("Send agps data len {0}", AgpsClient.AgpsData.Length);
            session.Close();
        }
    }
}
