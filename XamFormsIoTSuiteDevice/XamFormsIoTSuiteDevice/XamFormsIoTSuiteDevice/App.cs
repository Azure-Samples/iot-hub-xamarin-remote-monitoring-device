using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using AzureIoTSuiteRemoteMonitoringHelper;
using XamFormsIoTSuiteDevice.Helpers;
using Plugin.DeviceInfo;
using Plugin.Geolocator;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace XamFormsIoTSuiteDevice
{
    public class App : Application
    {
        // Instanciate the Remote Monitoring helper class
        RemoteMonitoringDevice Device = new RemoteMonitoringDevice();

        // UI Resources initialization
        #region UI Resources
        Image HeroImage = new Image
        {
            Aspect = Aspect.AspectFit,
            Source = ImageSource.FromFile("AzureIoTBanner.png")
        };
        Label LabelDeviceId = new Label
        {
            Text = "Device Id:"
        };
        Entry EntryDeviceId = new Entry
        {
            HorizontalTextAlignment = TextAlignment.Start,
            Placeholder = "Enter Device Id here"
        };
        Label LabelHostName = new Label
        {
            Text = "HostName:"
        };
        Entry EntryHostName = new Entry
        {
            HorizontalTextAlignment = TextAlignment.Start,
            Placeholder = "Enter Host Name here"
        };
        Label LabelDeviceKey = new Label
        {
            Text = "Device Key:"
        };
        Entry EntryDeviceKey = new Entry
        {
            HorizontalTextAlignment = TextAlignment.Start,
            Placeholder = "Enter Device Key here"
        };
        Button ButtonConnect = new Button
        {
            Text = "Press to Connect to IoT Suite"
        };
        Button ButtonSend = new Button
        {
            Text = "Press to Send Telemetry Data"
        };
        Label LabelTelemetry1 = new Label
        {
            Text = "Temperature"
        };
        Slider SliderTelemetry1 = new Slider
        {
            Minimum = 0,
            Maximum = 100
        };
        Label LabelTelemetry2 = new Label
        {
            Text = "Humidity"
        };
        Slider SliderTelemetry2 = new Slider
        {
            Minimum = 0,
            Maximum = 100
        };
        #endregion

        /// <summary>
        /// App Constructor
        /// </summary>
        public App()
        {
            // Retreive Settings stored on the device
            Device.DeviceId = Settings.DeviceId;
            Device.HostName = Settings.HostName;
            Device.DeviceKey = Settings.DeviceKey;
            // Add the telemetry data
            Device.AddTelemetry(new TelemetryFormat { Name = "Temperature", DisplayName = "Temperature (C)", Type = "double" }, (double)0);
            Device.AddTelemetry(new TelemetryFormat { Name = "Humidity", DisplayName = "Humidity (%)", Type = "double" }, (double)0);

            // Add a command
            Device.AddCommand(new AzureIoTSuiteRemoteMonitoringHelper.Command { Name = "TriggerAlarm", Parameters = new Collection<CommandParameter> { new CommandParameter { Name = "Message", Type = "String" } } });
            Device.onReceivedMessage += Device_ReceivedMessage;
            
            // If you are developing and want to avoid having to enter the full connection string on the device,
            // you can temporarily hard code it here. Comment this when done!
            //Device.DeviceId = "[DeviceId]";
            //Device.HostName = "[HostName]";
            //Device.DeviceKey = "[DeviceKey]";

            // Update current device location
            Task.Run(async () => {
                try
                {
                    var locator = CrossGeolocator.Current;
                    locator.DesiredAccuracy = 50;

                    var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
                    Device.Model.DeviceProperties.Latitude = position.Latitude;
                    Device.Model.DeviceProperties.Latitude = position.Longitude;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Unable to get location, may need to increase timeout: " + ex);
                }
            });


            // This is the place you can specify the metadata for your device. The below fields are not mandatory.
            Device.Model.DeviceProperties.CreatedTime = DateTime.UtcNow.ToString();
            Device.Model.DeviceProperties.UpdatedTime = DateTime.UtcNow.ToString();
            Device.Model.DeviceProperties.FirmwareVersion = "1.0";
            Device.Model.DeviceProperties.InstalledRAM = "Unknown";
            Device.Model.DeviceProperties.Manufacturer = "Unknown";
            Device.Model.DeviceProperties.ModelNumber = CrossDeviceInfo.Current.Model;
            Device.Model.DeviceProperties.Platform = CrossDeviceInfo.Current.Platform.ToString() + " " + CrossDeviceInfo.Current.Version;
            Device.Model.DeviceProperties.Processor = "Unknown";
            Device.Model.DeviceProperties.SerialNumber = CrossDeviceInfo.Current.Id;

            // Attach Callbacks to UI resources
            EntryDeviceId.TextChanged += EntryDeviceId_TextChanged;
            EntryDeviceId.Text = Device.DeviceId;
            EntryHostName.TextChanged += EntryHostName_TextChanged;
            EntryHostName.Text = Device.HostName;
            EntryDeviceKey.TextChanged += EntryDeviceKey_TextChanged;
            EntryDeviceKey.Text = Device.DeviceKey;
            ButtonConnect.Clicked += ButtonConnect_Clicked;
            ButtonSend.Clicked += ButtonSend_Clicked;
            SliderTelemetry1.ValueChanged += SliderTelemetry1_ValueChanged;
            SliderTelemetry1.Value = 50;
            SliderTelemetry2.ValueChanged += SliderTelemetry2_ValueChanged;
            SliderTelemetry2.Value = 50;

            // The root page of your application
            MainPage = new ContentPage
            {
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Start,
                    Padding = 10,
                    Children = {
                        HeroImage,
                        LabelDeviceId,
                        EntryDeviceId,
                        LabelHostName,
                        EntryHostName,
                        LabelDeviceKey,
                        EntryDeviceKey,
                        ButtonConnect,
                        LabelTelemetry1,
                        SliderTelemetry1,
                        LabelTelemetry2,
                        SliderTelemetry2,
                        ButtonSend
                    }
                }
            };
        }


        private void Device_ReceivedMessage(object sender, EventArgs e)
        {
            if (((ReceivedMessageEventArgs)e).Message.Parameters.ContainsKey("Message"))
            {
                string AlertText = ((ReceivedMessageEventArgs)e).Message.Parameters["Message"].ToString();
                if (AlertText != "")
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() => {
                        App.Current.MainPage.DisplayAlert("Message From Azure IoT Suite", AlertText, "Ok");
                    });
                }
            }
        }

        private bool CheckConfig()
        {
            if (((Device.DeviceId != null) && (Device.DeviceKey != null) && (Device.HostName != null) &&
                        (Device.DeviceId != "") && (Device.DeviceKey != "") && (Device.HostName != "")))
            {
                Settings.DeviceId = Device.DeviceId;
                Settings.DeviceKey = Device.DeviceKey;
                Settings.HostName = Device.HostName;
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SliderTelemetry2_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            LabelTelemetry2.Text = "Humidity: " + e.NewValue.ToString();
            Device.UpdateTelemetryData("Humidity", e.NewValue);
        }

        private void SliderTelemetry1_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            LabelTelemetry1.Text = "Temperature: " + e.NewValue.ToString();
            Device.UpdateTelemetryData("Temperature", e.NewValue);
        }

        private void ButtonSend_Clicked(object sender, EventArgs e)
        {
            if (Device.SendTelemetryData)
            {

                Device.SendTelemetryData = false;
                ButtonSend.Text = "Press to send telemetry data";
            }
            else
            {
                Device.SendTelemetryData = true;
                ButtonSend.Text = "Sending telemetry data";
            }
        }

        private void ButtonConnect_Clicked(object sender, EventArgs e)
        {
            if (Device.IsConnected)
            {
                Device.SendTelemetryData = false;
                if (Device.Disconnect())
                {
                    ButtonSend.IsEnabled = false;
                    EntryDeviceId.IsEnabled = true;
                    EntryDeviceKey.IsEnabled = true;
                    EntryHostName.IsEnabled = true;
                    ButtonConnect.Text = "Press to connect";
                }
            }
            else
            {
                if (Device.Connect())
                {
                    ButtonSend.IsEnabled = true;
                    EntryDeviceId.IsEnabled = false;
                    EntryDeviceKey.IsEnabled = false;
                    EntryHostName.IsEnabled = false;
                    ButtonConnect.Text = "Connected";

                }
            }

        }

        private void EntryDeviceKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            Device.DeviceKey = e.NewTextValue;
            ButtonConnect.IsEnabled = CheckConfig();
        }

        private void EntryHostName_TextChanged(object sender, TextChangedEventArgs e)
        {
            Device.HostName = e.NewTextValue;
            ButtonConnect.IsEnabled = CheckConfig();
        }

        private void EntryDeviceId_TextChanged(object sender, TextChangedEventArgs e)
        {
            Device.DeviceId = e.NewTextValue;
            ButtonConnect.IsEnabled = CheckConfig();
        }
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
