﻿/*
 *  Purpose: This Class has the Function to Encrypt the password.
 * 
 *  Author: Rahul Chaurasia
 *  Date: 17-1-2020
 */


using System;
using System.Collections.Generic;
using System.Text;

namespace FundooCommonLayer.Model
{
    public class EncodeDecode
    {

        /// <summary>
        /// It Encrpyt the Password to Base64
        /// </summary>
        /// <param name="password">User Password</param>
        /// <returns>Encrypted Password</returns>
        public static string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }

        //public string DecodeFrom64(string encodedData)
        //{
        //    UTF8Encoding encoder = new UTF8Encoding();
        //    Decoder utf8Decode = encoder.GetDecoder();
        //    byte[] todecode_byte = Convert.FromBase64String(encodedData);
        //    int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
        //    char[] decoded_char = new char[charCount];
        //    utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
        //    string result = new string(decoded_char);
        //    return result;
        //}



    }
}
