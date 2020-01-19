using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Xamarin.Essentials;

namespace FingerNote.Security
{
    public static class Crypto
    {
        public static async void SetKeyValue(string key, string value)
        {
            try
            {
                await SecureStorage.SetAsync(key, value);
            }
            catch (Exception ex) { }
        }

        public static string ComputeSHA512Hash(string data)
        {
            using (SHA512 sha512Hash = SHA512.Create())
            {
                byte[] bytes = sha512Hash.ComputeHash(Encoding.UTF8.GetBytes(data));
                StringBuilder builder = new StringBuilder();
                foreach (var item in bytes)
                {
                    builder.Append(item.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static string RandomString()
        {
            const string valid = "!@#$%^&*()-_=+[]{};:,./<>?|abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char cha;
            for (int i = 0; i < random.Next(64, 128); i++)
            {
                builder.Append(valid[random.Next(valid.Length)]);
            }
            return builder.ToString();
        }

        public static Tuple<string, string> Encrypt(string original, string key)
        {
            using (Aes aes = Aes.Create())
            {
                SHA256 hash = SHA256.Create();               
                aes.Key = hash.ComputeHash(Encoding.UTF8.GetBytes(key));
                aes.GenerateIV();
                hash.Dispose();
                string encrypted;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(original);
                        }
                        encrypted = Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
                return new Tuple<string, string>(encrypted, Convert.ToBase64String(aes.IV));
            }
        }
        public static string Decrypt(string encrypted, string key, string iv)
        {

            using (Aes aesAlg = Aes.Create())
            {
                SHA256 hash = SHA256.Create();
                aesAlg.Key = hash.ComputeHash(Encoding.UTF8.GetBytes(key));
                aesAlg.IV = Convert.FromBase64String(iv);
                string decrypted = null;
                hash.Dispose();
                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(encrypted)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            decrypted = srDecrypt.ReadToEnd();
                        }
                    }
                }
                return decrypted;
            }
        }
    }
}