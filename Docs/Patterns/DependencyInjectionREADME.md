# Dependency Injection and Navigation in WPF
This project uses Dependency Injection to manage ViewModels and Navigation to switch between them. However this section is meant to
have all examples of Dependency Injection but currently it only has examples of navigation.

## Adding new ViewModels to the app
This project utilizes Dependency Injection to manage the ViewModels. To add a new ViewModel to the app, you need to follow these steps:
### 1. Create a new ViewModel
Create a class that will be named according to the pattern : <Your_View_Model_Name>ViewModel.cs. The class should inherit from the ViewModel class.
Then createa a view that will corespond to the ViewModel. The view should be named according to the pattern: <Your_View_Model_Name>View.xaml.
### 2. Creata a DataTemplate for the ViewModel
In the App.xaml file, add a new DataTemplate for the ViewModel. The DataTemplate should look like this:
```xaml
<DataTemplate DataType="{x:Type local:<Your_View_Model_Name>ViewModel}">
	<local:<Your_View_Model_Name>View />
</DataTemplate>
```
This will tell the app to use the <Your_View_Model_Name>View for the <Your_View_Model_Name>ViewModel.

### 3. Register your ViewModel in the DI container in the App.xaml.cs file
To add your ViewModel add this line to the constructor of the App.xaml.cs file:
```csharp
services.AddSingleton<YourViewModel>();
```
This will register your ViewModel in the DI container.

### 4. Using the INavigationSevice
To use the INavigationService you need to inject it into your ViewModel. You can do this by adding a constructor to your ViewModel that takes an INavigationService as a parameter. Then you can use the INavigationService to navigate to your ViewModel.
However this project utilizes many navigation services (for example a subsystem for the transcription text) so you're gonna be using
an interface that derives from INavigationService. However carefully consider if adding a new navigation service is necessary.
Currently the project inculdes the following navigation services:
 - IMainNavigationService - the main nav service used to navigate in the mian widnow
 - ITranscriptionNavigationService - a nav service displayed inside the transcription window in the home view
 - IAlwaysTopWidgetNavigationService - a service for navigation in the always top widget

 When writing a new ViewModel you should consider if you need to use one of the existing navigation services or if you need to create a new one.
 Here is an example of usage of the IMainNavigationService:
```csharp
public class YourViewModel : ViewModel
{
	private readonly IMainNavigationService _navigationService;

	public YourViewModel(IMainNavigationService navigationService)
	{
		_navigationService = navigationService;
	}

	[RelayCommand]
	public void NavigateToYourViewModel()
	{
		_navigationService.NavigateTo<YourViewModel>();
	}
}
```
Here the NavigateToYourViewModel method will navigate to the YourViewModel. Now you can bind this method to a button in the view and when the button is clicked the app will navigate to the YourViewModel.


### 5. Closing remarks
Currently the in the MainWindow.xaml there is a line:
```xaml
<ContentControl Content="{Binding Navigation.CurrentView}" />
```
This is the space where your ViewModel will be displayed when the NavigateToYourViewModel command is called. You can change the layout of the MainView to fit your needs.