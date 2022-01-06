using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using WaterwayBarCodeGenerator.Helpers;
using WaterwayBarCodeGenerator.Model;
using Xamarin.Forms;

namespace WaterwayBarCodeGenerator
{
    public partial class MainPage : ContentPage
    {
        string memberNumber;
        CccMemberProfile user;
        List<CccMemberProfile> members;
        List<CccMemberProfile> results;


        public MainPage()
        {
            InitializeComponent();
            user = new CccMemberProfile();
            members = new List<CccMemberProfile>();
        }

        private void LogIn_Clicked(object sender, EventArgs e)
        {
            bool isEmailEmpty = string.IsNullOrEmpty(emailEntry.Text);
            bool isPasswordEmpty = string.IsNullOrEmpty(passwordEntry.Text);
            bool isvalid = IsValid(emailEntry.Text);
            DateTime dt = DateTime.Now;

            if (isEmailEmpty || isPasswordEmpty || !isvalid)
            {
                DisplayAlert("Alert", "Please enter valid email address", "OK");
            }
            else
            {
                string url = GenerateUrl(emailEntry.Text);
                 members = ApiWebRequestHelper.GetXmlRequest<List<CccMemberProfile>>(url);
                 bool isEmpty = !members.Any();

                if (isEmpty)
                {
                    DisplayAlert("Alert", "This email does not exist", "OK");

                }
                else
                {
                    members = members.Where(i => i.ExpirationDate > dt).Cast<CccMemberProfile>().ToList();

                    //Navigation.PushAsync(new MembershipsPage(members));
                }
            }
        }

        public static string GenerateUrl(string email)
        {
            return string.Format(Constants.MEMBER_SEARCH, email, Constants.API_KEY);
        }

        public bool IsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
