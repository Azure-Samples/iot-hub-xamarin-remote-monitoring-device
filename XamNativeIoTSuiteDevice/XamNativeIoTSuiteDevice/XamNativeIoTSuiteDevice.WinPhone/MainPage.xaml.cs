using AzureIoTSuiteRemoteMonitoringHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace XamNativeIoTSuiteDevice.WinPhone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MyClass Device;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            // Initialize IoT Hub client
            Device = new MyClass();

            // Add the telemetry data
            Device.AddTelemetry(new TelemetryFormat { Name = "Temperature", DisplayName = "Temp (C)", Type = "Double" }, (double)0);
            Device.AddTelemetry(new TelemetryFormat { Name = "Humidity", DisplayName = "Hmdt (%)", Type = "Double" }, (double)0);

            this.buttonConnect.IsEnabled = false;
            this.buttonConnect.Content = "Press to Connect";
            this.buttonConnect.Click += ButtonConnect_Click;

            this.buttonSend.IsEnabled = false;
            this.buttonSend.Content = "Press to send telemetry data";
            this.buttonSend.Click += ButtonSend_Click;

            this.textDeviceId.TextChanged += TextDeviceId_TextChanged;
            this.textDeviceId.Text = Device.DeviceId;

            this.textDeviceKey.TextChanged += TextDeviceKey_TextChanged;
            this.textDeviceKey.Text = Device.DeviceKey;

            this.textHostName.TextChanged += TextHostName_TextChanged;
            this.textHostName.Text = Device.HostName;

            this.sliderTemperature.ValueChanged += SliderTemperature_ValueChanged;
            this.sliderTemperature.Value = 50;

            this.sliderHumidity.ValueChanged += SliderHumidity_ValueChanged;
            this.sliderHumidity.Value = 50;

            // Set focus to the connect button
            buttonConnect.IsEnabled = Device.checkConfig();
        }

        private void SliderTemperature_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            textTemperature.Text = "Temperature: " + e.NewValue.ToString();
            Device.UpdateTelemetryData("Temperature", e.NewValue);
        }

        private void SliderHumidity_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            textHumidity.Text = "Humidity: " + e.NewValue.ToString();
            Device.UpdateTelemetryData("Humidity", e.NewValue);
        }

        private void TextHostName_TextChanged(object sender, TextChangedEventArgs e)
        {
            Device.HostName= textHostName.Text;
            buttonConnect.IsEnabled = Device.checkConfig();
        }

        private void TextDeviceKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            Device.DeviceKey = textDeviceKey.Text;
            buttonConnect.IsEnabled = Device.checkConfig();
        }

        private void TextDeviceId_TextChanged(object sender, TextChangedEventArgs e)
        {
            Device.DeviceId = textDeviceId.Text;
            buttonConnect.IsEnabled = Device.checkConfig();
        }

        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            if (Device.SendTelemetryData)
            {

                Device.SendTelemetryData = false;
                buttonSend.Content = "Press to send telemetry data";
            }
            else
            {
                Device.SendTelemetryData = true;
                buttonSend.Content = "Sending telemetry data";
            }
        }

        private void ButtonConnect_Click(object sender, RoutedEventArgs e)
        {
            if (Device.IsConnected)
            {
                Device.SendTelemetryData = false;
                if (Device.Disconnect())
                {
                    buttonSend.IsEnabled = false;
                    textDeviceId.IsEnabled = true;
                    textDeviceKey.IsEnabled = true;
                    textHostName.IsEnabled = true;
                    buttonConnect.Content = "Press to connect";
                }
            }
            else
            {
                if (Device.Connect())
                {
                    buttonSend.IsEnabled = true;
                    textDeviceId.IsEnabled = false;
                    textDeviceKey.IsEnabled = false;
                    textHostName.IsEnabled = false;
                    buttonConnect.Content = "Connected";

                }
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {


        }
    }
}
