# Minutes
A WPF-based application for serving Speech2TextAPI to Windows users. 
Provides production grade front-end dedicated for seamless integration with the API

# App overview
Currently the app can recod audio from the sound card, send it to the server and receive the transcribed text. The app also provides a simple UI for the user to interact with the app.
When the window loads it connects to the websocket server. When the user clicks the record button, the app starts recording audio and sends it to the server. The server then sends the transcribed text back to the app, which is displayed in the text box.

# Systems
An overview of all systems, their interactions and usage.

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

## Using Mediator pattern to communicate between ViewModels
This project uses the Mediator pattern to anonymously communicate between ViewModels. 
To send a message from one ViewModel to another, you need to follow these steps:
### 1. Register the the function that will handle the message
To register the functio that will handle the message you can use the function:
```csharp
Mediator.Instance.Register("Your_message_name", Your_recive_function);
```
The register function takes two arguments. The first one is the message name, 
the second one is the function that will handle the message. 
The function should take one argument of type object.
This is the message that will be sent to the function.

### 2. Send the message
To send the message you can use the function:
```csharp
Mediator.Instance.Send("Your_message_name", data);
```
The send function takes two arguments. The first one is the message name (the same one that you used in previous step)
, the second one is the data that will be sent as the parameter to the function that will handle the message.

### 3. Unregister the function
If for some reason you want to unregister the function that handles the message, you can use the function:
```csharp
Mediator.Instance.Unregister("Your_message_name", Your_recive_function);
```
The function works just like the Register function but it unregisters the function that handles the message.

## About audio visualisation and record button style
The RecordButton style works by binding to IsRecording propery in the HomeViewModel. If there is no such property it will caouse a binding error.