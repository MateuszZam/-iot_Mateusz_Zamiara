using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMA.Models;
using PFMA.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PFMA.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShopDetailsPage : ContentPage
    {
        private ShopDetails shop;
        public ShopDetailsPage(int shopId)
        {
            InitializeComponent();
            LblUserName.Text = Preferences.Get("userName", string.Empty);
            GetShopDetails(shopId);
        }

        private async void SideMenu_OnTapped(object sender, EventArgs e)
        {
            GridOverlay.IsVisible = true;
            await SlMenu.TranslateTo(0, 0, 400, Easing.Linear);
        }

        private async void TapCloseMenu_OnTapped(object sender, EventArgs e)
        {
            await SlMenu.TranslateTo(-250, 0, 400, Easing.Linear);
            GridOverlay.IsVisible = false;
        }

        private async void GetShopDetails(int shopId)
        {
            shop = await ApiService.GetShopDetails(shopId);
            ShopNameLabel.Text = shop.Name;
            ShopCategory.Text = shop.Type;
            ShopsInCityCount.Text = shop.InCityCount.ToString();
            var imageStream = new MemoryStream(shop.ImageBytes);
            ShopImage.Source = ImageSource.FromStream(() => imageStream);
        }

        private void BackButton_OnTapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
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

        private void BtnAddReceipt_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new AddReceiptPage(shop));
        }
    }
}