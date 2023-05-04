using Shared;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Navigation
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private List<NavigationPaneModel>? _NavigationPaneInfos;
        public List<NavigationPaneModel> NavigationPaneInfos
        {
            get { return _NavigationPaneInfos; }
            set
            {
                if (_NavigationPaneInfos != value)
                {
                    _NavigationPaneInfos = value;
                    RaisePropertyChanged(nameof(NavigationPaneInfos));
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;

            NavigationPaneInfos = new List<NavigationPaneModel>
            {
                new NavigationPaneModel
                {
                    Header = "Projects", 
                    NavigationItemType = NavigationItemType.Project, 
                    DataSource = Repository.GetNavigationItems,
                    IsExpanded = true //<=== THIS TRIGGERS LOAD
                },

                new NavigationPaneModel
                {
                    Header = "Inventory", 
                    NavigationItemType = NavigationItemType.Inventory,
                    DataSource = Repository.GetNavigationItems
                },

                new NavigationPaneModel
                {
                    Header = "Companies" , 
                    NavigationItemType = NavigationItemType.Company,
                    DataSource = Repository.GetNavigationItems,
                    IsExpanded = true //<=== THIS TRIGGERS LOAD
                },

                new NavigationPaneModel
                {
                    Header = "Employees", 
                    NavigationItemType = NavigationItemType.Employee,
                    DataSource = Repository.GetNavigationItems
                }
            };
        }

        protected void RaisePropertyChanged(string properyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(properyName));
        }
    }
}
