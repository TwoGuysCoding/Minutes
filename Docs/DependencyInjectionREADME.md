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

### 4. Add your ViewModel to the MainViewModel
In the MainViewModel, add a command that will navigate to your ViewModel. The command should look like this:
```csharp
[RelayCommand]
public void NavigateToYourViewModel()
{
	Navigation.NavigateTo<YourViewModel>();
}
```
This will create a YourViewModelCommand that can be assinged to for example button. Instead of YourViewModel, use the name of your ViewModel.

Now you can bind the command to a button in the MainView. When the button is clicked, the app will navigate to your ViewModel.

### 5. Closing remarks
Currently the in the MainWindow.xaml there is a line:
```xaml
<ContentControl Content="{Binding Navigation.CurrentView}" />
```
This is the space where your ViewModel will be displayed when the NavigateToYourViewModel command is called. You can change the layout of the MainView to fit your needs.