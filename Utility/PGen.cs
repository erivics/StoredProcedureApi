using System.Security.Cryptography;
using System.Text;

namespace StoredProcedureAp.Utility
{
    public class PGen
    {
        /// <summary>
        /// Generate the encrypted string 
        /// </summary>
        /// <param name="data">Random Number</param>
        /// <returns>Encrypted String</returns>
        private static string MD5Hash(string data)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(Encoding.ASCII.GetBytes(data));

            StringBuilder stringBuilder = new StringBuilder();
            foreach( byte b in hash)
            {
                stringBuilder.AppendFormat("{0,2:X2}", b);
            }
            return stringBuilder.ToString();
        }

        public static string VerificationCodeGenerated(int length, string type = "alphanumeric")
        {
            Random random = new Random();
            if(type == "numeric")
            {
                return random.Next(1000, 9999).ToString();
            }
            string vcode = MD5Hash(random.Next().ToString().Substring(0, length));
            string new_vcode ="";

            for (int i = 0; i < vcode.Length; i++)
            {
                if (random.Next(0,2) == 1)
                    new_vcode += vcode.Substring(i, 1).ToUpper();
                else  
                    new_vcode += vcode.Substring(i, 1);
            }
            return new_vcode;
        }
    }
}
