using Shared;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace NavigationContainer
{
    public class NavigationContainer : _ControlBase
    {
        #region Private Fields
        private static Repository repository = new Repository();
        #endregion

        #region Properties
        private List<NavigationPane>? _ContainerItems;
        public List<NavigationPane> ContainerItems
        {
            get { return _ContainerItems; }
            set
            {
                if (_ContainerItems != value)
                {
                    _ContainerItems = value;
                }
            }
        }
        #endregion

        #region NavigationPanes DP
        public static readonly DependencyProperty NavigationPanesProperty =
                    DependencyProperty.Register("NavigationPanes",
                    typeof(List<NavigationPaneModel>),
                    typeof(NavigationContainer),
                    new PropertyMetadata(null, new PropertyChangedCallback(OnNavigationPanesChanged)));

        public List<NavigationPaneModel> NavigationPanes
        {
            get { return (List<NavigationPaneModel>)GetValue(NavigationPanesProperty); }
            set { SetValue(NavigationPanesProperty, value); }
        }

        private static async void OnNavigationPanesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (NavigationContainer)d;
            await control.Load();
        }
        #endregion

        #region CTOR
        static NavigationContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationContainer),
                new FrameworkPropertyMetadata(typeof(NavigationContainer)));
        }
        #endregion

        #region Private Methods
        private async Task Load()
        {
            if (NavigationPanes != null)
            {
                ContainerItems = new List<NavigationPane>();

                List<Task> tasks = new List<Task>(NavigationPanes.Count);
                foreach (var navigationPaneModel in NavigationPanes)
                {
                    tasks.Add(LoadPane(navigationPaneModel));
                }

                await Task.WhenAll(tasks);
            }
        }

        private async Task LoadPane(NavigationPaneModel navigationPaneModel)
        {
            // It's ok for data to be null here. There may be no data for that ItemType
            var data = await Task.Run(() => repository.GetNavigationItems(navigationPaneModel.NavigationItemType));

            ContainerItems.Add(new NavigationPane
            {
                Header = navigationPaneModel.Header ?? "",
                Items = new ObservableCollection<NavigationEntity>(data)
            });
        }
        #endregion
    }
}
