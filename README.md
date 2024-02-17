# Minutes
A WPF-based application for serving Speech2TextAPI to Windows users. 
Provides production grade front-end dedicated for seamless integration with the API

# App overview
Currently the app can recod audio from the sound card, send it to the server and receive the transcribed text. The app also provides a simple UI for the user to interact with the app.
When the window loads it connects to the websocket server. When the user clicks the record button, the app starts recording audio and sends it to the server. The server then sends the transcribed text back to the app, which is displayed in the text box.

# Docs
1. [Overview of the models](./Docs/Modules/ModelsFolderREADME.md)
2. [Overview of project structure](./Docs/Modules/ProjectStructureREADME.md)
3. [Overview of DI](./Docs/Patterns/DependencyInjectionREADME.md)
4. [Overview of Mediator pattern](./Docs/Patterns/MediatorREADME.md)

## About audio visualisation and record button style
The RecordButton style works by binding to IsRecording propery in the HomeViewModel. If there is no such property it will cause a binding error.