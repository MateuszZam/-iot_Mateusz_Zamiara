using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMA.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PFMA.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
        }

        private async void BtnSignUp_OnClicked(object sender, EventArgs e)
        {
            var response = await ApiService.RegisterUser(RegName.Text, RegEmail.Text, RegPassword.Text);
            if (response)
            {
                await DisplayAlert("Hi there!", "Your account has been created", "OK");
                await Navigation.PushModalAsync(new SignInPage());
            }
            else
            {
                await DisplayAlert("Dang it!", "Something went wrong", "Cancel");
            }
        }

        private void LblSignIn_OnTapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new SignInPage());
        }
    }
}