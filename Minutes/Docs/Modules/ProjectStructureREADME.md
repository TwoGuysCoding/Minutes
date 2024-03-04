# Overview of the project structure
This project is divided into many folders. This is a brief overview of the project structure and the contents of each folder.
## 1. MVVM folder
This folder contains the MVVM architecture of the project. It is divided into 3 subfolders:
### -Model
This folder contains all the .cs file that are used to communicate with the outside world. In general,
anything than is not UI or is not used to communicate with the UI is placed in this folder.
### -ViewModel
This folders contains all the ViewModels of the project. 
ViewModels are classes that are used to communicate between the Model and the View. They are also used to store the state of the View.
They are basically the middleman between the Model and the View.
### -View
This folder contains all the Views of the project. Views are the UI of the project.
Basically all the xaml files go here.

### Important note
Everything you could describe as a "page" could be considerd a View. 
If it is big enough it's good to consider creating a ViewModel for it.
If you decide to create a new View make sure it's a UserControl and not a Page.

## 2. Services folder
This folder is used to store all services that are used in the project. A good example of a service is the NavigationService.
This service is used to navigate between pages. It is used in the ViewModels to navigate between pages and is implemented using
Dependency Injection pattern. To read more check the section about Dependency Injection.

## 3. Resources folder
This folder is used to store all the xaml dictionaries that are used in the project.
Currently it contains only the Colors.xaml and the Styles.xaml files.
The Colors.xaml file is used to store all the colors that are used in the project.
The Styles.xaml file is used to store all the styles that are used in the project.

## 4. Icons and Fonts folder
This folder is used to store all the icons and fonts that are used in the project.
When adding a new icon or font make sure to add it to the project and set the Build Action 
to "Resource" and the Copy to Output Directory to "Copy always".
When adding a new font make sure to add it to the App.xaml file and 
set the Build Action to "Resource" and the Copy to Output Directory to "Do not copy".

## 5. Core folder
This folder is used to store all the core classes that are used in the project. All base classes are stored here.

## 6. Utils folder
Everything that can be considered a utility goes here. Currently it contains only the Mediator class.