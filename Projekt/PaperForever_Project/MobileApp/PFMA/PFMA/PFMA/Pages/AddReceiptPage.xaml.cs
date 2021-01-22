using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageToArray;
using PFMA.Models;
using PFMA.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PFMA.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddReceiptPage : ContentPage
    {
        private int shopId;
        private MediaFile file;
        public AddReceiptPage(ShopDetails shop)
        {
            InitializeComponent();
            LblUserName.Text = Preferences.Get("userName", string.Empty);
            ChosenShopName.Text = shop.Name;
            shopId = shop.Id;
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

        private async void BtnConfirmReceipt_OnClicked(object sender, EventArgs e)
        {
            var imageArray = FromFile.ToArray(file.GetStream());

            var receipt = new Receipt()
            {
                UserId = Preferences.Get("userId",0),
                ShopId = shopId,
                Price = Convert.ToSingle(PriceInput.Text),
                ImageArray = imageArray
            };

            var response = await ApiService.AddReceipt(file,receipt);

            if (!response)
            {
                await DisplayAlert("Oops", "Something went wrong", "Cancel");
            }
            else
            {
                await DisplayAlert("Hurray!", "Your receipt is now forevered", "OK");
                await Navigation.PushModalAsync(new ReceiptsPage(Preferences.Get("userId", 0)));
            }
        }

        private async void UploadButton_OnTapped(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Oops", "Can't access the gallery!", "Cancel");
                return;
            }

            file = await CrossMedia.Current.PickPhotoAsync();

            if (file == null)
                return;

            ReceiptImage.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });
        }

        private async void OpenCameraButton_OnTapped(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("Oops", "Can't access camera", "Cancel");
                return;
            }

            file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                CompressionQuality = 50
            });

            if (file == null)
                return;

            ReceiptImage.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });
        }
    }
}