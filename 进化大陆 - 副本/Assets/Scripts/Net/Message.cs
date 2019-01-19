using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using UnityEngine;

public class Message 
{

    private byte[] buffer=new byte[1024];
    private int startIndex;

    public byte[] Buffer
    {
        get { return buffer; }
        set { buffer = value; }
    }

    public int StartIndex
    {
        get { return startIndex; }
        set { startIndex = value; }
    }

    public int RemainCount
    {
        get { return buffer.Length - startIndex; }
    }
    public void ReadMessage(int count,Action<ActionCode,string> ProcessCallBack)
    {
        startIndex += count;
        while (true)
        {
            if (startIndex < 4)
            {
                return;
            }
            int dataLength = BitConverter.ToInt32(buffer, 0);
            if (startIndex - 4 == dataLength)
            {
                ActionCode actionCode = (ActionCode)BitConverter.ToInt32(buffer, 4);
                string s = Encoding.UTF8.GetString(buffer, 8, dataLength - 4);
                ProcessCallBack(actionCode, s);
                Array.Copy(buffer, dataLength+4,buffer,0,startIndex-4-dataLength);
                startIndex -= (4 + dataLength);
            }
            else
            {
                return;
            }
        }
        
    }

    public byte[] PackData(RequestCode requestCode, ActionCode actionCode, string data)
    {
        byte[] requestCodeBytes = BitConverter.GetBytes((int) requestCode);
        byte[] actionCodeBytes = BitConverter.GetBytes((int) actionCode);
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        int bytesLength = requestCodeBytes.Length + actionCodeBytes.Length + dataBytes.Length;
        byte[] bytesLengBytes = BitConverter.GetBytes(bytesLength);
        return bytesLengBytes.Concat(requestCodeBytes).ToArray<byte>().Concat(actionCodeBytes).ToArray<byte>()
            .Concat(dataBytes).ToArray<byte>();
    }
}
