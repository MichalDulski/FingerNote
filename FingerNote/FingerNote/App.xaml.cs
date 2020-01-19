using FingerNote.Data;
using FingerNote.Enums;
using FingerNote.Interfaces;
using FingerNote.Security;
using FingerNote.Views;
using System;
using System.IO;
using System.Security.Cryptography;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FingerNote
{

    public partial class App : Application
    {
        public static LoginMethodEnum LoginMethod;
        public static string FingerPass = "FINGER_PASS";
        private static string FingerIV = "FINGER_IV";
        public static string PasswordPass = "PASSWORD_PASS";
        public static string PasswordIV = "PASSWORD_IV";
        public static string PassText = null;
        static NoteDatabase noteDatabase;
        public static NoteDatabase NoteDatabase
        {
            get
            {
                if (noteDatabase == null)
                {
                    string methodPassword;
                    string key;
                    switch (LoginMethod)
                    {
                        case LoginMethodEnum.Password:
                            methodPassword = SecureStorage.GetAsync(PasswordPass).Result;
                            if (methodPassword == null)
                            {
                                string passwd;
                                if(SecureStorage.GetAsync(FingerPass).Result == null)
                                {
                                    passwd = Crypto.RandomString();
                                    var tup = Crypto.Encrypt(passwd, PassText);
                                    PassText = "";
                                    SecureStorage.SetAsync(PasswordPass, tup.Item1);
                                    SecureStorage.SetAsync(PasswordIV, tup.Item2);
                                    noteDatabase = new NoteDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Notes.db3"), passwd);
                                }
                                else
                                {
                                    key = DependencyService.Get<IGetKeyService>().GetKey();
                                    methodPassword = SecureStorage.GetAsync(FingerPass).Result;
                                    var iv = SecureStorage.GetAsync(FingerIV).Result;
                                    var decrypted = Crypto.Decrypt(methodPassword, key, iv);
                                    noteDatabase = new NoteDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Notes.db3"), decrypted);
                                    var tup = Crypto.Encrypt(decrypted, PassText);
                                    SecureStorage.SetAsync(PasswordPass, tup.Item1);
                                    SecureStorage.SetAsync(PasswordIV, tup.Item2);
                                }
                            }
                            else
                             {
                                var iv = SecureStorage.GetAsync(PasswordIV).Result;
                                var decrypted = Crypto.Decrypt(methodPassword, PassText, iv);
                                PassText = "";
                                noteDatabase = new NoteDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Notes.db3"), decrypted);
                            }
                            break;
                        case LoginMethodEnum.Fingerprint:
                            key = DependencyService.Get<IGetKeyService>().GetKey();
                            methodPassword = SecureStorage.GetAsync(FingerPass).Result;
                            if(methodPassword == null)
                            {
                                if (SecureStorage.GetAsync(PasswordPass).Result == null)
                                {
                                    var passwd = Crypto.RandomString();
                                    var tup = Crypto.Encrypt(passwd, key);
                                    SecureStorage.SetAsync(FingerPass, tup.Item1);
                                    SecureStorage.SetAsync(FingerIV, tup.Item2);
                                    noteDatabase = new NoteDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Notes.db3"), passwd);
                                }
                                else
                                {
                                    var iv = SecureStorage.GetAsync(PasswordIV).Result;
                                    methodPassword = SecureStorage.GetAsync(PasswordPass).Result;
                                    var decrypted = Crypto.Decrypt(methodPassword, PassText, iv);
                                    PassText = "";
                                    noteDatabase = new NoteDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Notes.db3"), decrypted);
                                    var tup = Crypto.Encrypt(decrypted, key);
                                    SecureStorage.SetAsync(FingerPass, tup.Item1);
                                    SecureStorage.SetAsync(FingerIV, tup.Item2);
                                }
                            }
                            else
                            {
                                var iv = SecureStorage.GetAsync(FingerIV).Result;
                                var decrypted = Crypto.Decrypt(methodPassword, key, iv);
                                noteDatabase = new NoteDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Notes.db3"), decrypted);
                            }
                            break;
                        default:
                            break;
                    }                 
                }
                return noteDatabase;
            }
        }

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
