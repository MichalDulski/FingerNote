using System;
using System.Collections.Generic;
using System.Text;

namespace FingerNote.ViewModels
{
    class NoteViewModel : ParentViewModel
    {
        int id;
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        string content;
        public string Content
        {
            get { return content; }
            set
            {
                content = value;
                OnPropertyChanged();
            }
        }

        public NoteViewModel() : base()
        {
            Id = 0;
            Content = "";
        }
    }
}
