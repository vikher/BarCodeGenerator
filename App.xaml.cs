using Microsoft.Identity.Client;
using System;
using WaterwayBarCodeGenerator.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace WaterwayBarCodeGenerator
{
    public partial class App : Application
    {
        public static PublicClientApplication AuthenticationClient { get; private set; }

        public App()
        {
            InitializeComponent();

            AuthenticationClient = new PublicClientApplication(Constants.ApplicationID);
            MainPage = new NavigationPage(new LoginPage());
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
