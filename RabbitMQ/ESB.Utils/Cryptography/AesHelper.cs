using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ESB.Utils.Cryptography
{
    public static class AESEncryption
    {
        public static String AES_decrypt(string encrypted, byte[] key, byte[] IV)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encrypted);
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = key;
            aes.IV = IV;
            ICryptoTransform crypto = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] secret = crypto.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
            crypto.Dispose();
            return System.Text.ASCIIEncoding.ASCII.GetString(secret);
        }
        public static string Decrypt1<T>(string text, string password, string salt, byte[] key, byte[] IV)

        where T : SymmetricAlgorithm, new()
        {

            DeriveBytes rgb = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(salt));



            SymmetricAlgorithm algorithm = new T();



            algorithm.Key = key;

            algorithm.IV = IV;



            byte[] rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);

            byte[] rgbIV = rgb.GetBytes(algorithm.BlockSize >> 3);



            ICryptoTransform transform = algorithm.CreateDecryptor(rgbKey, rgbIV);



            using (MemoryStream buffer = new MemoryStream(Convert.FromBase64String(text)))
            {

                using (CryptoStream stream = new CryptoStream(buffer, transform, CryptoStreamMode.Read))
                {

                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {

                        return reader.ReadToEnd();

                    }

                }

            }

        }

        public static String EncryptIt(String s, byte[] key, byte[] IV)
        {
            String result;
            RijndaelManaged rijn = new RijndaelManaged();
            rijn.Mode = CipherMode.CBC;
            rijn.Padding = PaddingMode.PKCS7;
            //rijn.KeySize = 16;
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (ICryptoTransform encryptor = rijn.CreateEncryptor(key, IV))
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(s);
                        }
                    }
                }
                result = Convert.ToBase64String(msEncrypt.ToArray());
            }
            rijn.Clear();
            return result;
        }
        public static String DecryptIt(String s, byte[] key, byte[] IV)
        {
            String result;
            RijndaelManaged rijn = new RijndaelManaged();
            rijn.Mode = CipherMode.CBC;
            rijn.Padding = PaddingMode.PKCS7;
            rijn.KeySize = 128;
            using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(s)))
            {
                using (ICryptoTransform decryptor = rijn.CreateDecryptor(key, IV))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader swDecrypt = new StreamReader(csDecrypt))
                        {
                            result = swDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            rijn.Clear();
            return result;
        }
        #region Static Functions

        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="PlainText">Text to be encrypted</param>
        /// <param name="Password">Password to encrypt with</param>
        /// <param name="Salt">Salt to encrypt with</param>
        /// <param name="HashAlgorithm">Can be either SHA1 or MD5</param>
        /// <param name="PasswordIterations">Number of iterations to do</param>
        /// <param name="InitialVector">Needs to be 16 ASCII characters long</param>
        /// <param name="KeySize">Can be 128, 192, or 256</param>
        /// <returns>An encrypted string</returns>
        public static string Encrypt(string PlainText, string Password,
            string Salt = "Kosher", string HashAlgorithm = "SHA1",
            int PasswordIterations = 2, string InitialVector = "OFRna73m*aze01xY",
            int KeySize = 256)
        {
            if (string.IsNullOrEmpty(PlainText))
                return "";
            byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(InitialVector);
            byte[] SaltValueBytes = Encoding.ASCII.GetBytes(Salt);
            byte[] PlainTextBytes = Encoding.UTF8.GetBytes(PlainText);
            PasswordDeriveBytes DerivedPassword = new PasswordDeriveBytes(Password, SaltValueBytes, HashAlgorithm, PasswordIterations);
            byte[] KeyBytes = DerivedPassword.GetBytes(KeySize / 8);
            RijndaelManaged SymmetricKey = new RijndaelManaged();
            SymmetricKey.Mode = CipherMode.CBC;
            byte[] CipherTextBytes = null;
            using (ICryptoTransform Encryptor = SymmetricKey.CreateEncryptor(KeyBytes, InitialVectorBytes))
            {
                using (MemoryStream MemStream = new MemoryStream())
                {
                    using (CryptoStream CryptoStream = new CryptoStream(MemStream, Encryptor, CryptoStreamMode.Write))
                    {
                        CryptoStream.Write(PlainTextBytes, 0, PlainTextBytes.Length);
                        CryptoStream.FlushFinalBlock();
                        CipherTextBytes = MemStream.ToArray();
                        MemStream.Close();
                        CryptoStream.Close();
                    }
                }
            }
            SymmetricKey.Clear();
            return Convert.ToBase64String(CipherTextBytes);
        }

        /// <summary>
        /// Decrypts a string
        /// </summary>
        /// <param name="CipherText">Text to be decrypted</param>
        /// <param name="Password">Password to decrypt with</param>
        /// <param name="Salt">Salt to decrypt with</param>
        /// <param name="HashAlgorithm">Can be either SHA1 or MD5</param>
        /// <param name="PasswordIterations">Number of iterations to do</param>
        /// <param name="InitialVector">Needs to be 16 ASCII characters long</param>
        /// <param name="KeySize">Can be 128, 192, or 256</param>
        /// <returns>A decrypted string</returns>
        public static string Decrypt(string CipherText, string Password,
            string Salt = "Kosher", string HashAlgorithm = "SHA1",
            int PasswordIterations = 2, string InitialVector = "OFRna73m*aze01xY",
            int KeySize = 256)
        {
            if (string.IsNullOrEmpty(CipherText))
                return "";
            byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(InitialVector);
            byte[] SaltValueBytes = Encoding.ASCII.GetBytes(Salt);
            byte[] CipherTextBytes = Convert.FromBase64String(CipherText);
            PasswordDeriveBytes DerivedPassword = new PasswordDeriveBytes(Password, SaltValueBytes, HashAlgorithm, PasswordIterations);
            byte[] KeyBytes = DerivedPassword.GetBytes(KeySize / 8);
            RijndaelManaged SymmetricKey = new RijndaelManaged();
            SymmetricKey.Mode = CipherMode.CBC;
            byte[] PlainTextBytes = new byte[CipherTextBytes.Length];
            int ByteCount = 0;
            using (ICryptoTransform Decryptor = SymmetricKey.CreateDecryptor(KeyBytes, InitialVectorBytes))
            {
                using (MemoryStream MemStream = new MemoryStream(CipherTextBytes))
                {
                    using (CryptoStream CryptoStream = new CryptoStream(MemStream, Decryptor, CryptoStreamMode.Read))
                    {

                        ByteCount = CryptoStream.Read(PlainTextBytes, 0, PlainTextBytes.Length);
                        MemStream.Close();
                        CryptoStream.Close();
                    }
                }
            }
            SymmetricKey.Clear();
            return Encoding.UTF8.GetString(PlainTextBytes, 0, ByteCount);
        }

        #endregion

       
    }
}
