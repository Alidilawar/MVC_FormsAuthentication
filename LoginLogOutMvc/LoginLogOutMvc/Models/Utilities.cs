using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace LoginLogOutMvc.Models
{
    public class Utilities
    {
        public static string Encrypt(string key, string plaintxt)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream((Stream)ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter((Stream)cs))
                        {
                            sw.Write(plaintxt);
                        }
                        array = ms.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(array);            
        }

        public static string Decrypt(string key, string ciphertxt)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(ciphertxt);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform Decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream(buffer))
                {
                    using (CryptoStream cs = new CryptoStream((Stream)ms, Decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sR = new StreamReader((Stream)cs))
                        {
                           return sR.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}