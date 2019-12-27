using System;
using System.Collections.Generic;
using System.Text;

namespace FingerNote.ViewModels
{
    class LoginViewModel:ParentViewModel
    {
        public bool CreateVisible { get; set; } = true;
        public string PasswordPlainText { get; set; }
    }
}
