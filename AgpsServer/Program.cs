using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket;
using SuperSocket.Common;
using SuperSocket.Facility;
using SuperSocket.SocketBase;
using SuperSocket.SocketEngine;
using SuperSocket.SocketService;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Command;
using log4net;
using System.Configuration;
using SuperSocket.Facility.Protocol;
using System.Data;
using System.Data.Linq;
using System.Collections;
using System.Timers;

namespace AgpsServer
{
    class Program
    {
        // Create a logger for use in this class
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //获取Configuration对象
        private static Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        //public static List<string> Keys = new List<string>();
        //public static List<string> Pwds = new List<string>();

        public static Dictionary<string, string> KeyDic = new Dictionary<string, string>();

        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start the agps client!");

            Console.ReadKey();
            Console.WriteLine();
            
            //测试代码
            //byte[] rsp = { 0x30, 0x00, 0x31, 0x00, 0x1D, 0x60, 0xD1, 0x79, 0x80, 0xA0, 0xA1 };
            //byte[] rsp1 = { 0x40, 0x00, 0x41, 0x00, 0x1D, 0x60, 0xD1, 0x79, 0x80, 0xB0, 0xB1, 0xC0, 0xCF, 0xFE, 0xFF };
            //string utfString = Encoding.UTF8.GetString(rsp, 0, rsp.Length);
            //string utfString1 = Encoding.ASCII.GetString(rsp, 0, rsp.Length);
            //string str = Encoding.GetEncoding(936).GetString(rsp, 0, rsp.Length);
            //Console.WriteLine(utfString);
            //Console.WriteLine(utfString1);
            //Console.WriteLine(str);
            //byte[] rsp2 = new byte[10];
            //Console.WriteLine(rsp2.Length);
            //List< byte > ls = new List<byte>();
            //ls.AddRange(rsp);
            //ls.AddRange(rsp1);
            //rsp2 = ls.ToArray();
            //Console.WriteLine(rsp2.Length);
            //ArrayList arrLi = new ArrayList();
            //arrLi.AddRange(rsp);
            //Console.WriteLine(arrLi.ToArray().Length);
            //arrLi.AddRange(rsp1);
            //Console.WriteLine(arrLi.ToArray().Length);
            //byte[] tmpRsp = { 0x60, 0xD1 };
            //var i = arrLi.IndexOf(tmpRsp);
            //Console.WriteLine(i);
            //byte tmpRsp1 = 0x60;
            //var j = arrLi.IndexOf(tmpRsp1);
            //Console.WriteLine(j);
            //Console.WriteLine(Array.IndexOf(rsp, tmpRsp));

            //// 注意是TelnetServer
            //var appServer = new TelnetServer();
            //appServer.Setup(2012);
            //// 开始监听
            //appServer.Start();
            //while (Console.ReadKey().KeyChar != 'q')
            //{
            //    Console.WriteLine();
            //    continue;
            //}
            //// 停止服务器。
            //appServer.Stop();

            //启动客户端
            string host = "agps.u-blox.com";
            int port = 46434;
            AgpsClient client = new AgpsClient();
            client.StartClient(host, port);
            Console.WriteLine("Ublox Agps data len {0}", AgpsClient.AgpsData.Length);

            //MTK AGPS data
            MtkAgpsData mtkAgps = new MtkAgpsData();
            mtkAgps.update();
            Console.WriteLine("Mtk Agps data len {0}", MtkAgpsData.Data.Length);

            //启动定时器，定时更新AGPS数据。
            System.Timers.Timer agpsUbloxTimer = new System.Timers.Timer();
            agpsUbloxTimer.AutoReset = true;
            agpsUbloxTimer.Enabled = true;
            agpsUbloxTimer.Interval = 600000;
            agpsUbloxTimer.Elapsed += new System.Timers.ElapsedEventHandler(AgpsUbloxHandle);
            agpsUbloxTimer.Start();

            Console.WriteLine("Press any key to start the agps server!");
            Console.ReadKey();
            Console.WriteLine();

            //读取配置文件
            //ConfigurationSection accounts = config.AppSettings.Settings());
            //Console.WriteLine(accounts.ToString());
            //判断App.config配置文件中是否有key
            if (ConfigurationManager.AppSettings.HasKeys())
            {
                //List<string> theKeys = new List<string>();      //保存key的集合
                //List<string> theValues = new List<string>();    //保存value的集合
                ////遍历出所有的key并添加进thekeys集合
                //foreach (string theKey in ConfigurationManager.AppSettings.Keys)
                //{
                //    theKeys.Add(theKey);
                //}
                ////根据key遍历出所有的value并添加进theValues集合
                //for (int i = 0; i < theKeys.Count; i++)
                //{
                //    foreach (string theValue in ConfigurationManager.AppSettings.GetValues(theKeys[i]))
                //    {
                //        theValues.Add(theValue);
                //    }
                //}
                ////验证key和value
                //for (int i = 0; i < theKeys.Count; i++)
                //{
                //    Console.WriteLine("Account: {0}, {1}", theKeys[i], theValues[i]);
                //}
                //遍历出所有的key并添加进thekeys集合
                //foreach (string theKey in ConfigurationManager.AppSettings.Keys)
                //{
                //    Keys.Add(theKey);
                //}
                ////根据key遍历出所有的value并添加进theValues集合
                //for (int i = 0; i < Keys.Count; i++)
                //{
                //    foreach (string theValue in ConfigurationManager.AppSettings.GetValues(Keys[i]))
                //    {
                //        Pwds.Add(theValue);
                //    }
                //}
                //for (int i = 0; i < Keys.Count; i++)
                //{
                //    Console.WriteLine("Account: {0},{1}", Keys[i], Pwds[i]);
                //}
                foreach(string key in ConfigurationManager.AppSettings.Keys)
                {
                    string value = ConfigurationManager.AppSettings.GetValue(key);
                    KeyDic.Add(key, value);
                }
                foreach (KeyValuePair<string, string> kvp in KeyDic)
                {
                    Console.WriteLine("KeyDic: {0},{1}", kvp.Key, kvp.Value);
                }
            }

            // 通过工厂创建一个加载器。
            var bootstrap = BootstrapFactory.CreateBootstrap();
            if (!bootstrap.Initialize())
            {
                Console.WriteLine("Failed to initialize!");
                Console.ReadKey();
                return;
            }
            
            var result = bootstrap.Start();
            Console.WriteLine("Start result : {0}!", result);
            log.Debug("Start result");
            log.Info("Start succ!");
            if (result == StartResult.Failed)
            {
                Console.WriteLine("Failed to start!");
                Console.ReadKey();
                return;
            }
            
            while (Console.ReadKey().KeyChar != 'q')
            {
                Console.WriteLine();
                continue;
            }
            // 停止服务器
            bootstrap.Stop();

            //var appServer = new AppServer();

            ////Setup the appServer
            //if (!appServer.Setup(2012)) //Setup with listening port
            //{
            //    Console.WriteLine("Failed to setup!");
            //    Console.ReadKey();
            //    return;
            //}

            //Console.WriteLine();

            ////Try to start the appServer
            //if (!appServer.Start())
            //{
            //    Console.WriteLine("Failed to start!");
            //    Console.ReadKey();
            //    return;
            //}

            //Console.WriteLine("The server started successfully, press key 'q' to stop it!");
            //appServer.NewSessionConnected += new SessionHandler<AppSession>(appServer_NewSessionConnected);
            //appServer.NewRequestReceived += new RequestHandler<AppSession, StringRequestInfo>(appServer_NewRequestReceived);

            //while (Console.ReadKey().KeyChar != 'q')
            //{
            //    Console.WriteLine();
            //    continue;
            //}

            ////Stop the appServer
            //appServer.Stop();

            //Console.WriteLine("The server was stopped!");
            //Console.ReadKey();
        }

        ////处理连接
        //static void appServer_NewSessionConnected(AppSession session)
        //{
        //    session.Send("Welcome to SuperSocket Telnet Server");
        //}

        ////处理请求
        //static void appServer_NewRequestReceived(AppSession session, StringRequestInfo requestInfo)
        //{
        //    switch (requestInfo.Key.ToUpper())
        //    {
        //        case "1":
        //            {
        //                session.Send("123456789");

        //            }
        //            break;
        //        case "2":
        //            {
        //                session.Send("2222222222");
        //            }
        //            break;
        //        case "3":
        //            {
        //                session.Send("333333333333");
        //            }
        //            break;

        //    }
        //    //switch (requestInfo.Key.ToUpper())
        //    //{
        //    //    case ("ECHO"):
        //    //        session.Send(requestInfo.Body);
        //    //        break;

        //    //    case ("ADD"):
        //    //        session.Send(requestInfo.Parameters.Select(p => Convert.ToInt32(p)).Sum().ToString());
        //    //        break;

        //    //    case ("MULT"):

        //    //        var result = 1;

        //    //        foreach (var factor in requestInfo.Parameters.Select(p => Convert.ToInt32(p)))
        //    //        {
        //    //            result *= factor;
        //    //        }

        //    //        session.Send(result.ToString());
        //    //        break;
        //    //}
        //}

        //public class TelnetSession : AppSession<TelnetSession>
        //{
        //    protected override void OnSessionStarted()
        //    {
        //        this.Send("Welcom to Supersocket Telnet Server");
        //    }

        //    protected override void HandleUnknownRequest(StringRequestInfo requestInfo)
        //    {
        //        this.Send("Unknow request");
        //    }

        //    protected override void HandleException(Exception e)
        //    {
        //        this.Send("Application error: {0}", e.Message);
        //    }

        //    protected override void OnSessionClosed(CloseReason reason)
        //    {
        //        //add you logics which will be executed after the session is closed
        //        base.OnSessionClosed(reason);
        //    }
        //}

        //public class TelnetServer : AppServer<TelnetSession>
        //{
        //    protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
        //    {
        //        return base.Setup(rootConfig, config);
        //    }

        //    protected override void OnStartup()
        //    {
        //        base.OnStartup();
        //    }

        //    protected override void OnStopped()
        //    {
        //        base.OnStopped();
        //    }
        //}

        private static void AgpsUbloxHandle(object source, ElapsedEventArgs e)
        {
            string host = "agps.u-blox.com";
            int port = 46434;

            AgpsClient client = new AgpsClient();
            //string host = "121.199.26.185";
            //int port = 5000;
            Console.WriteLine("AgpsUbloxHandle");
            //string result = SocketSendReceive(host, port);
            client.StartClient(host, port);

            //MTK AGPS data
            MtkAgpsData mtkAgps = new MtkAgpsData();
            mtkAgps.update();
        }
    }
    // 在下面的代码中，当一个新的连接连接上时，服务器端立即向客户端发送欢迎信息。 这段代码还重写了其它AppSession的方法用以实现自己的业务逻辑。
    public class TelnetSession : AppSession<TelnetSession>
    {
        // 重载OnSessionStarted函数，等同于appServer.NewSessionConnected += NewSessionConnected
        protected override void OnSessionStarted()
        {
            // 会话链接成功后的逻辑部分。
            this.Send("Welcome to SuperSocket Telnet Server");
            //this.Close();
        }

        protected override void HandleUnknownRequest(StringRequestInfo requestInfo)
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

    // 你可以根据你的业务需求来给Session类增加新的属性
    public class PlayerSession : AppSession<PlayerSession>
    {
        public int GameHallId { get; internal set; }

        public int RoomId { get; internal set; }
    }

    // 现在 TelnetSession 将可以用在 TelnetServer 的会话中，也有很多方法可以重载
    public class TelnetServer : AppServer<TelnetSession>
    {
        //默认使用的是命令行协议。
        public TelnetServer() : base(new CommandLineReceiveFilterFactory(Encoding.Default, new BasicRequestInfoParser("=", ",")))
        {
            //LOGIN:USER=PWD
        }

        //public TelnetServer() : base(new CountSpliterReceiveFilterFactory((byte)'=', 2))
        //{
        //    //固定数量分隔符协议
        //}

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

    #region
    //命令行协议
    public class LOGIN : CommandBase<TelnetSession, StringRequestInfo>
    {
        public override void ExecuteCommand(TelnetSession session, StringRequestInfo requestInfo)
        {
            session.Send("Login succ {0} {1} {2}!", requestInfo.Key, requestInfo.Body, requestInfo.Parameters);
        }
    }

    public class ECHO : CommandBase<TelnetSession, StringRequestInfo>
    {
        public override void ExecuteCommand(TelnetSession session, StringRequestInfo requestInfo)
        {
            session.Send("ECHO succ!");
        }
    }

    public class APPKEY : CommandBase<TelnetSession, StringRequestInfo>
    {
        public override void ExecuteCommand(TelnetSession session, StringRequestInfo requestInfo)
        {
            //session.Send("APPKEY succ!");
            string value = Program.KeyDic["appkey"];
            Console.WriteLine("appkey value :{0}", value);
            int i = string.Compare(requestInfo.Body, value);
            if (i == 0)
            {
                Console.WriteLine("APPKEY pwd succ!");
                //session.Send(AgpsClient.AgpsData);
                //byte[] rsp = {0x30, 0x31, 0x7F, 0xA0, 0xA1, 0x00, 0x01, 0x02};
                //string utfString = Encoding.GetEncoding("iso-8859-1").GetString(rsp, 0, rsp.Length);
                //string utfString1 = Encoding.GetEncoding("UTF-8").GetString(rsp, 0, rsp.Length); //IBM00858
                //session.Send(utfString);
                //session.Send(utfString1);
                //session.Send(new ArraySegment<byte>(rsp));
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

    public class SECURENET01 : CommandBase<TelnetSession, StringRequestInfo>
    {
        public override void ExecuteCommand(TelnetSession session, StringRequestInfo requestInfo)
        {
            session.Send("SECURENET01 succ!");
        }
    }

    public class SECURENET : CommandBase<TelnetSession, StringRequestInfo>
    {
        public override void ExecuteCommand(TelnetSession session, StringRequestInfo requestInfo)
        {
            session.Send("SECURENET succ!");
        }
    }

    public class ATAGPS : CommandBase<TelnetSession, StringRequestInfo>
    {
        public override void ExecuteCommand(TelnetSession session, StringRequestInfo requestInfo)
        {
            session.Send("ATAGPS succ!");
        }
    }
    #endregion

    #region
    public class EEEE : CommandBase<MyFixedHeaderReceiveProtocolSession, BinaryRequestInfo>
    {
        public override void ExecuteCommand(MyFixedHeaderReceiveProtocolSession session, BinaryRequestInfo requestInfo)
        {
            //session.Send(new ArraySegment<byte>(requestInfo.Body));
            session.Send("EEEE succ!");
        }
    }
    #endregion
}
