using System;
using System.Collections.Generic;
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
    }
}
