using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GCManagementApp.Models
{
    [Serializable]
    public class BlueStackProcess : NotifyPropertyChanged
    {
        private DnsEndPoint _instanceIpAddress;
        [JsonIgnore]
        public DnsEndPoint InstanceIpAddress
        {
            get => _instanceIpAddress; 
            set => SetProperty(ref _instanceIpAddress, value);
        }

        private string _host;
        public string Host
        {
            get => _host;
            set => SetProperty(ref _host, value);
        }

        private int _port;
        public int Port
        {
            get => _port; 
            set => SetProperty(ref _port, value);
        }
    }
}
