using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Minutes.Core;
using Minutes.MVVM.ViewModels;
using Minutes.Services;
using Minutes.Windows;

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

            services.AddSingleton<MainWindow>(provider => new MainWindow
            {
                DataContext = provider.GetRequiredService<MainViewModel>()
            });
            services.AddSingleton<AlwaysTopWidgetWindow>(provider => new AlwaysTopWidgetWindow
            {
                DataContext = provider.GetRequiredService<AlwaysTopWidgetMainViewModel>()
            });
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<AlwaysTopWidgetMainViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<TranscriptionTextViewModel>();
            services.AddSingleton<SummaryTextViewModel>();
            services.AddSingleton<AlwaysTopWidgetHomeViewModel>();
            services.AddSingleton<EnhancedTranscriptionTextViewModel>();
            services.AddSingleton<IMainNavigationService, NavigationService>();
            services.AddSingleton<ITextDisplayNavigationService, NavigationService>();
            services.AddSingleton<IAlwaysTopWidgetNavigationService, NavigationService>();
            services.AddSingleton<IWindowNavigationService, WindowNavigationService>();

            services.AddSingleton<Func<Type, ViewModel>>(serviceProvider =>
                viewModelType => (ViewModel)serviceProvider.GetRequiredService(viewModelType));

            services.AddSingleton<Func<Type, Window>>(serviceProvider =>
                windowType => (Window)serviceProvider.GetRequiredService(windowType));

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
            services.AddSingleton<IRecordingService, RecordingService>();

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
