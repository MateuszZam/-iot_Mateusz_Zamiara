using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFMA.Models;
using PFMA.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PFMA.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchShopsPage : ContentPage
    {
        public SearchShopsPage()
        {
            InitializeComponent();
        }

        private async void ShopNameInput_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == null)
            {
                return;
            }

            var shopsList = await ApiService.GetShopsOfName(e.NewTextValue.ToLower());

            CvShops.ItemsSource = shopsList;
        }

        private void CvShops_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentSelection = e.CurrentSelection.FirstOrDefault() as FindShops;
            if (currentSelection == null) return;
            Navigation.PushModalAsync(new ShopDetailsPage(currentSelection.Id));
            ((CollectionView)sender).SelectedItem = null;
        }

        private void SearchBack_OnTapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}