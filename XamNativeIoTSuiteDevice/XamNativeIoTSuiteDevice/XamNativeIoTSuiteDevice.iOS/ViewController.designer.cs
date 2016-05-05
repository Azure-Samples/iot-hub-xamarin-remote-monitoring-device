// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace XamNativeIoTSuiteDevice.iOS
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton buttonConnect { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton buttonSend { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISlider sliderHumidity { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISlider sliderTemperature { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField textDeviceId { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField textDeviceKey { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField textHostName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel textHumidity { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel textTemperature { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (buttonConnect != null) {
                buttonConnect.Dispose ();
                buttonConnect = null;
            }

            if (buttonSend != null) {
                buttonSend.Dispose ();
                buttonSend = null;
            }

            if (sliderHumidity != null) {
                sliderHumidity.Dispose ();
                sliderHumidity = null;
            }

            if (sliderTemperature != null) {
                sliderTemperature.Dispose ();
                sliderTemperature = null;
            }

            if (textDeviceId != null) {
                textDeviceId.Dispose ();
                textDeviceId = null;
            }

            if (textDeviceKey != null) {
                textDeviceKey.Dispose ();
                textDeviceKey = null;
            }

            if (textHostName != null) {
                textHostName.Dispose ();
                textHostName = null;
            }

            if (textHumidity != null) {
                textHumidity.Dispose ();
                textHumidity = null;
            }

            if (textTemperature != null) {
                textTemperature.Dispose ();
                textTemperature = null;
            }
        }
    }
}