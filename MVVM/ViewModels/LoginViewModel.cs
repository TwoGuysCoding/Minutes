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
using System.Text.RegularExpressions;
using Minutes.Services;
using Application = System.Windows.Application;
using System.Windows.Shell;
using System.Windows;

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

            Application.Current.MainWindow.WindowState = WindowState.Normal;
            WindowChrome.SetWindowChrome(Application.Current.MainWindow, new WindowChrome
            {
                CaptionHeight = 0,
                CornerRadius = new CornerRadius(10),
                GlassFrameThickness = new Thickness(0),
                ResizeBorderThickness = new Thickness(10)
            });
        }

        public async void RegisterUser(object o)
        {
            var password = (o as IPasswordContainer)?.Password;
            if (Mail == null || password == null)
            {
                Debug.WriteLine("Username or password is null");
                return;
            }

            if (!Regex.IsMatch(Mail,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)))
            {
                Debug.WriteLine("Invalid email");
                Message = "Invalid email!";
                return;
            }
            if (password.Length < 12)
            {
                Debug.WriteLine("Password is too short");
                Message = "Password is too short! Min 12 characters";
                return;
            }
            try
            {
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
            if (!Regex.IsMatch(Mail,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)))
            {
                Debug.WriteLine("Invalid email");
                Message = "Invalid email!";
                return;
            }
            if (password.Length < 12)
            {
                Debug.WriteLine("Password is too short");
                Message = "Password is too short! Min 12 characters";
                return;
            }
            try
            { 
                using var httpClient = new HttpClient();
                var jsonContent = new StringContent(JsonConvert.SerializeObject(
                    new { email = Mail, password = new System.Net.NetworkCredential(string.Empty, password).Password }), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("http://localhost:4000/api/create_token", jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"Login successful: {Mail}");
                    Message = "Login successful!";
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


        [RelayCommand]
        private void Exit()
        {
            Application.Current.Shutdown();
        }

        [RelayCommand]
        private void Min()
        {
            Application.Current.MainWindow.WindowState = System.Windows.WindowState.Minimized;
        }

    }
}
