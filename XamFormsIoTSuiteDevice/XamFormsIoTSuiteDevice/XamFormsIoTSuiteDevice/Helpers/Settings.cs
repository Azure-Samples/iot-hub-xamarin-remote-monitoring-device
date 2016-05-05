// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace XamFormsIoTSuiteDevice.Helpers
{
  /// <summary>
  /// This is the Settings static class that can be used in your Core solution or in any
  /// of your client applications. All settings are laid out the same exact way with getters
  /// and setters. 
  /// </summary>
  public static class Settings
  {
    private static ISettings AppSettings
    {
      get
      {
        return CrossSettings.Current;
      }
    }

        #region Setting Constants

        private const string DeviceIdKey = "DeviceId_key";
        private static readonly string DeviceIdDefault = string.Empty;

        private const string DeviceKeyKey = "DeviceKey_key";
        private static readonly string DeviceKeyDefault = string.Empty;

        private const string HostNameKey = "HostName_key";
        private static readonly string HostNameDefault = string.Empty;

        #endregion


        public static string DeviceId
        {
            get { return AppSettings.GetValueOrDefault<string>(DeviceIdKey, DeviceIdDefault); }
            set { AppSettings.AddOrUpdateValue<string>(DeviceIdKey, value); }
        }
        public static string DeviceKey
        {
            get { return AppSettings.GetValueOrDefault<string>(DeviceKeyKey, DeviceKeyDefault); }
            set { AppSettings.AddOrUpdateValue<string>(DeviceKeyKey, value); }
        }
        public static string HostName
        {
            get { return AppSettings.GetValueOrDefault<string>(HostNameKey, HostNameDefault); }
            set { AppSettings.AddOrUpdateValue<string>(HostNameKey, value); }
        }
    }
}