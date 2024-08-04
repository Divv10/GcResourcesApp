using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Models
{
    public class ProfileFile : NotifyPropertyChanged
    {
        private Guid _id;
        public Guid Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private bool _isDefault;
        public bool IsDefault
        {
            get => _isDefault; 
            set => SetProperty(ref _isDefault, value);
        }

        private string _path;
        public string Path
        {
            get => _path; 
            set => SetProperty(ref _path, value);
        }
    }
}
