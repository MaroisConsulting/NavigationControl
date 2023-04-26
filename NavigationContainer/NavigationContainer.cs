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

        #region DP's
        #region DP ContainerItems
        public static readonly DependencyProperty ContainerItemsProperty =
                    DependencyProperty.Register("ContainerItems",
                    typeof(List<NavigationPane>),
                    typeof(NavigationContainer),
                    new PropertyMetadata(null, new PropertyChangedCallback(OnContainerItemsChanged)));

        public List<NavigationPane>? ContainerItems
        {
            get { return (List<NavigationPane>?)GetValue(ContainerItemsProperty); }
            set { SetValue(ContainerItemsProperty, value); }
        }

        private static void OnContainerItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //var control = (NavigationContainer)d;
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
                var items = new List<NavigationPane>();

                List<Task> tasks = new List<Task>(NavigationPanes.Count);
                foreach (var navigationPaneModel in NavigationPanes)
                {
                    tasks.Add(LoadPane(navigationPaneModel, items));
                }

                await Task.WhenAll(tasks);

                if (items != null)
                {
                    ContainerItems =  new List<NavigationPane>(items);
                }
            }
        }

        private async Task LoadPane(NavigationPaneModel navigationPaneModel, List<NavigationPane> containerItems)
        {
            // It's ok for data to be null here. There may be no data for that ItemType
            var data = await Task.Run(() => repository.GetNavigationItems(navigationPaneModel.NavigationItemType));

            containerItems.Add(new NavigationPane
            {
                Header = navigationPaneModel.Header ?? "",
                Items = new ObservableCollection<NavigationEntity>(data)
            });
        }
        #endregion
    }
}
