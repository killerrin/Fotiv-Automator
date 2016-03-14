using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Fotiv_Automator.Infrastructure
{
    public class Gravitar
    {
        public enum GravitarRating
        {
            G,
            PG,
            R,
            X
        }

        /// Hashes an email with MD5.  Suitable for use with Gravatar profile image urls
        public static string HashEmailForGravatar(string email)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.  
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.  
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(email));

            // Create a new Stringbuilder to collect the bytes  
            // and create a string.  
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string.  
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();  // Return the hexadecimal string. 
        }

        public static string GetAvatar(string email, int pixelSize=80, GravitarRating rating = GravitarRating.G, bool ssl = false)
        {
            //  Compute the hash
            string hash = HashEmailForGravatar(email);

            //  Assemble the url and return
            string domain = ssl ? "https://secure.gravatar.com/" : "http://www.gravatar.com/";
            string url = string.Format("avatar/{0}?size={1}&rating={2}", hash, pixelSize, rating.ToString());
            return domain + url;
        }

        public static string GetProfile(string email, bool ssl = false)
        {
            //  Compute the hash
            string hash = HashEmailForGravatar(email);

            //  Assemble the url and return
            string domain = ssl ? "https://secure.gravatar.com/" : "http://www.gravatar.com/";
            string url = string.Format(domain + "{0}", hash);

            return url;
        }

        public static string GetProfileJson(string email, bool ssl = false)
        {
            return GetProfile(email, ssl) + ".json";
        }
    }
}