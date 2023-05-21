using System.Security.Cryptography;

namespace BitirmeProjesi.Services
{
    public class StudentPasswordHasher
    {

        public static string Encrypt(string metin)
        {
            MD5CryptoServiceProvider pwd = new MD5CryptoServiceProvider();

            byte[] byteDegeri = System.Text.Encoding.UTF8.GetBytes(metin);
            byte[] sifreliByte = pwd.ComputeHash(byteDegeri);
            return Convert.ToBase64String(sifreliByte);
        }

    }
}
