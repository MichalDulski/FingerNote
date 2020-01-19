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
            if (SecureStorage.GetAsync(key).Result == null)
                PrevPassword.IsVisible = false;
        }

        private async void ChangePassword_Clicked(object sender, EventArgs e)
        {
            var oldHash = Crypto.ComputeSHA512Hash(PrevPassword.Text);
            try
            {
                if(SecureStorage.GetAsync(key).Result == null)
                {
                    var newHash = Crypto.ComputeSHA512Hash(NewPassword.Text);
                    Crypto.SetKeyValue(key, newHash);
                    await Navigation.PopAsync().ConfigureAwait(true);
                }
                else 
                { 
                    string savedHash = await SecureStorage.GetAsync(key).ConfigureAwait(true);
                    if(oldHash == savedHash)
                    {
                        var iv = SecureStorage.GetAsync(App.PasswordIV).Result;
                        var methodPassword = SecureStorage.GetAsync(App.PasswordPass).Result;
                        var decrypted = Crypto.Decrypt(methodPassword, PrevPassword.Text, iv);
                        var newHash = Crypto.ComputeSHA512Hash(NewPassword.Text);
                        var tup = Crypto.Encrypt(decrypted, NewPassword.Text);
                        Crypto.SetKeyValue(key, newHash);
                        SecureStorage.SetAsync(App.PasswordPass, tup.Item1);
                        SecureStorage.SetAsync(App.PasswordIV, tup.Item2);
                        await Navigation.PopAsync().ConfigureAwait(true);
                    }
                    else
                    {
                        await DisplayAlert("Error", "Wrong password", "Ok");
                    }
                }
            }
            catch(Exception ex) { }
        }
    }
}