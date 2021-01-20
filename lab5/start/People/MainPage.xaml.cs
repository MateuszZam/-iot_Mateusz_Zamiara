using People.Models;
using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace People
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public async void OnNewButtonClicked(object sender, EventArgs args)
        {
            StatusMessage.Text = "";

            await App.PersonRepo.AddNewPersonAsync(NewPerson.Text);
            StatusMessage.Text = App.PersonRepo.StatusMessage;
        }

        public async void OnGetButtonClicked(object sender, EventArgs args)
        {
            StatusMessage.Text = "";

            List<Person> people = await App.PersonRepo.GetAllPeopleAsync();
            PeopleList.ItemsSource = people;
        }
    }
}
