using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    /// <summary>
    /// class for cahnged event args
    /// </summary>
    public class MD5ChangedEventArgs : EventArgs
    {
        public readonly byte[] NewData;
        public readonly string FingerPrint;

        public MD5ChangedEventArgs(byte[] data, string HashedValue)
        {
            byte[] NewData = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
                NewData[i] = data[i];
            FingerPrint = HashedValue;
        }

        public MD5ChangedEventArgs(string data, string HashedValue)
        {
            byte[] NewData = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
                NewData[i] = (byte)data[i];

            FingerPrint = HashedValue;
        }

    }

}
