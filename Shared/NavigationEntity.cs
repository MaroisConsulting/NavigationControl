using System.ComponentModel;

namespace Shared
{
    public class NavigationEntity : _EntityBase
    {
        private string? _Caption;
        public string Caption
        {
            get { return _Caption; }
            set
            {
                if (_Caption != value)
                {
                    _Caption = value;
                    RaisePropertyChanged(nameof(Caption));
                }
            }
        }

        private NavigationItemType _ItemType;
        public NavigationItemType ItemType
        {
            get { return _ItemType; }
            set
            {
                if (_ItemType != value)
                {
                    _ItemType = value;
                    RaisePropertyChanged(nameof(ItemType));
                }
            }
        }

        private List<NavigationEntity>? _Children;
        public List<NavigationEntity> Children
        {
            get { return _Children; }
            set
            {
                if (_Children != value)
                {
                    _Children = value;
                    RaisePropertyChanged(nameof(Children));
                }
            }
        }

        public NavigationEntity()
        {
            Children = new List<NavigationEntity>();
        }
    }
}
