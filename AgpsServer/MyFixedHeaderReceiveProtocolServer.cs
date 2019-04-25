using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.SocketBase.Config;

namespace AgpsServer
{
    public class MyFixedHeaderReceiveProtocolServer : AppServer<MyFixedHeaderReceiveProtocolSession, BinaryRequestInfo>
    {
        public MyFixedHeaderReceiveProtocolServer()
            : base(new DefaultReceiveFilterFactory<MyFixedHeaderReceiveFilter, BinaryRequestInfo>())
        {

        }

        protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
        {
            // 对家配置文件进行相应的修改。
            return base.Setup(rootConfig, config);
        }

        protected override void OnStartup()
        {
            // 服务器启动的逻辑部分
            base.OnStartup();
        }

        protected override void OnStopped()
        {
            // 停止服务器的逻辑部分
            base.OnStopped();
        }
    }
}
