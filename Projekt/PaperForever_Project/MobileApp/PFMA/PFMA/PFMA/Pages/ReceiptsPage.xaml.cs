using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class ReceiptsPage : ContentPage
    {
        public ObservableCollection<ReceiptList> ReceiptsCollection;

        public ReceiptsPage(int userId)
        {
            InitializeComponent();
            LblUserName.Text = Preferences.Get("userName", string.Empty);
            ReceiptsCollection = new ObservableCollection<ReceiptList>();
            GetReceipts(userId);
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

        private void CvReceipts_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentSelection = e.CurrentSelection.FirstOrDefault() as ReceiptList;
            if (currentSelection == null) return;
            Navigation.PushModalAsync(new ReceiptDetailsPage(Preferences.Get("userId", 0),currentSelection.Id));
            ((CollectionView)sender).SelectedItem = null;
        }

        private async void GetReceipts(int userId)
        {
            var receipts = await ApiService.GetUserReceipts(userId);

            foreach (var r in receipts)
            {
                ReceiptsCollection.Add(r);
            }

            CvReceipts.ItemsSource = ReceiptsCollection;
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
    }
}