using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterwayBarCodeGenerator.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.Identity.Client;
using System.IdentityModel.Tokens.Jwt;
using WaterwayBarCodeGenerator.Helpers;

namespace WaterwayBarCodeGenerator
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MembershipsPage : ContentPage
	{
        AuthenticationResult authenticationResult;
        List<CccMemberProfile> members;
        DateTime dt = DateTime.Now;

        public MembershipsPage (AuthenticationResult result)
		{
			InitializeComponent ();
            members = new List<CccMemberProfile>();

            var token = new JwtSecurityToken(result.Token);
            string email = token.Claims.FirstOrDefault(c => c.Type == "emails").Value;
            string url = GenerateUrl(email);
            members = ApiWebRequestHelper.GetXmlRequest<List<CccMemberProfile>>(url);

            //members = members.Where(i => i.ExpirationDate > dt).Cast<CccMemberProfile>().ToList();
            membershipsListView.ItemsSource = members;
            membershipsListView.ItemTapped += MembershipsListView_ItemTapped;
        }

        public static string GenerateUrl(string email)
        {
            return string.Format(Constants.MEMBER_SEARCH, email, Constants.API_KEY);
        }

        private void MembershipsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            CccMemberProfile member = (CccMemberProfile)e.Item;
            if (member.ExpirationDate < dt)
            {
                DisplayAlert("Notification", "Your membership expired please contact Waterway", "OK");
            }
            else
            {
                Navigation.PushAsync(new memberDetailsPage(member));
            }
        }

        protected override void OnAppearing()
        {

            base.OnAppearing();
        }

        async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            App.AuthenticationClient.UserTokenCache.Clear(Constants.ApplicationID);
            await Navigation.PushAsync(new LoginPage());
        }
    }
}