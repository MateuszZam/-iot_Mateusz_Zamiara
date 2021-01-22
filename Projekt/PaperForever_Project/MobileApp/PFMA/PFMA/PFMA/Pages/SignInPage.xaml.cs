using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMA.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PFMA.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignInPage : ContentPage
    {
        public SignInPage()
        {
            InitializeComponent();
        }

        private async void BtnSignIn_OnClicked(object sender, EventArgs e)
        {
            var response = await ApiService.LoginUser(LogEmail.Text, LogPassword.Text);

            if (response)
            {
                Application.Current.MainPage = new NavigationPage(new ShopsPage());
            }
            else
            {
                await DisplayAlert("Dang it!", "Something went wrong", "Cancel");
            }
        }

        private void LblSignUp_OnTapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new SignUpPage());
        }
    }
}