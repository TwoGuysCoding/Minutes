using Microsoft.Extensions.DependencyInjection;
using Minutes.Core;
using Minutes.MVVM.ViewModels;
using Minutes.Services;
using Minutes.Windows;
using System.Windows;
using Serilog;

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

            services.AddSingleton<ITimerService, TimerService>();
            services.AddSingleton<ITranscriptionService, TranscriptionService>();

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            SetUpLogger();
            mainWindow.Show();
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Log.Information("Application Exiting");
            Log.CloseAndFlush();
            base.OnExit(e);
        }

        private void SetUpLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/myapplog-.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("Application Starting");
        }
    }

}
