using FingerNote.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FingerNote.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private string key = "UserHash";

        public SettingsPage()
        {
            InitializeComponent();
        }

        private async void ChangePassword_Clicked(object sender, EventArgs e)
        {
            var oldHash = Crypto.ComputeSHA512Hash(PrevPassword.Text);
            try
            {
                string savedHash = await SecureStorage.GetAsync(key).ConfigureAwait(true);
                if(oldHash == savedHash)
                {
                    var newHash = Crypto.ComputeSHA512Hash(NewPassword.Text);
                    Crypto.SetKeyValue(key, newHash);
                    await Navigation.PopAsync().ConfigureAwait(true);
                }
                else
                {
                    await DisplayAlert("Error", "Wrong password", "Ok");
                }
            }
            catch(Exception ex) { }
        }
    }
}