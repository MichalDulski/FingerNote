using FingerNote.Models;
using FingerNote.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FingerNote.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotePage : ContentPage
    {
        NoteViewModel vm
        {
            get { return BindingContext as NoteViewModel; }
        }

        public NotePage() : base()
        {
            InitializeComponent();
            BindingContext = new NoteViewModel();
            Note note = App.NoteDatabase.GetNoteAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            vm.Content = note.Content;
            vm.Id = note.Id;
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var note = new Note
            {
                Id = vm.Id,
                Content = vm.Content
            };

            await App.NoteDatabase.SaveNoteAsync(note).ConfigureAwait(false);
        }
    }
}