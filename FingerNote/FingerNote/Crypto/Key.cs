using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace FingerNote.Crypto
{
    public static class Key
    {
        public static async void SetNotesKey(string key)
        {
            try { await SecureStorage.SetAsync("NotesKey", key).ConfigureAwait(true); }
            catch (Exception) { }
        }

        public static async Task<string> GetNotesKey()
        {
            try
            {
                var token = await SecureStorage.GetAsync("NotesKey");
                return token;
            }
            catch (Exception)
            {
                return "Error";
            }
        }
    }
}
