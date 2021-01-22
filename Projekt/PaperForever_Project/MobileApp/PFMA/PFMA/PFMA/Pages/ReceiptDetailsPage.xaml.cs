using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class ReceiptDetailsPage : ContentPage
    {
        private int chosenReceiptId;
        public ReceiptDetailsPage(int userId, int receiptId)
        {
            InitializeComponent();
            LblUserName.Text = Preferences.Get("userName", string.Empty);
            chosenReceiptId = receiptId;
            GetReceiptDetails(userId,receiptId);
        }

        private async void GetReceiptDetails(int userId, int receiptId)
        {
            var receipt = await ApiService.GetReceiptDetails(userId,receiptId);
            ReceiptShopName.Text = receipt.ShopName;
            ReceiptDate.Text = receipt.ReceiptDateTime.ToShortDateString();
            ReceiptPrice.Text = receipt.Price.ToString();
            var imageStream = new MemoryStream(receipt.ImageBytes);
            ReceiptImage.Source = ImageSource.FromStream(() => imageStream);
        }

        private async void SideMenu_OnTapped(object sender, EventArgs e)
        {
            GridOverlay.IsVisible = true;
            await SlMenu.TranslateTo(0, 0, 400, Easing.Linear);
        }

        private void BackButton_OnTapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private async void BtnDeleteReceipt_OnClicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Confirm", "Do you want to delete this receipt?", "Yes", "No!");

            if (result)
            {
                var response = await ApiService.DeleteReceipt(chosenReceiptId);

                if (response == false) return;
                await Navigation.PushModalAsync(new ReceiptsPage(Preferences.Get("userId", 0)));
            }
        }

        private void ShowShops_OnTapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ShopsPage());
        }

        private void ShowReceipts_OnTapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ReceiptsPage(Preferences.Get("userId", 0)));
        }

        private void SignOut_OnTapped(object sender, EventArgs e)
        {
            Preferences.Set("accessToken", string.Empty);
            Preferences.Set("validationEnd", 0);
            Application.Current.MainPage = new NavigationPage(new SignInPage());
        }

        private async void TapCloseMenu_OnTapped(object sender, EventArgs e)
        {
            await SlMenu.TranslateTo(-250, 0, 400, Easing.Linear);
            GridOverlay.IsVisible = false;
        }
    }
}