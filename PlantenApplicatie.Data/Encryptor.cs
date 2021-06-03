using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PlantenApplicatie
{
    //class made by Davy
    public static class Encryptor
    {
        //generate a hashcode for the password
        public static byte[] GenerateMD5Hash(string text)
        {
            //convert the result to a byte
            byte[] result =  { 0x20 };

            try
            {

                MD5 md5 = new MD5CryptoServiceProvider();

                //compute hash from the bytes of text  
                md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

                //get hash result after compute it  
                result = md5.Hash;
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return result;
        }

        public static string CheckMD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
    }
}
