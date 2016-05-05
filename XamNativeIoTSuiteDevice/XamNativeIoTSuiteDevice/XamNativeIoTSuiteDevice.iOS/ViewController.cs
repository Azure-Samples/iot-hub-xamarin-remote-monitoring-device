using AzureIoTSuiteRemoteMonitoringHelper;
using System;

using UIKit;

namespace XamNativeIoTSuiteDevice.iOS
{
	public partial class ViewController : UIViewController
	{
        MyClass Device;

        public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

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
            buttonConnect.AccessibilityIdentifier = "buttonConnect";
            buttonConnect.Enabled = false;
            buttonConnect.SetTitle("Press to connect", UIControlState.Normal);
            buttonConnect.TouchUpInside += ButtonConnect_TouchUpInside;

            buttonSend.AccessibilityIdentifier = "buttonSend";
            buttonSend.Enabled = false;
            buttonSend.SetTitle("Press to send telemetry data", UIControlState.Normal);
            buttonSend.TouchUpInside += ButtonSend_TouchUpInside;

            textDeviceId.AccessibilityIdentifier = "textDeviceId";
            textDeviceId.EditingChanged += TextDeviceId_EditingChanged;
            textDeviceId.Text = Device.DeviceId;

            textHostName.AccessibilityIdentifier = "textHostName";
            textHostName.EditingChanged += TextHostName_EditingChanged; ;
            textHostName.Text = Device.HostName;

            textDeviceKey.AccessibilityIdentifier = "textDeviceKey";
            textDeviceKey.EditingChanged += TextDeviceKey_EditingChanged;
            textDeviceKey.Text = Device.DeviceKey;

            sliderTemperature.AccessibilityIdentifier = "sliderTemperature";
            sliderTemperature.ValueChanged += SliderTemperature_ValueChanged;
            sliderTemperature.MinValue = 0;
            sliderTemperature.MaxValue = 100;
            sliderTemperature.Value = 50;

            sliderHumidity.AccessibilityIdentifier = "sliderHumidity";
            sliderHumidity.ValueChanged += SliderHumidity_ValueChanged;
            sliderHumidity.MinValue = 0;
            sliderHumidity.MaxValue = 100;
            sliderHumidity.Value = 50;

            // Check configuration and enable connect button if it looks ok
            buttonConnect.Enabled = Device.checkConfig();

        }

        private void SliderTemperature_ValueChanged(object sender, EventArgs e)
        {
            textTemperature.Text = "Temperature: " + sliderTemperature.Value; ;
            Device.UpdateTelemetryData("Temperature", sliderTemperature.Value);
        }
        private void SliderHumidity_ValueChanged(object sender, EventArgs e)
        {
            textHumidity.Text = "Humidity: " + sliderHumidity.Value; ;
            Device.UpdateTelemetryData("Humidity", sliderHumidity.Value);
        }

        private void TextHostName_EditingChanged(object sender, EventArgs e)
        {
            Device.HostName = textHostName.Text.ToString();
            buttonConnect.Enabled = Device.checkConfig();
        }

        private void TextDeviceId_EditingChanged(object sender, EventArgs e)
        {
            Device.DeviceId = textDeviceId.Text.ToString();
            buttonConnect.Enabled = Device.checkConfig();
        }

        private void TextDeviceKey_EditingChanged(object sender, EventArgs e)
        {
            Device.DeviceKey = textDeviceKey.Text.ToString();
            buttonConnect.Enabled = Device.checkConfig();
        }

        private void ButtonSend_TouchUpInside(object sender, EventArgs e)
        {
            if (Device.SendTelemetryData)
            {

                Device.SendTelemetryData = false;
                buttonSend.SetTitle("Press to send telemetry data", UIControlState.Normal);
            }
            else
            {
                Device.SendTelemetryData = true;
                buttonSend.SetTitle("Sending telemetry data", UIControlState.Normal);
            }
        }

        private void ButtonConnect_TouchUpInside(object sender, EventArgs e)
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
                    buttonConnect.SetTitle("Press to connect", UIControlState.Normal);
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
                    buttonConnect.SetTitle("Connected", UIControlState.Normal);

                }
            }
        }

        public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

