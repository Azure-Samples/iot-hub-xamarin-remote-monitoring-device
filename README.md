---
services: iot-hub, iot-suite
platforms: csharp, xamarin
author: olivierbloch
---

# Connect iOS, Android and Windows devices to Azure IoT Suite Remote Monitoring using Xamarin
This sample shows how to run and connect a Xamarin mobile application to an Azure IoT Suite Remote Monitoring Preconfigured Solution.
This sample is useful to demonstrate the Azure IoT Suite using a mobile device (Android, iOS, Windows).
Here are some links to learn more on [Azure IoT Suite](https://azure.microsoft.com/en-us/documentation/suites/iot-suite/) and [Azure IoT Suite preconfigured solutions](https://azure.microsoft.com/en-us/documentation/articles/iot-suite-what-are-preconfigured-solutions/).

## Running this sample
The repository contains 2 samples, one for Xamarin native and one for Xamarin Forms. There is one solution for each.
The Xamarin native solution targets Android, iOS and Windows Phone 8.1.
The Xamarin Forms solution targets Android, iOS, UWP (Windows 10 and Windows 10 Mobile), Windows Phone 8.1 and Windows 8.1

### Prerequisites

You can build and run the samples on a Windows machine or on a Mac.

#### Developing on Windows 
On Windows you will be able to build and run the samples on the following platforms:

- Android (Xamarin native and Xamarin Forms samples)
- iOS (Xamarin native and Xamarin Forms samples) (note that you will need a Mac with the dev tools installed next to your Windows dev machine to compile and run the sample for iOS)
- Windows 10 (Xamarin Forms)
- Windows 10 mobile (Xamarin Forms)
- Windows 8.1 (Xamarin Forms sample)
- Windows Phone 8.1 (Xamarin native and Xamarin Forms samples)

Requirements:

- A PC running Windows 10 (this would be your development machine).
- [Visual Studio 2015 Update 2](https://www.visualstudio.com/) making sur you checked the **Cross Platform Mobile Development | C#/.Net (Xamarin)** option during install
- [optional] [Windows 10 SDK](https://dev.windows.com/en-US/downloads/windows-10-sdk) if you want to build and run the UWP project.
- [optional] A Mac machine with the development tools installed for Xamarin (see below for the tools requirements).
- [optional] A mobile device running one of the following: iOS, Android, Windows 10, Windows 10 mobile, Windows 8.1, Windows Phone 8.1 if you want to deploy the application on a real device. Otherwise you can use emulators and simulators that come along with Xamarin and related tools.
- [optional] [Xamarin Android Player](https://www.xamarin.com/android-player). This one is a great Android emulator for development. 

To target Windows 10, you will need to setup your OS to developer mode:

- On Windows 10 PC:
    1. Click on the Windows Icon, 
    1. Type `For Developers Settings` and press enter.
    1. In the **Developers Settings** section, select the option **Developer Mode**
- On Windows 10 Mobile (if you plan to deploy to a phone running Windows 10 mobile)
    1. Touch the search button
    1. Type `Settings` and touch the **Settings** icon to enter the settings panel
    1. Scroll down to **Update & Security**, then Developers and select the **Developer mode** option
      
#### Developing on a Mac
On Mac you will be able to build and run the samples on the following platforms:

- Android (Xamarin native and Xamarin Forms samples)
- iOS ( (Xamarin native and Xamarin Forms samples)

Requirements:

- A Mac running OSx 10.10.5+ or 10.11
- XCode 7.1
- [Xamarin Studio](https://www.xamarin.com/download)
- [optional] [Xamarin Android Player](https://www.xamarin.com/android-player). This one is a great Android emulator for development. 

To build and deploy to iOS device you will need to have an Apple Developer account.


## Deploy an Azure IoT Suite Remote Monitoring preconfigured solution

In order to deploy an Azure IoT Suite preconfigured solution, you need an Azure subscription. If you don't have one, you can easily create a [free trial subscription](https://azure.microsoft.com/en-us/free/).
This [article](https://azure.microsoft.com/en-us/documentation/articles/iot-suite-getstarted-preconfigured-solutions/) describes in details how to get started with Azure IoT Suite Remote Monitoring preconfigured solutions, but if you want the short version, see below.
Once you have an Azure subscription, browse to [http://www.azureiotsuite.com](http://www.azureiotsuite.com)
Once logged in using your Azure subscription credentials:

1. Click on **Create a new solution**.
1. Select **Remote Monitoring**
1. Enter a solution name
1. Select a region for your solution to be hosted in
1. Select your subscription (if you have several subscriptions for the account you're logged in with)
1. Click on **Create solution** at the bottom

It will take several minutes to deploy all the services of the solution, in the meantime, you can get the device application ready.

## Run the device application on Windows 10 PC and mobile

In order to run the device application on your PC, here are the few steps:

1. Clone or download the github repository (see links on top)
1. For the Xamarin Native sample, open the solution XamNativeIoTSuiteDevice\XamNativeIoTSuiteDevice.sln in Visual Studio (for Windows) or Xamarin Studio (for Mac), build, run.
1. For the Xamarin Forms sample, open the solution XamFormsIoTSuiteDevice\XamFormsIoTSuiteDevice.sln in Visual Studio (for Windows) or Xamarin Studio (for Mac), build, run.

## Create a device ID for your mobile device application in Azure IoT Suite Remote Monitoring preconfigured solution

At this point the Remote Monitoring solution should be deployed (if not, go get a coffee).

![](https://raw.githubusercontent.com/Azure-Samples/iot-hub-dotnet-uwp-remote-monitoring/master/Media/IoTSuiteSolution.PNG)

```
Important: we are not using the simulated devices that are automatically deployed as 
part of the remote monitoring solution. 

It is recommended to deactivate all the simulated devices from the Devices tab in 
the dashboard to prevent unecessary traffic and cost to the Azure subscription.
```

In order to connect your mobile application to your Azure IoT Suite instance (which by now should be deployed), you will need to create a unique ID for it in the Suite dashboard.
Navigating the Remote Monitoring dahboard and creating a device ID is extensively described in the [Getting Started with Azure IoT Suite preconfigured solutions article](https://azure.microsoft.com/en-us/documentation/articles/iot-suite-getstarted-preconfigured-solutions/).
Once you have created a new device ID, copy the Device ID, Host Name and Device Key from the IoT Suite into the mobile application.

## Use the application

The device application is dead simple.

1. Connecting to IoT Suite:
    - Enter the credentials generated in previous steps in the corresponding fields of the app.
    - Press the "Press to Connect To IoT Suite" button.
    - At this point you should see the device metadata appear in the IoT Suite dashboard under the **devices** tab.
1. Sending telemetry data
    - Press the **Press to send data to IoT Suite** button.
    - Data will start showing up in the IoT Suite dashboard. You can play with the sliders to change the values
1. Receiving messages from IoT Suite on the device
    - In the IoT Suite dashboard, go to the **Devices** tab
    - Select your device
    - In the right menu, press on **Commands**
    - In the Commands combo, select the command you want to send to the device, type your text and press **send**
    - The device should display the message.
