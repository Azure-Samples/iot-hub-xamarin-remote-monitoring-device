using System;
using AzureIoTSuiteRemoteMonitoringHelper;
using XamNativeIoTSuiteDevice.Helpers;

namespace XamNativeIoTSuiteDevice
{
    public class MyClass : RemoteMonitoringDevice
    {
        public MyClass()
        {
            // Init the Device Model
            this.DeviceId = Settings.DeviceId;
            this.DeviceKey = Settings.DeviceKey;
            this.HostName = Settings.HostName;
        }
        public bool checkConfig()
        {
            if (((this.DeviceId != null) && (this.DeviceKey != null) && (this.HostName != null) &&
                        (this.DeviceId != "") && (this.DeviceKey != "") && (this.HostName != "")))
            {
                Settings.DeviceId = this.DeviceId;
                Settings.DeviceKey = this.DeviceKey;
                Settings.HostName = this.HostName;
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}

