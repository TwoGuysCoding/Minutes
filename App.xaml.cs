using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Minutes.Core;
using Minutes.MVVM.ViewModels;
using Minutes.Services;

namespace Minutes
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton(provider => new MainWindow
            {
                DataContext = provider.GetRequiredService<MainViewModel>()
            });
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<TranscriptionTextViewModel>();
            services.AddSingleton<SummaryTextViewModel>();
            services.AddSingleton<AlwaysTopWidgetViewModel>();
            services.AddSingleton<EnhancedTranscriptionTextViewModel>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<ITextDisplayNavigationService, NavigationService>();

            services.AddSingleton<Func<Type, ViewModel>>(serviceProvider =>
                viewModelType => (ViewModel)serviceProvider.GetRequiredService(viewModelType));

            services.AddSingleton<WasapiLoopBackCaptureRecordingDevice>();
            services.AddSingleton<WaveInEventRecordingDevice>();

            services.AddSingleton<Func<RecordingDeviceType, IRecordingDevice>>(serviceProvider => key =>
            {
                return key switch
                {
                    RecordingDeviceType.WasapiLoopBackCapture => serviceProvider
                        .GetRequiredService<WasapiLoopBackCaptureRecordingDevice>(),
                    RecordingDeviceType.WaveInEvent => serviceProvider.GetRequiredService<WaveInEventRecordingDevice>(),
                    _ => throw new KeyNotFoundException()
                };
            });
            services.AddTransient<IRecordingService, RecordingService>();

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }
    }

}
