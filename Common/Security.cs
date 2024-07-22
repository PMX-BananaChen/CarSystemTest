/********************************************************************************
** Author: gaocaihui
** Created On: 05/18/2010 16:30
** Comments: Encrypt&Decrypt component.
*********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace CarSystemTest.Common
{
    public class Security
    {
        public Security()
        {
        }

        /// <summary>
        /// 採用MD5或SHA1加密方式
        /// </summary>
        /// <param name="PasswordString"></param>
        /// <param name="PasswordFormat"></param>
        /// <returns></returns>
        public static string EncryptPassword(string PasswordString, string PasswordFormat)
        {
            string EncryptPassword = "";
            if (PasswordFormat == "SHA1")
            {
                EncryptPassword = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(PasswordString, "SHA1");
            }
            else if (PasswordFormat == "MD5")
            {
                EncryptPassword = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(PasswordString, "MD5");                
            }
            return EncryptPassword;
        }

        #region BASE64 Encoding.by gaocaihui add 2009-2-27
        /// <summary>
        /// Encode64
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        public string Encode64(string Message)
        {
            char[] Base64Code = new char[]
              {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N',
                  'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b',
                  'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p',
                  'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3',
                  '4', '5', '6', '7', '8', '9', '+', '/', '='
              };
            byte empty = (byte)0;
            System.Collections.ArrayList byteMessage = new
              System.Collections.ArrayList(System.Text.Encoding.Default.GetBytes
              (Message));
            System.Text.StringBuilder outmessage;
            int messageLen = byteMessage.Count;
            int page = messageLen / 3;
            int use = 0;
            if ((use = messageLen % 3) > 0)
            {
                for (int i = 0; i < 3 - use; i++)
                    byteMessage.Add(empty);
                page++;
            }
            outmessage = new System.Text.StringBuilder(page * 4);
            for (int i = 0; i < page; i++)
            {
                byte[] instr = new byte[3];
                instr[0] = (byte)byteMessage[i * 3];
                instr[1] = (byte)byteMessage[i * 3 + 1];
                instr[2] = (byte)byteMessage[i * 3 + 2];
                int[] outstr = new int[4];
                outstr[0] = instr[0] >> 2;
                outstr[1] = ((instr[0] & 0x03) << 4) ^ (instr[1] >> 4);
                if (!instr[1].Equals(empty))
                    outstr[2] = ((instr[1] & 0x0f) << 2) ^ (instr[2] >> 6);
                else
                    outstr[2] = 64;
                if (!instr[2].Equals(empty))
                    outstr[3] = (instr[2] & 0x3f);
                else
                    outstr[3] = 64;
                outmessage.Append(Base64Code[outstr[0]]);
                outmessage.Append(Base64Code[outstr[1]]);
                outmessage.Append(Base64Code[outstr[2]]);
                outmessage.Append(Base64Code[outstr[3]]);
            }
            return outmessage.ToString();
        }
        /// <summary>
        /// Decode64
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        public string Decode64(string Message)
        {
            if (Message == null || Message == "")   
            {
                return null;
            }

            string Base64Code = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
            int page = Message.Length / 4;
            System.Collections.ArrayList outMessage = new System.Collections.ArrayList(page * 3);
            char[] message = Message.ToCharArray();
            for (int i = 0; i < page; i++)
            {
                byte[] instr = new byte[4];
                instr[0] = (byte)Base64Code.IndexOf(message[i * 4]);
                instr[1] = (byte)Base64Code.IndexOf(message[i * 4 + 1]);
                instr[2] = (byte)Base64Code.IndexOf(message[i * 4 + 2]);
                instr[3] = (byte)Base64Code.IndexOf(message[i * 4 + 3]);
                byte[] outstr = new byte[3];
                outstr[0] = (byte)((instr[0] << 2) ^ ((instr[1] & 0x30) >> 4));
                if (instr[2] != 64)
                {
                    outstr[1] = (byte)((instr[1] << 4) ^ ((instr[2] & 0x3c) >> 2));
                }
                else
                {
                    outstr[2] = 0;
                }
                if (instr[3] != 64)
                {
                    outstr[2] = (byte)((instr[2] << 6) ^ instr[3]);
                }
                else
                {
                    outstr[2] = 0;
                }
                outMessage.Add(outstr[0]);
                if (outstr[1] != 0)
                    outMessage.Add(outstr[1]);
                if (outstr[2] != 0)
                    outMessage.Add(outstr[2]);
            }
            byte[] outbyte = (byte[])outMessage.ToArray(Type.GetType("System.Byte"));
            return System.Text.Encoding.Default.GetString(outbyte);
        }
        #endregion
    }
}
