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
        [ObservableProperty] private string? _mail = string.Empty;

        [RelayCommand]
        public void Register(object o)
        {
            RegisterUser(o);
        }

        public async void RegisterUser(Object o)
        {
            SecureString? _password = (o as IPasswordContainer)?.Password;
            if (_mail == null || _password == null)
            {
                Debug.WriteLine("Username or password is null");
                return;
            }
            try
            {
                using var httpClient = new HttpClient();
                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(new { email = _mail, password = new System.Net.NetworkCredential(string.Empty, _password).Password }),
                    Encoding.UTF8,
                    "application/json");
                var response = await httpClient.PostAsync("http://localhost:4000/api/register", jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"Registration successfull: {_mail}");
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
        
    }
}
