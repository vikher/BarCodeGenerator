using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterwayBarCodeGenerator.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace WaterwayBarCodeGenerator
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{


        private string checksumDigit;
        private string numberBeforeChecksumDigit;
        private string memberNumber;

        public HomePage(CccMemberProfile user)
        {
            InitializeComponent();
            BindingContext = user;
            numberBeforeChecksumDigit = "0727" + user.Number + "0";
            checksumDigit = CalculateChecksumDigit(numberBeforeChecksumDigit);
            memberNumber = numberBeforeChecksumDigit + checksumDigit;
            barcodeImage.BarcodeValue = memberNumber;
            memberNumberBarcode.Text = memberNumber;
        }

        public string CalculateChecksumDigit(string sTemp)
        {
            int iSum = 0;
            int iDigit = 0;

            // Calculate the checksum digit here.
            for (int i = 1; i <= sTemp.Length; i++)
            {
                iDigit = Convert.ToInt32(sTemp.Substring(i - 1, 1));
                if (i % 2 == 0)
                {    // even
                    iSum += iDigit * 1;
                }
                else
                {    // odd
                    iSum += iDigit * 3;
                }
            }

            int iCheckSum = (10 - (iSum % 10)) % 10;
            return iCheckSum.ToString();

        }
    }
}