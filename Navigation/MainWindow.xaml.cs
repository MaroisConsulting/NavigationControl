using CommunityToolkit.Mvvm.Input;
using NavigationContainer;
using Shared;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

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

        private ICommand? _NavigationItemSelectedCommand;
        public ICommand NavigationItemSelectedCommand
        {
            get
            {
                if (_NavigationItemSelectedCommand == null)
                    _NavigationItemSelectedCommand = new RelayCommand<NavigationEventArgs>(p => NavigationItemSelectedExecuted(p), p => NavigationItemSelectedCanExecute(p));
                return _NavigationItemSelectedCommand;
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
                    DataSource = Functional.Apply(Repository.GetNavigationItems, NavigationItemType.Project),
                    IsExpanded = true
                },

                new NavigationPaneModel
                {
                    Header = "Inventory", 
                    NavigationItemType = NavigationItemType.Inventory,
                    DataSource = Functional.Apply(Repository.GetNavigationItems, NavigationItemType.Inventory),
                },

                new NavigationPaneModel
                {
                    Header = "Companies" , 
                    NavigationItemType = NavigationItemType.Company,
                    DataSource = Functional.Apply(Repository.GetNavigationItems, NavigationItemType.Company),
                },

                new NavigationPaneModel
                {
                    Header = "Employees", 
                    NavigationItemType = NavigationItemType.Employee,
                    DataSource = Functional.Apply(Repository.GetNavigationItems, NavigationItemType.Employee),
                }
            };
        }

        private bool NavigationItemSelectedCanExecute(NavigationEventArgs args)
        {
            return true;
        }

        private void NavigationItemSelectedExecuted(NavigationEventArgs args)
        {
            var message = $"{args.NavigationEntity.ItemType} {args.NavigationEntity.Id} selected";
            MessageBox.Show(message, "Item Selected", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        protected void RaisePropertyChanged(string properyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(properyName));
        }
    }
}
