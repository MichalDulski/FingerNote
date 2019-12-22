using FingerNote.Data;
using FingerNote.Interfaces;
using FingerNote.Views;
using System;
using System.IO;
using System.Security.Cryptography;
using Xamarin.Forms;

namespace FingerNote
{

    public partial class App : Application
    {
        private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();

        static NoteDatabase noteDatabase;
        public static NoteDatabase NoteDatabase
        {
            get
            {
                if (noteDatabase == null)
                {
                    var key = DependencyService.Get<IGetKeyService>().GetKey();
                    //if(key == null)
                    //{
                    //    //byte[] data = new byte[1024];
                    //    //rngCsp.GetBytes(data);
                    //    //key = BitConverter.ToString(data, 0);
                    //    //Key.SetNotesKey(key);
                    //    key = DependencyService.Get<IGetKeyService>().GetKey();
                    //}
                    noteDatabase = new NoteDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Notes.db3"), key);
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
