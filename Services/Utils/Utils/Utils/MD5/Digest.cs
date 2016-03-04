using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    /// <summary>
    /// constants for md5
    /// </summary>
    public enum MD5InitializerConstant : uint
    {
        A = 0x67452301,
        B = 0xEFCDAB89,
        C = 0x98BADCFE,
        D = 0X10325476
    }
    /// <summary>
    /// Represent digest with ABCD
    /// </summary>
    sealed public class Digest
    {
        public uint A;
        public uint B;
        public uint C;
        public uint D;

        public Digest()
        {
            A = (uint)MD5InitializerConstant.A;
            B = (uint)MD5InitializerConstant.B;
            C = (uint)MD5InitializerConstant.C;
            D = (uint)MD5InitializerConstant.D;
        }
        public override string ToString()
        {
            string st;
            st = MD5Helper.ReverseByte(A).ToString("X8") +
                MD5Helper.ReverseByte(B).ToString("X8") +
                MD5Helper.ReverseByte(C).ToString("X8") +
                MD5Helper.ReverseByte(D).ToString("X8");
            return st;

        }

    }
}
