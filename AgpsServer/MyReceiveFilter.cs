using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase.Protocol;

namespace AgpsServer
{
    class MyReceiveFilter : IReceiveFilter<MyRequestInfo>
    {
        private int m_ParsedLength;

        private int m_OffsetDelta;

        /// <summary>
        /// Filters received data of the specific session into request info.
        /// </summary>
        /// <param name="readBuffer">The read buffer.</param>
        /// <param name="offset">The offset of the current received data in this read buffer.</param>
        /// <param name="length">The length of the current received data.</param>
        /// <param name="toBeCopied">if set to <c>true</c> [to be copied].</param>
        /// <param name="rest">The rest, the length of the data which hasn't been parsed.</param>
        /// <returns></returns>
        public MyRequestInfo Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest)
        {
            rest = 0;
            var requestInfo = new MyRequestInfo();
            string strReq = System.Text.Encoding.Default.GetString(readBuffer, offset, length);
            Console.WriteLine("MyRequestInfo {0},{1},{2}", offset, length, strReq);
            
            if (length <= 0)
            {
                return requestInfo;
            }

            string[] strParts = strReq.Split('=');
            if (strParts.Count() < 2)
            {
                return requestInfo;
            }
            requestInfo.Key = strParts[0];
            requestInfo.value = strParts[1];
            if ((0 == requestInfo.Key.Length) || (0 == requestInfo.value.Length))
            {
                return requestInfo;
            }
            InternalReset();
            Console.WriteLine("MyRequestInfo {0},{1}", requestInfo.Key, requestInfo.value);
            return requestInfo;
        }

        /// <summary>
        /// Gets the size of the left buffer.
        /// </summary>
        /// <value>
        /// The size of the left buffer.
        /// </value>
        public int LeftBufferSize
        {
            get { return m_ParsedLength; }
        }

        /// <summary>
        /// Gets the next receive filter.
        /// </summary>
        public IReceiveFilter<MyRequestInfo> NextReceiveFilter
        {
            get { return null; }
        }

        private void InternalReset()
        {
            m_ParsedLength = 0;
            m_OffsetDelta = 0;
        }
        /// <summary>
        /// Resets this instance to initial state.
        /// </summary>
        public void Reset()
        {
            InternalReset();
        }

        public FilterState State { get; }
    }
}
