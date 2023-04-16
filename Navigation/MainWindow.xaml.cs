using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Navigation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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
                new NavigationPaneModel {Header = "Projects", NavigationItemType = NavigationItemType.Project},
                new NavigationPaneModel {Header = "Inventory", NavigationItemType = NavigationItemType.Inventory},
                new NavigationPaneModel {Header = "Companies" , NavigationItemType = NavigationItemType.Company},
                new NavigationPaneModel {Header = "Employees", NavigationItemType = NavigationItemType.Employee},
            };
        }

        protected void RaisePropertyChanged(string properyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(properyName));
        }
    }
}
