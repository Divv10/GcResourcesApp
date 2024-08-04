using GCManagementApp.Enums;
using System;
using System.Collections.Generic;

namespace GCManagementApp.Models
{
    [Serializable]
    public class SerializableContentTeam : NotifyPropertyChanged
    {
        private List<Hero> _heroes;
        public List<Hero> Heroes
        {
            get => _heroes;
            set => SetProperty(ref _heroes, value);
        }

        private ContentAttributeEnum _element;
        public ContentAttributeEnum Element
        {
            get => _element;
            set => SetProperty(ref _element, value);
        }
    }
}
