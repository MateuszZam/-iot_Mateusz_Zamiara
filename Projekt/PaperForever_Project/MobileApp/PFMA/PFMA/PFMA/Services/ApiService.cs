using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PFMA.Models;
using Plugin.Media.Abstractions;
using Xamarin.Essentials;

namespace PFMA.Services
{
    public static class ApiService
    {
        #region UserMethods

        public static async Task<bool> RegisterUser(string name, string email, string password)
        {
            var register = new Register()
            {
                Name = name,
                Email = email,
                Password = password
            };

            var httpClient = new HttpClient();

            var jsonSerialized = JsonConvert.SerializeObject(register);
            var content = new StringContent(jsonSerialized, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "/api/users/register", content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public static async Task<bool> LoginUser(string email, string password)
        {
            var login = new Login()
            {
                Email = email,
                Password = password
            };

            var httpClient = new HttpClient();

            var jsonSerialized = JsonConvert.SerializeObject(login);
            var content = new StringContent(jsonSerialized, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "/api/users/login", content);
            if (!response.IsSuccessStatusCode) return false;

            var jsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<LoginToken>(jsonResult);

            Preferences.Set("accessToken", result.accessToken);
            Preferences.Set("userId", result.userId);
            Preferences.Set("userName", result.userName);

            return true;
        }

        #endregion

        #region ShopMethods

        public static async Task<List<ShopList>> GetAllShops()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + "/api/shops");

            return JsonConvert.DeserializeObject<List<ShopList>>(response);
        }

        public static async Task<List<ShopList>> GetShopsOfType(string shopType)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + "/api/shops/ShopsOfType?type=" + shopType);

            return JsonConvert.DeserializeObject<List<ShopList>>(response);
        }

        public static async Task<List<FindShops>> GetShopsOfName(string shopName)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + "/api/shops/FindShops?name=" + shopName);

            return JsonConvert.DeserializeObject<List<FindShops>>(response);
        }

        public static async Task<ShopDetails> GetShopDetails(int shopId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + "/api/shops/ShopDetails/" + shopId);

            return JsonConvert.DeserializeObject<ShopDetails>(response);
        }

        #endregion

        #region ReceiptMethods

        public static async Task<bool> AddReceipt(MediaFile mediaFile ,Receipt r)
        {

            var httpClient = new HttpClient();

            var content = new MultipartFormDataContent
            {
                {new StringContent(r.Price.ToString()), "Price"},
                {new StringContent(r.UserId.ToString()), "UserId"},
                {new StringContent(r.ShopId.ToString()), "ShopId"}
            };
            content.Add(new StreamContent(new MemoryStream(r.ImageArray)), "Image", mediaFile.Path);

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));

            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "/api/receipts", content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public static async Task<bool> DeleteReceipt(int receiptId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.DeleteAsync(AppSettings.ApiUrl + "/api/receipts/" + receiptId);

            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public static async Task<List<ReceiptList>> GetUserReceipts(int userId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + "/api/receipts/UserReceipts/" + userId);

            return JsonConvert.DeserializeObject<List<ReceiptList>>(response);
        }

        public static async Task<ReceiptDetails> GetReceiptDetails(int userId, int receiptId)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = 
                await httpClient.GetStringAsync(AppSettings.ApiUrl + String.Format("/api/receipts/ReceiptDetails?uid={0}&rid={1}", userId, receiptId));

            return JsonConvert.DeserializeObject<ReceiptDetails>(response);
        }

        #endregion
    }

}
