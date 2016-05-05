using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using AzureIoTSuiteRemoteMonitoringHelper;

namespace XamNativeIoTSuiteDevice.Droid
{
	[Activity (Label = "XamNativeIoTSuiteDevice.Droid", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
        MyClass Device;

        Button buttonConnect;
        Button buttonSend;

        TextView textDeviceId;
        TextView textDeviceKey;
        TextView textHostName;

        TextView textTemperature;
        TextView textHumidity;

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

            // Initialize IoT Hub client
            Device = new MyClass();

            // Add the telemetry data
            Device.AddTelemetry(new TelemetryFormat { Name = "Temperature", DisplayName = "Temp (C)", Type = "Double" }, (double)0);
            Device.AddTelemetry(new TelemetryFormat { Name = "Humidity", DisplayName = "Hmdt (%)", Type = "Double" }, (double)0);


            // If you are developing and want to avoid having to enter the full connection string on the device,
            // you can temporarily hard code it here. Comment this when done!
            //Device.DeviceId = "[DeviceId]";
            //Device.HostName = "[HostName]";
            //Device.DeviceKey = "[DeviceKey]";

            // Prepare UI elements
            buttonConnect = FindViewById<Button>(Resource.Id.buttonConnect);
            buttonConnect.Enabled = false;
            buttonConnect.Text = "Press to Connect";
            buttonConnect.Click += ButtonConnect_Click;

            buttonSend = FindViewById<Button>(Resource.Id.buttonSend);
            buttonSend.Enabled = false;
            buttonSend.Text = "Press to send telemetry data";
            buttonSend.Click += ButtonSend_Click;

            textDeviceId = FindViewById<TextView>(Resource.Id.textDeviceId);
            textDeviceId.TextChanged += TextDeviceId_TextChanged;
            textDeviceId.Text = Device.DeviceId;

            textDeviceKey = FindViewById<TextView>(Resource.Id.textDeviceKey);
            textDeviceKey.TextChanged += TextDeviceKey_TextChanged;
            textDeviceKey.Text = Device.DeviceKey;

            textHostName = FindViewById<TextView>(Resource.Id.textHostName);
            textHostName.TextChanged += TextHostName_TextChanged;
            textHostName.Text = Device.HostName;

            textTemperature = FindViewById<TextView>(Resource.Id.textTemperature);
            SeekBar seekBarTemperature = FindViewById<SeekBar>(Resource.Id.seekBarTemperature);
            seekBarTemperature.ProgressChanged += SeekBarTemperature_ProgressChanged;
            seekBarTemperature.Progress = 50;

            textHumidity = FindViewById<TextView>(Resource.Id.textHumidity);
            SeekBar seekBarHumidity = FindViewById<SeekBar>(Resource.Id.seekBarHumidity);
            seekBarHumidity.ProgressChanged += SeekBarHumidity_ProgressChanged;
            seekBarHumidity.Progress = 50;

            // Set focus to the connect button
            buttonConnect.RequestFocus();
            buttonConnect.Enabled = Device.checkConfig();
        }
        private void SeekBarHumidity_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            textHumidity.Text = "Humidity: " + e.Progress;
            Device.UpdateTelemetryData("Humidity", e.Progress);
        }

        private void SeekBarTemperature_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            textTemperature.Text = "Temperature: " + e.Progress;
            Device.UpdateTelemetryData("Temperature", e.Progress);
        }

        private void ButtonSend_Click(object sender, EventArgs e)
        {
            if (Device.SendTelemetryData)
            {

                Device.SendTelemetryData = false;
                buttonSend.Text = "Press to send telemetry data";
            }
            else
            {
                Device.SendTelemetryData = true;
                buttonSend.Text = "Sending telemetry data";
            }
        }

        private void ButtonConnect_Click(object sender, EventArgs e)
        {
            if (Device.IsConnected)
            {
                Device.SendTelemetryData = false;
                if (Device.Disconnect())
                {
                    buttonSend.Enabled = false;
                    textDeviceId.Enabled = true;
                    textDeviceKey.Enabled = true;
                    textHostName.Enabled = true;
                    buttonConnect.Text = "Press to connect";
                }
            }
            else
            {
                if (Device.Connect())
                {
                    buttonSend.Enabled = true;
                    textDeviceId.Enabled = false;
                    textDeviceKey.Enabled = false;
                    textHostName.Enabled = false;
                    buttonConnect.Text = "Connected";

                }
            }

        }

        private void TextDeviceId_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            Device.DeviceId = e.Text.ToString();
            buttonConnect.Enabled = Device.checkConfig();
        }

        private void TextDeviceKey_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            Device.DeviceKey = e.Text.ToString();
            buttonConnect.Enabled = Device.checkConfig();
        }

        private void TextHostName_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            Device.HostName = e.Text.ToString();
            buttonConnect.Enabled = Device.checkConfig();
        }
    }
}


