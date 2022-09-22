# cityWeather
Name-Surname: Mehmet İZBUL

Country of residency: CYPRUS

This project consists of two parts.

-	The first part (Step 1) is a CLI application that displays the city weather for next 2 days.

-	The second part (Step 2) is the web API design to store/retrieve weather forecast for a specific city to/from TUI Musement.

## The Folder Structure

The "System Requirements and Design" document (R&D document.pdf) is inside the root folder, which describes both projects in detail.
The "CLI Application Implementation" part of the document describes the "Step 1" and "Web API Design" part explains the API design, which is requested in "Step 2" on the assessment document.
Please follow along with this document as needed for both steps.

There are 2 sub-folders. 

### The "Step 1" Folder
This folder includes the source code and the windows installer file for the first part, CLI application. The application is being developed using Visual Studio IDE, with C# programming language.
The the project source code is inside "Step 1/Source Code" folder. The windows installer file (cityWeatherSetup.msi) is inside "Step 1/Installer" folder. The installer sets up a standalone executable
file in Windows environment. Simply double clicking "cityWeatherSetup.msi" will install the application with few "next" clicks. Then double clicking the installed application will
start to display the weathers for the cities in desired format in command-prompt environment.


### The "Step 2" Folder
This folder contains 2 files which are the actual API design documents created using "https://editor.swagger.io/". 

- "weatherApiDesignSwagger.txt" file is the raw swagger file and it can be pasted into "https://editor.swagger.io/" address to be able to see the design in swagger environment.
- "weatherApiDesign.json" is the JSON converted version of the "weatherApiDesignSwagger.txt" file which is a common notation among software developers.





