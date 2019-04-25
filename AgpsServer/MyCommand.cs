using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.Common;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.SocketBase.Command;

namespace AgpsServer
{
    class MyCommand
    {
        public class EEEE : CommandBase<MySession, MyRequestInfo>
        {
            public override void ExecuteCommand(MySession session, MyRequestInfo requestInfo)
            {
                session.Send("EEEE succ {0},{1}!", requestInfo.Key, requestInfo.value);
            }
        }

        public class ECHO : CommandBase<MySession, MyRequestInfo>
        {
            public override void ExecuteCommand(MySession session, MyRequestInfo requestInfo)
            {
                session.Send("ECHO succ!");
            }
        }

        public class APPKEY : CommandBase<MySession, MyRequestInfo>
        {
            public override void ExecuteCommand(MySession session, MyRequestInfo requestInfo)
            {
                string value = Program.KeyDic["appkey"];
                Console.WriteLine("appkey value :{0}", value);
                int i = string.Compare(requestInfo.value, value);
                if (i == 0)
                {
                    Console.WriteLine("APPKEY pwd succ!");
                    session.Send(new ArraySegment<byte>(AgpsClient.AgpsData));
                    Console.WriteLine("Send agps data len {0}", AgpsClient.AgpsData.Length);
                    session.Close();
                }
                else
                {
                    Console.WriteLine("invaild pwd!");
                    session.Close();
                }
            }
        }

        public class SECURENET01 : CommandBase<MySession, MyRequestInfo>
        {
            public override void ExecuteCommand(MySession session, MyRequestInfo requestInfo)
            {
                session.Send("SECURENET01 succ!");
            }
        }

        public class SECURENET : CommandBase<MySession, MyRequestInfo>
        {
            public override void ExecuteCommand(MySession session, MyRequestInfo requestInfo)
            {
                session.Send("SECURENET succ!");
            }
        }

        public class MTKAGPS : CommandBase<MySession, MyRequestInfo>
        {
            public override void ExecuteCommand(MySession session, MyRequestInfo requestInfo)
            {
                session.Send("MTKAGPS succ!");
            }
        }

        public class TDAGPS : CommandBase<MySession, MyRequestInfo>
        {
            public override void ExecuteCommand(MySession session, MyRequestInfo requestInfo)
            {
                session.Send("TDAGPS succ!");
            }
        }

        public class ATAGPS : CommandBase<MySession, MyRequestInfo>
        {
            public override void ExecuteCommand(MySession session, MyRequestInfo requestInfo)
            {
                session.Send("ATAGPS succ!");
            }
        }
    }
}
