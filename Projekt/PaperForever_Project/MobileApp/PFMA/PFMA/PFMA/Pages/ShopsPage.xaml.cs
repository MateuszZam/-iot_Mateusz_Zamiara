using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class ShopsPage : ContentPage
    {
        public ObservableCollection<ShopList> ShopsCollection;
        public ShopsPage()
        {
            InitializeComponent();
            LblUserName.Text = Preferences.Get("userName", string.Empty);
            ShopsCollection = new ObservableCollection<ShopList>();
            GetShops();
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

        private async void GetShops()
        {
            var shops = await ApiService.GetAllShops();

            foreach (var s in shops)
            {
                var imageStream = new MemoryStream(s.ImageBytes);
                s.Image = ImageSource.FromStream(() => imageStream);
                ShopsCollection.Add(s);
            }

            CvShops.ItemsSource = ShopsCollection;
        }

        private async void GetShopsOfType(string shopType)
        {
            ShopsCollection.Clear();
            var shops = await ApiService.GetShopsOfType(shopType);

            foreach (var s in shops)
            {
                var imageStream = new MemoryStream(s.ImageBytes);
                s.Image = ImageSource.FromStream(() => imageStream);
                ShopsCollection.Add(s);
            }

            CvShops.ItemsSource = ShopsCollection;
        }

        private void CvShops_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentSelection = e.CurrentSelection.FirstOrDefault() as ShopList;
            if (currentSelection == null) return;
            Navigation.PushModalAsync(new ShopDetailsPage(currentSelection.Id));
            ((CollectionView) sender).SelectedItem = null;
        }

        private void SearchAction_OnTapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new SearchShopsPage());
        }

        private void SignOut_OnTapped(object sender, EventArgs e)
        {
            Preferences.Set("accessToken", string.Empty);
            Preferences.Set("validationEnd", 0);
            Application.Current.MainPage = new NavigationPage(new SignInPage());
        }

        private void ShowReceipts_OnTapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ReceiptsPage(Preferences.Get("userId", 0)));
        }

        private void ShowShops_OnTapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ShopsPage());
        }

        private void TypePicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedType = TypePicker.Items[TypePicker.SelectedIndex];

            if (selectedType != "Pick type")
            {
                GetShopsOfType(selectedType);
            }
        }
    }
}