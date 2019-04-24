using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Timers;

namespace AgpsServer
{
    class AgpsClient
    {
        // Create a logger for use in this class
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static byte[] AgpsData = new byte[5000];

        private static Socket ConnectSocket(string server, int port)
        {
            Socket s = null;
            IPHostEntry hostEntry = null;

            //Get host related information
            hostEntry = Dns.GetHostEntry(server);

            foreach (IPAddress address in hostEntry.AddressList)
            {
                IPEndPoint ipe = new IPEndPoint(address, port);
                Socket tmpSocket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                tmpSocket.Connect(ipe);
                if (tmpSocket.Connected)
                {
                    s = tmpSocket;
                    break;
                }
            }
            return s;
        }

        private static string SocketSendReceive(string server, int port)
        {
            string request = "cmd=aid;user=sky.gc.m2m@gmail.com;pwd=Kumzvh;lat=34262829;lon=108942656;pacc=2000000\n";
            Byte[] bytesSent = Encoding.ASCII.GetBytes(request);
            Byte[] bytesReceived = new Byte[5000];
            string response = "";

            //Create a socket connection with the specified server and port.
            using (Socket s = ConnectSocket(server, port))
            {
                if (s == null)
                {
                    if (log.IsDebugEnabled) log.Debug("Connection failed");
                    return ("Connection failed");
                }
                //Send request to the server.
                s.Send(bytesSent, bytesSent.Length, 0);

                //Receive the server response content.
                int bytes = 0;
                do
                {
                    bytes = s.Receive(bytesReceived, bytesReceived.Length, 0);
                    response = response + Encoding.ASCII.GetString(bytesReceived, 0, bytes);
                } while (bytes > 0);
            }

            string debugText = "Receive data length: " + response.Length;
            if (log.IsDebugEnabled) log.Debug(debugText);

            return response;
        }

        //private static void AgpsUbloxHandle(object source, ElapsedEventArgs e)
        //{
        //    string host = "agps.u-blox.com";
        //    int port = 46434;
        //    //string host = "121.199.26.185";
        //    //int port = 5000;
        //    Console.WriteLine("AgpsUbloxHandle");
        //    //string result = SocketSendReceive(host, port);
        //    StartClient(host, port);
        //}

        // State object for receiving data from remote device.
        public class StateObject
        {
            //Client socket.
            public Socket workSocket = null;
            //Size of recevie buffer.
            public const int BufferSize = 5000;
            //Receive buffer.
            public byte[] buffer = new byte[BufferSize];
            //Receive data string.
            //public StringBuilder sb = new StringBuilder();
            public List<byte> li = new List<byte>();
        }

        //ManualResetEvent instances signal completion.
        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);
        //The response from the remote server.
        private static string response = string.Empty;

        public void StartClient(string server, int port)
        {
            //Connect to a remote device.
            try
            {
                Socket s = null;
                IPHostEntry hostEntry = null;
                //string request = "cmd=aid;user=sky.gc.m2m@gmail.com;pwd=Kumzvh;lat=34262829;lon=108942656;pacc=2000000\n";
                string request = "cmd=aid;user=sky.gc.m2m@gmail.com;pwd=Kumzvh;lat=536011919;lon=1078013906;pacc=1829037593\n";

                //Get host related information
                hostEntry = Dns.GetHostEntry(server);
                IPAddress address = hostEntry.AddressList[0];
                //IPAddress address = IPAddress.Parse(server);
                IPEndPoint ipe = new IPEndPoint(address, port);
                //Create a TCP/IP socket.
                Socket client = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                //Connect to the remote endpoint.
                connectDone.Reset();
                client.BeginConnect(ipe, new AsyncCallback(ConnectCallback), client);
                connectDone.WaitOne();
                string connSucc = "Socket connect success!!!";
                Console.WriteLine(connSucc);
                if (log.IsDebugEnabled) log.Debug(connSucc);

                //Send request to remote server.
                sendDone.Reset();
                Send(client, request);
                sendDone.WaitOne();
                string sendSucc = "Socket send success!!!";
                Console.WriteLine(sendSucc);
                if (log.IsDebugEnabled) log.Debug(sendSucc);

                //Receive the responses from the remote server.
                receiveDone.Reset();
                Receive(client);
                receiveDone.WaitOne();
                //Write the response to the console.
                Console.WriteLine("Response received : {0}", response);

                //Release the socket.
                client.Shutdown(SocketShutdown.Both);
                client.Close();
                string closeSucc = "Socket close!!!";
                Console.WriteLine(closeSucc);
                if (log.IsDebugEnabled) log.Debug(closeSucc);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                //Retrieve the socket from the state object
                Socket client = (Socket)ar.AsyncState;
                //Complete the connection.
                client.EndConnect(ar);
                string tmp = "Socket connected to " + client.RemoteEndPoint.ToString();
                Console.WriteLine(tmp);
                if (log.IsDebugEnabled) log.Debug(tmp);
                //Signal that the connection has been made.
                connectDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void Send(Socket client, string data)
        {
            try
            {
                //Convert the string data to byte data using ASCII encoding.
                byte[] sendDataBytes = Encoding.ASCII.GetBytes(data);
                //Begin sending the data to the remote server.
                client.BeginSend(sendDataBytes, 0, sendDataBytes.Length, 0, new AsyncCallback(SendCallback), client);
                sendDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                //Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;
                //Complete sending the data to the remote server.
                int bytesSent = client.EndSend(ar);
                string tmp = "Send " + bytesSent + " bytes to server.";
                Console.WriteLine(tmp);
                if (log.IsDebugEnabled) log.Debug(tmp);
                sendDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void Receive(Socket client)
        {
            try
            {
                //Create the state object.
                StateObject state = new StateObject();
                state.workSocket = client;
                //Begin receiving the data from the remote server.
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                //Retrieve the state object and the client socket from the asynchronous state object.
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;
                //Read data from the remote server.
                int bytesRead = client.EndReceive(ar);
                string tmp = "receive data length " + bytesRead;
                Console.WriteLine(tmp);
                if (log.IsDebugEnabled) log.Debug(tmp);
                if (bytesRead > 0)
                {
                    //There might be more data, so store the data received so far.
                    //state.sb.Append(Encoding.GetEncoding("utf-8").GetString(state.buffer, 0, bytesRead));
                    byte[] tmpBuff = new byte[bytesRead];
                    Buffer.BlockCopy(state.buffer, 0, tmpBuff, 0, bytesRead);

                    state.li.AddRange(tmpBuff);
                    //Get the rest of the data.
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    //All the data has arrived, put it in response.
                    //if (state.sb.Length > 1)
                    //{
                    //    response = state.sb.ToString();
                    //    //ConvertAgpsData(response);
                    //}
                    if (state.li.ToArray().Length > 1)
                    {
                        AgpsData = state.li.ToArray();
                        Console.WriteLine(AgpsData.Length);
                    }
                    //Signal that all bytes have been received.
                    receiveDone.Set();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void ConvertAgpsData(string srcData)
        {
            string tmp = "";
            int i = 0;
            string CRLF2 = "\r\n\r\n";
            //解析原始的AGPS数据
            i = srcData.IndexOf(CRLF2);
            tmp = srcData.Substring(i + 4);

            //转换成服务器的AGPS数据
            string head = "skynetm2m.com-apgs-server\r\n";
            string head1 = "Content-Type: application/ubx\r\n";
            string head2 = "Content-Length: " + tmp.Length + CRLF2;
            //AgpsData = head + head1 + head2 + tmp;
        }
    }
}
