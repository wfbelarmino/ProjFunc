using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ProjFunc.Helper
{
    public  static class CriptoHelper
    {
        //criado o medodo para fazer o hash
        public static string HashMD5(string val)
        {
            //Obs.: Todos os algoritmos dentro do Asp.Net, trabalha com bytes e não trabalha strings.
            //Por isso a conversão abaixo
            var bytes = Encoding.ASCII.GetBytes(val);
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(bytes);

            var ret = string.Empty;
            for (int i = 0; i < hash.Length; i++)
            {
                ret += hash[i].ToString("x2");
            }

            return ret;
        }
    }
}