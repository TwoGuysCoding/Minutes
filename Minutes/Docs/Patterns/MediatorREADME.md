# Mediator pattern
This is a simple implementation of the Mediator pattern in C#.
It's goal is to provide a simple way to communicate between different parts of the application without the need to reference them directly.
If you need a simple way to communicate between different parts of your application, this is the right pattern for you.

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