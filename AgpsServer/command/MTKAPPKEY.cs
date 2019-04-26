using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase.Command;

namespace AgpsServer.command
{
    public class MTKAPPKEY : CommandBase<MySession, MyRequestInfo>
    {
        public override void ExecuteCommand(MySession session, MyRequestInfo requestInfo)
        {
            session.Send(new ArraySegment<byte>(MtkAgpsData.Data));
            Console.WriteLine("MTK send agps data len {0}", MtkAgpsData.Data.Length);
            session.Close();
        }
    }
}
