using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Common;

namespace GameServer.Servers
{
    public class Message
    {
        private byte[] buffer = new byte[1024];
        private int startIndex = 0;

        public byte[] DataBytes
        {
            get { return buffer; }
            set { buffer = value; }
        }

        public int StartIndex
        {
            get { return startIndex; }
        }

        public int RemainCount
        {
            get { return buffer.Length - startIndex; }
        }

        public void ReadMessage(int count,Action<RequestCode,ActionCode,string> ProcessDataCallBack)
        {
            startIndex += count;
            while (true)
            {
                if (startIndex<4)
                {
                    return;
                }

                int dataLength = BitConverter.ToInt32(buffer,0);
                if (startIndex-4>=dataLength)
                {
                    RequestCode requestCode = (RequestCode) BitConverter.ToInt32(buffer, 4);
                    ActionCode actionCode = (ActionCode) BitConverter.ToInt32(buffer, 8);
                    string data = Encoding.UTF8.GetString(buffer, 12, dataLength - 8);
                    ProcessDataCallBack(requestCode, actionCode, data);
                    Array.Copy(buffer,dataLength+4,buffer,0,startIndex-4-dataLength);
                    startIndex -= (dataLength + 4);
                }
                else
                {
                    return;
                }
            }
        }


        public byte[] PackBytes(ActionCode actionCode, string data)
        {
            byte[] actionCodeBytes = BitConverter.GetBytes((int) actionCode);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            int totalDataLength = actionCodeBytes.Length + dataBytes.Length;
            byte[] totalDataLengthBytes = BitConverter.GetBytes(totalDataLength);
            return totalDataLengthBytes.Concat(actionCodeBytes).ToArray<byte>().Concat(dataBytes).ToArray<byte>();
        }


    }
}
