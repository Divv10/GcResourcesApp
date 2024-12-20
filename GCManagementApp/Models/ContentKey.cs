﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Models
{
    public class ContentKey : NotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _key;
        public string Key
        {
            get => _key;
            set => SetProperty(ref _key, value);
        }

        private List<string> _heroes;
        public List<string> Heroes
        {
            get => _heroes;
            set => SetProperty(ref _heroes, value);
        }

        private string _videoLink;
        public string VideoLink
        {
            get => _videoLink;
            set => SetProperty(ref _videoLink, value);
        }
    }
}
