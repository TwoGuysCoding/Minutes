using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Minutes.Core;
using Minutes.Utils;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using System.Security;
using Minutes.Services;

namespace Minutes.MVVM.ViewModels
{
    internal partial class LoginViewModel : ViewModel
    {
        [ObservableProperty] private string? _message = string.Empty;
        [ObservableProperty] private string? _mail = string.Empty;
        [ObservableProperty] private int _tabIndex = 0;
        private IMainNavigationService _mainNavigationService;


        [RelayCommand]
        public void Register(object o)
        {
            RegisterUser(o);
        }

        public LoginViewModel(IMainNavigationService navService)
        {
            _mainNavigationService = navService;
        }

        public async void RegisterUser(object o)
        {
            var password = (o as IPasswordContainer)?.Password;
            if (Mail == null || password == null)
            {
                Debug.WriteLine("Username or password is null");
                return;
            }
            try
            {
                Message = "Creating account...";
                using var httpClient = new HttpClient();
                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(new { email = Mail, password = new System.Net.NetworkCredential(string.Empty, password).Password }),
                    Encoding.UTF8,
                    "application/json");
                var response = await httpClient.PostAsync("http://localhost:4000/api/register", jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"Registration successfull: {Mail}");
                    TabIndex = 0;
                    Message = "Account created successfully! Please check your mail!";
                }
                else
                {
                    Debug.WriteLine("Failed Registration");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception occured while registration request: {e.Message}");
            }
        }

        [RelayCommand]
        public void Login(object o)
        {
            LoginUser(o);
        }

        public async void LoginUser(object o)
        {
            var password = (o as IPasswordContainer)?.Password;
            if (Mail == null || password == null)
            {
                Debug.WriteLine("Username or password is null");
                return;
            }
            try
            { 
                Message = "Logging in...";
                using var httpClient = new HttpClient();
                var jsonContent = new StringContent(JsonConvert.SerializeObject(
                    new { email = Mail, password = new System.Net.NetworkCredential(string.Empty, password).Password }), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("http://localhost:4000/api/create_token", jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"Login successfull: {Mail}");
                    Message = "Login successfull!";
                    _mainNavigationService.NavigateTo<HomeViewModel>();
                }
                else
                {
                    Debug.WriteLine("Failed Login");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception occured while login request: {e.Message}");
            }
        }
        
    }
}
