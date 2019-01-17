using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Servers
{
    class Message
    {
        private byte[] dataBytes=new byte[1024];
        private int startIndex=0;

        public byte[] DataBytes
        {
            get { return dataBytes; }
            set { dataBytes = value; }
        }

        public int StartIndex
        {
            get { return startIndex; }
        }

        public int RemainCount
        {
            get {return dataBytes.Length-startIndex;}
        }

        public void ReadMessage(int count)
        {

        }

        private void HandleRequest()
        {

        }
    }
}
