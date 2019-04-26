using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AgpsServer
{
    class MtkAgpsData
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static byte[] Data = new byte[5000];
        private static string mtkAgpsDataFilePath = "../agps/MTK6HS.EPO";

        //public MtkAgpsData()
        //{
        //    try
        //    {
        //        FileStream aFile = new FileStream(mtkAgpsDataFilePath, FileMode.Open);
        //        int fileLen = (int)aFile.Length;
        //        byte[] tmpData = new byte[fileLen];
        //        aFile.Read(tmpData, 0, fileLen);
        //        Data = tmpData;
        //        aFile.Close();

        //        string str = "MTK agps init " + Data.Length;
        //        Program.log.Info(str);
        //    }
        //    catch (IOException e)
        //    {
        //        Console.WriteLine(e.ToString());
        //        return;
        //    }
        //    return;
        //}

        //更新数据
        public void update()
        {
            try
            {
                FileStream aFile = new FileStream(mtkAgpsDataFilePath, FileMode.Open);
                int fileLen = (int)aFile.Length;
                byte[] tmpData = new byte[fileLen];
                aFile.Read(tmpData, 0, fileLen);
                Data = tmpData;
                aFile.Close();

                string str = "MTK agps data " + Data.Length;
                if (log.IsInfoEnabled)
                {
                    log.Info(str);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.ToString());
                return;
            }
            return;
        }

    }
}
