using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatAppDTO;
using Microsoft.AspNetCore.SignalR.Client;
using MobileChatApp.Models;
using Xamarin.Forms;

namespace MobileChatApp
{
    public partial class MainPage : ContentPage
    {
        private HubConnection conn;
        private List<ChatMessageListItem> msgList = new List<ChatMessageListItem>();
        private string userNamePhone = "Matt";
        public MainPage()
        {
            InitializeComponent();
            conn = new HubConnectionBuilder()
                .WithUrl("http://192.168.100.11:5000/chat")
                .Build();

            conn.StartAsync();

            conn.On<ChatMessage>("ReceivedMessage", (msg) =>
            {
                var chatListItem = new ChatMessageListItem(msg);
                msgList.Insert(0, chatListItem);
            });
            chatList.ItemsSource = msgList;

            btnSend.Clicked += BtnSend_Clicked;
            btnSignIn.Clicked += BtnSignIn_Clicked;
        }

        private void BtnSignIn_Clicked(object sender, EventArgs e)
        {
            userNamePhone = textUser.Text;
            if (string.IsNullOrWhiteSpace(userNamePhone) || userNamePhone.Length < 3)
            {
                DisplayAlert("Error", "Username above 3 letters required!", "Ok");
                return;
            }

            conn.SendAsync("SignInUser", userNamePhone);

            gridLogin.IsVisible = false;
            gridChat.IsVisible = true;
        }

        private void BtnSend_Clicked(object sender, EventArgs e)
        {
            var message = textMsg.Text;
            if (string.IsNullOrWhiteSpace(message))
            {
                DisplayAlert("Error", "Required text!", "Ok");
                return;
            }

            conn.SendAsync("SendMessage", new ChatMessage
            {
                Message = message,
                UserName = userNamePhone
            });

            textMsg.Text = string.Empty;
        }
    }
}
