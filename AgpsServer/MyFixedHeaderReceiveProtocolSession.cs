﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;

namespace AgpsServer
{
    public class MyFixedHeaderReceiveProtocolSession : AppSession<MyFixedHeaderReceiveProtocolSession, BinaryRequestInfo>
    {
        protected override void HandleException(Exception e)
        {
            this.Send("HandleException");
        }

        // 重载OnSessionStarted函数，等同于appServer.NewSessionConnected += NewSessionConnected
        protected override void OnSessionStarted()
        {
            // 会话链接成功后的逻辑部分。
            this.Send("Welcome to SuperSocket FiexdHeader Server");
            //this.Close();
        }

        protected override void HandleUnknownRequest(BinaryRequestInfo requestInfo)
        {
            // 收到未知请求的逻辑部分
            this.Send("Unknow request");
        }

        protected override void OnSessionClosed(CloseReason reason)
        {
            // 会话关闭后的逻辑代码
            Console.WriteLine("close session!");
            base.OnSessionClosed(reason);
        }
    }
}
