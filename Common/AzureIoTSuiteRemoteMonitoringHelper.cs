using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AzureIoTSuiteRemoteMonitoringHelper
{
    #region Data Contracts for serializing data
    [DataContract]
    public class DeviceProperties
    {
        [DataMember]
        internal string DeviceID;

        [DataMember]
        internal bool HubEnabledState = true;

        [DataMember(EmitDefaultValue =false)]
        internal string CreatedTime;

        [DataMember]
        internal string DeviceState = "normal";

        [DataMember(EmitDefaultValue = false)]
        internal string UpdatedTime;

        [DataMember(EmitDefaultValue = false)]
        internal string Manufacturer;

        [DataMember(EmitDefaultValue = false)]
        internal string ModelNumber;

        [DataMember(EmitDefaultValue = false)]
        internal string SerialNumber;

        [DataMember(EmitDefaultValue = false)]
        internal string FirmwareVersion;

        [DataMember(EmitDefaultValue = false)]
        internal string Platform;

        [DataMember(EmitDefaultValue = false)]
        internal string Processor;

        [DataMember(EmitDefaultValue = false)]
        internal string InstalledRAM;

        [DataMember]
        internal double Latitude= 47.6603;

        [DataMember]
        internal double Longitude= -122.1405;

    }

    [DataContract]
    public class CommandParameter
    {
        [DataMember]
        internal string Name;

        [DataMember]
        internal string Type;
    }

    [DataContract]
    public class Command
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        internal Collection<CommandParameter> Parameters = new Collection<CommandParameter>();
    }

    [DataContract]
    public class ReceivedMessage
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string MessageId { get; set; }
        [DataMember]
        public string CreatedTime { get; set; }
        [DataMember]
        public Dictionary<string, object> Parameters { get; set; }
    }


    [DataContract]
    public class TelemetryFormat
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public string Type { get; set; }
    }

    [DataContract]
    public class DeviceModel
    {
        [DataMember]
        public DeviceProperties DeviceProperties { get; set; } = new DeviceProperties();

        [DataMember]
        internal Collection<Command> Commands = new Collection<Command>();

        [DataMember]
        internal Collection<TelemetryFormat> Telemetry = new Collection<TelemetryFormat>();

        [DataMember]
        internal bool IsSimulatedDevice = false;

        [DataMember]
        internal string Version = "1.0";

        [DataMember]
        internal string ObjectType = "DeviceInfo";
    }
    #endregion


    /// <summary>
    /// ReceivedMessageEventArgs class
    /// Class to pass event arguments for new message received from the IoT Suite dashboard
    /// </summary>
    public class ReceivedMessageEventArgs : System.EventArgs
    {
        public ReceivedMessage Message { get; set; }

        public ReceivedMessageEventArgs(ReceivedMessage message)
        {
            Message = message;
        }
    }

    /// <summary>
    /// RemoteMonitoringDevice class
    /// Provides helper functions for easily connecting a device to Azure IoT Suite Remote Monitoring 
    /// </summary>
    public class RemoteMonitoringDevice
    {
        // Azure IoT Hub client
        private DeviceClient deviceClient;

        // Device Model values
        public DeviceModel Model { get; set; } = new DeviceModel();

        // Collection of telemetry data
        public Dictionary<string, object> Telemetry { get; set; } = new Dictionary<string, object>();
        public void AddTelemetry(TelemetryFormat TelemetryItem, object DefaultValue)
        {
            if (!Telemetry.ContainsKey(TelemetryItem.Name))
            {
                Telemetry.Add(TelemetryItem.Name, DefaultValue);
                Model.Telemetry.Add(TelemetryItem);
            }
        }

        // Collection of commands
        public Dictionary<string, object> Commands { get; set; } = new Dictionary<string, object>();
        public void AddCommand(Command CommandItem)
        {
            if (!Commands.ContainsKey(CommandItem.Name))
            {
                Commands.Add(CommandItem.Name, CommandItem);
                Model.Commands.Add(CommandItem);
            }
        }

        // Connection settings (Device Id, Device Key and HostName) used to establish connection with IoT Hub instance
        private string _DeviceId;
        public string DeviceId
        {
            get { return _DeviceId; }
            set
            {
                _DeviceId = value;
                if (!Telemetry.ContainsKey("DeviceId")) Telemetry.Add("DeviceId", value);
                else Telemetry["DeviceId"] = value;
                Model.DeviceProperties.DeviceID = value;
            }
        }
        public string DeviceKey { get; set; }
        public string HostName { get; set; }

        // Users can decide when to start/stop sending telemetry data and at what frequency
        public bool SendTelemetryData { get; set; }
        public int SendTelemetryFreq { get; set; } = 1000;
        public bool IsConnected { get; set; } = false;

        // Sending and receiving tasks
        CancellationTokenSource TokenSource = new CancellationTokenSource();

        // Event Handler for notifying the reception of a new message from IoT Hub
        public event EventHandler onReceivedMessage;


        // Trigger for notifying reception of new message from IoT Suite dashboard
        protected virtual void OnReceivedMessage(ReceivedMessageEventArgs e)
        {
            if (onReceivedMessage != null)
                onReceivedMessage(this, e);
        }

        /// <summary>
        /// Serialize message
        /// </summary>
        private byte[] Serialize(object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            return Encoding.UTF8.GetBytes(json);

        }

        /// <summary>
        /// DeSerialize message
        /// </summary>
        private JObject DeSerialize(byte[] data)
        {
            string text = Encoding.UTF8.GetString(data, 0, data.Length);
            return JObject.Parse(text);
        }

        /// <summary>
        /// Send device's telemetry data to Azure IoT Hub
        /// </summary>
        public async void sendData(object data)
        {
            try
            {
                var msg = new Message(Serialize(data));
                if (deviceClient != null)
                {
                    await deviceClient.SendEventAsync(msg);
                    Debug.WriteLine("Sent message to IoT Hub:\n" + JsonConvert.SerializeObject(data));
                }
                else Debug.WriteLine("Connection To IoT Hub is not established. Cannot send message now");
                
            }
            catch (System.Exception e)
            {
                Debug.WriteLine("Exception while sending message to IoT Hub:\n" + e.Message.ToString());
            }
        }

        /// <summary>
        /// Create the Connection String out of the device credentials
        /// </summary>
        /// <returns></returns>
        private string ConnectionString()
        {
            return "HostName=" + this.HostName + ";DeviceId=" + this.DeviceId + ";SharedAccessKey=" + this.DeviceKey;
        }

        /// <summary>
        /// Update Telemetry data
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        public void UpdateTelemetryData(string Name, double Value)
        {
            if (this.Telemetry.ContainsKey(Name))
                this.Telemetry[Name] = Value;
        }
        /// <summary>
        /// Connect
        /// Connect to Azure IoT Hub ans start the send and receive loops
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            try
            {
                // Create Azure IoT Hub Client and open messaging channel
                deviceClient = DeviceClient.CreateFromConnectionString(ConnectionString(), TransportType.Http1);
                deviceClient.OpenAsync();
                IsConnected = true;
                
                // Send Device Meta Data to IoT Suite
                sendData(Model);

                CancellationToken ct = TokenSource.Token;
                // Create send task
                Task.Factory.StartNew(async()=> {
                    while (true)
                    {
                        if (SendTelemetryData)
                        {
                             // Send current telemetry data
                            sendData(this.Telemetry);
                        }
                        await Task.Delay(SendTelemetryFreq);

                        if (ct.IsCancellationRequested)
                        {
                            // Cancel was called
                            Debug.WriteLine("Sending task canceled");
                            break;
                        }

                    }
                }, ct);

                // Create receive task
                Task.Factory.StartNew(async() =>
                {
                    while (true)
                    {
                        // Receive message from Cloud (for now this is a pull because only HTTP is available for UWP applications)
                        Message message = await deviceClient.ReceiveAsync();
                        if (message != null)
                        {
                            try
                            {
                                // Read message and deserialize
                                var obj = DeSerialize(message.GetBytes());

                                ReceivedMessage command = new ReceivedMessage();
                                command.Name = obj["Name"].ToString();
                                command.MessageId = obj["MessageId"].ToString();
                                command.CreatedTime = obj["CreatedTime"].ToString();
                                command.Parameters = new Dictionary<string, object>();
                                foreach (dynamic param in obj["Parameters"])
                                {
                                    command.Parameters.Add(param.Name, param.Value);
                                }

                                // Invoke message received callback
                                OnReceivedMessage(new ReceivedMessageEventArgs(command));

                                // We received the message, indicate IoTHub we treated it
                                await deviceClient.CompleteAsync(message);
                            }
                            catch (Exception e)
                            {
                                // Something went wrong. Indicate the backend that we coudn't accept the message
                                Debug.WriteLine("Error while deserializing message received: " + e.Message);
                                await deviceClient.RejectAsync(message);
                            }
                        }
                        if (ct.IsCancellationRequested)
                        {
                            // Cancel was called
                            Debug.WriteLine("Receiving task canceled");
                            break;
                        }
                    }
                }, ct);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error while trying to connect to IoT Hub:" + e.Message.ToString());
                deviceClient = null;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Disconnect
        /// Disconnect from IoT Hub
        /// </summary>
        /// <returns></returns>
        public bool Disconnect()
        {
            if (deviceClient != null)
            {
                try
                {
                    deviceClient.CloseAsync();
                    deviceClient = null;
                    IsConnected = false;
                }
                catch
                {
                    Debug.WriteLine("Error while trying close the IoT Hub connection");
                    return false;
                }
            }
            return true;
        }
    }
}
