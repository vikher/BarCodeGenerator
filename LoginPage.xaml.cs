using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterwayBarCodeGenerator.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.Identity.Client;
using WaterwayBarCodeGenerator.Model;
using System.IdentityModel.Tokens.Jwt;


namespace WaterwayBarCodeGenerator
{
	//[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        public LoginPage ()
		{
            InitializeComponent();
            wwlogo.Source = ImageSource.FromFile("wwlogo.png");
        }
        protected override async void OnAppearing()
        {
            UpdateSignInState(true);
            try
            {
                // Look for existing user
                var result = await App.AuthenticationClient.AcquireTokenSilentAsync(Constants.Scopes);
                UpdateSignInState(false);
                await Navigation.PushAsync(new MembershipsPage(result));
            }
            catch
            {
                // Do nothing - the user isn't logged in
                UpdateSignInState(true);
            }
            base.OnAppearing();
        }

        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            UpdateActivityIndicator(true);
            UpdateSignInState(false);
            try
            {

                var result = await App.AuthenticationClient.AcquireTokenAsync(
                    Constants.Scopes,
                    string.Empty,
                    UiOptions.SelectAccount,
                    string.Empty,
                    null,
                    Constants.Authority,
                    Constants.SignUpSignInPolicy);

                await Navigation.PushAsync(new MembershipsPage(result));
                UpdateActivityIndicator(false);
                UpdateSignInState(true);
            }
            catch (MsalException ex)
                {
                    if (ex.Message != null && ex.Message.Contains("AADB2C90118"))
                    {
                        await OnForgotPassword();
                    }
                    if (ex.ErrorCode != "authentication_canceled")
                    {
                        await DisplayAlert("An error has occurred", "Exception message: " + ex.Message, "Dismiss");
                    }
                }    
        }

        void UpdateActivityIndicator(bool isActive)
        {
            ActivityIndicator.IsEnabled = isActive;
            ActivityIndicator.IsRunning = isActive;
            ActivityIndicator.IsVisible = isActive;

        }

        void UpdateSignInState(bool isSignedIn)
        {
            loginButton.IsVisible = isSignedIn;
        }

        async Task OnForgotPassword()
        {
            try
            {
                var result = await App.AuthenticationClient.AcquireTokenAsync(
                    Constants.Scopes,
                    string.Empty,
                    UiOptions.SelectAccount,
                    string.Empty,
                    null,
                    Constants.Authority,
                    Constants.ResetPasswordPolicy);

                await Navigation.PushAsync(new MembershipsPage(result));
                UpdateActivityIndicator(false);
                UpdateSignInState(true);
            }
            catch (Exception ex)
            {
                // Do nothing - ErrorCode will be displayed in OnLoginButtonClicked
                // Alert if any exception excludig user cancelling sign-in dialog
                if (((ex as MsalException)?.ErrorCode != "authentication_canceled"))
                    await DisplayAlert($"Exception:", ex.ToString(), "Dismiss");
            }
        }

    }
}