using FingerNote.Data;
using FingerNote.Views;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FingerNote
{
    public partial class App : Application
    {
        static NoteDatabase noteDatabase;
        public static NoteDatabase NoteDatabase
        {
            get
            {
                if (noteDatabase == null)
                {
                    noteDatabase = new NoteDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Notes.db3"));
                }
                return noteDatabase;
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new NotePage();
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
