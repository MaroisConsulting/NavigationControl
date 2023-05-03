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
        //private static Repository repository = new Repository();
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

        private static void OnNavigationPanesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (NavigationContainer)d;
            control.Load();
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
        private void Load()
        {
            if (NavigationPanes != null)
            {
                ContainerItems = new List<NavigationPane>();

                foreach (var navigationPaneModel in NavigationPanes)
                {
                    var navigationPane = new NavigationPane
                    { 
                        Header = navigationPaneModel.Header ?? "",
                        ItemType = navigationPaneModel.NavigationItemType,
                        NavigationPaneModel = navigationPaneModel
                        
                    };
                    ContainerItems.Add(navigationPane);

                    //navigationPane.IsExpanded = navigationPaneModel.IsExpanded;
                }
            }
        }
        #endregion
    }
}
