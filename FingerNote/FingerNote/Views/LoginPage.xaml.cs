using FingerNote.Security;
using FingerNote.ViewModels;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FingerNote.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private string key = "UserHash";
        LoginViewModel vm
        {
            get { return BindingContext as LoginViewModel; }
        }

        public LoginPage() : base()
        {
            InitializeComponent();
            try
            {
                if (SecureStorage.GetAsync(key).IsCompleted)
                    CreateButton.IsVisible = false;
            }
            catch {}
            BindingContext = new LoginViewModel();
        }

        private async Task SetResultAsync(FingerprintAuthenticationResult result)
        {
            if (result.Authenticated)
            {
                await Navigation.PushAsync(new NotePage()).ConfigureAwait(true);
                //await DisplayAlert("FingerPrint Sample", "Success", "Ok");
            }
            else
            {
                await DisplayAlert("FingerPrint Sample", result.ErrorMessage, "Ok");
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var result = await CrossFingerprint.Current.IsAvailableAsync(true);
            if (result)
            {
                var cancellationToken = new System.Threading.CancellationToken();
                var auth = await CrossFingerprint.Current.AuthenticateAsync("Prove you have fingers!", cancellationToken);
                await SetResultAsync(auth);
            }
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            var hash = Crypto.ComputeSHA512Hash(PasswordEntry.Text);
            try {
                string savedHash = await SecureStorage.GetAsync(key).ConfigureAwait(true);
                if (savedHash == hash)
                {
                    await Navigation.PushAsync(new NotePage()).ConfigureAwait(true);
                }
                else
                {
                    PasswordEntry.Text = "";
                    await DisplayAlert("Error", "Wrong password", "Ok");
                }
            }
            catch (Exception ex) { }
        }

        private void CreateButton_Clicked(object sender, EventArgs e)
        {
            CreateButton.IsVisible = false;
            var hash = Crypto.ComputeSHA512Hash(PasswordEntry.Text);
            Crypto.SetKeyValue(key, hash);
        }
    }
}