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
                if (SecureStorage.GetAsync(key).Result != null || SecureStorage.GetAsync(App.FingerPass).Result != null)
                    CreateButton.IsVisible = false;
                else
                    CreateButton.IsVisible = true;
            }
            catch {}
            BindingContext = new LoginViewModel();
        }

        private async Task SetResultAsync(FingerprintAuthenticationResult result)
        {
            if (result.Authenticated)
            {
                if(SecureStorage.GetAsync(App.FingerPass).Result == null)
                {
                    if(SecureStorage.GetAsync(App.PasswordPass).Result == null)
                    {
                        App.LoginMethod = Enums.LoginMethodEnum.Fingerprint;
                        await Navigation.PushAsync(new NotePage()).ConfigureAwait(true);
                    }
                    else
                    {
                        var hash = Crypto.ComputeSHA512Hash(PasswordEntry.Text);
                        try
                        {
                            string savedHash = await SecureStorage.GetAsync(key).ConfigureAwait(true);
                            if (savedHash == hash)
                            {
                                App.PassText = PasswordEntry.Text;
                                PasswordEntry.Text = "";
                                App.LoginMethod = Enums.LoginMethodEnum.Fingerprint;
                                await Navigation.PushAsync(new NotePage()).ConfigureAwait(true);
                            }
                            else
                            {
                                PasswordEntry.Text = "";
                                await DisplayAlert("Error", "Please provide correct password", "Ok");
                            }
                        }
                        catch (Exception ex) { }
                    }
                }
                else
                {
                    App.LoginMethod = Enums.LoginMethodEnum.Fingerprint;
                    await Navigation.PushAsync(new NotePage()).ConfigureAwait(true);
                }
            }
            else
            {
                await DisplayAlert("Finger not found :(", result.ErrorMessage, "Ok");
            }
        }

        private async void FingerprintButton_Clicked(object sender, EventArgs e)
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
                    App.PassText = PasswordEntry.Text;
                    App.LoginMethod = Enums.LoginMethodEnum.Password;
                    PasswordEntry.Text = "";
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