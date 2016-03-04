using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    /// <summary>
    /// class for changing event args
    /// </summary>
    public class MD5ChangingEventArgs : EventArgs
    {
        public readonly byte[] NewData;

        public MD5ChangingEventArgs(byte[] data)
        {
            byte[] NewData = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
                NewData[i] = data[i];
        }

        public MD5ChangingEventArgs(string data)
        {
            byte[] NewData = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
                NewData[i] = (byte)data[i];
        }

    }
}
