using FingerNote.ViewModels;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FingerNote.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage() : base()
        {
            InitializeComponent();
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
    }
}