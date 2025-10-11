using GCManagementApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Models
{
    public class HeroBuild : NotifyPropertyChanged
    {
        private List<GearSet> _sets;
        public List<GearSet> Sets
        {
            get => _sets; 
            set => SetProperty(ref _sets, value);
        }

        private Dictionary<ChaserTraitEnum, int> _csTraits;
        public Dictionary<ChaserTraitEnum, int> CsTraits
        {
            get => _csTraits; 
            set => SetProperty(ref _csTraits, value);
        }

        private Dictionary<LevelTraitEnum, int> _levelTraits;
        public Dictionary<LevelTraitEnum, int> LvlTraits
        {
            get => _levelTraits;
            set => SetProperty(ref _levelTraits, value);
        }

        private string[] _accessories;
        public string[] Accessories
        {
            get => _accessories;
            set => SetProperty(ref _accessories, value);
        }
        
        private AccessorySetEnum _accColor;
        public AccessorySetEnum AccessoryColor
        {
            get => _accColor;
            set => SetProperty(ref _accColor, value);
        }

        private string _artifact;
        public string Artifact
        {
            get => _artifact;
            set => SetProperty(ref _artifact, value);
        }

        private ArtifactType _artifactType;
        public ArtifactType ArtifactType
        {
            get => _artifactType;
            set => SetProperty(ref _artifactType, value);
        }

        private string[] _siTypes;
        public string[] SiTypes
        {
            get => _siTypes;
            set => SetProperty(ref _siTypes, value);
        }

        private List<List<int>> _siTraitList;
        public List<List<int>> SiTraitList
        {
            get => _siTraitList;
            set => SetProperty(ref _siTraitList, value);
        }
    }
}
