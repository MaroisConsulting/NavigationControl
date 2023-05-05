using Shared;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace NavigationContainer
{
    public class NavigationPane : _ControlBase
    {
        #region DP's
        #region DP Header
        public static readonly DependencyProperty HeaderProperty =
                    DependencyProperty.Register("Header",
                    typeof(string),
                    typeof(NavigationPane),
                    new PropertyMetadata("", new PropertyChangedCallback(OnHeaderChanged)));

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //var control = (NavigationContainer)d;
        }
        #endregion

        #region DP ItemType
        public static readonly DependencyProperty ItemTypeProperty =
                    DependencyProperty.Register("ItemType",
                    typeof(NavigationItemType?),
                    typeof(NavigationContainer),
                    new PropertyMetadata(null, new PropertyChangedCallback(OnItemTypeChanged)));

        public NavigationItemType? ItemType
        {
            get { return (NavigationItemType?)GetValue(ItemTypeProperty); }
            set { SetValue(ItemTypeProperty, value); }
        }


        private static void OnItemTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //var control = (NavigationContainer)d;
        }
        #endregion

        #region DP Items
        public static readonly DependencyProperty ItemsProperty =
                    DependencyProperty.Register("Items",
                    typeof(ObservableCollection<NavigationEntity>),
                    typeof(NavigationPane),
                    new PropertyMetadata(null, new PropertyChangedCallback(OnItemsChanged)));

        public ObservableCollection<NavigationEntity> Items
        {
            get { return (ObservableCollection<NavigationEntity>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        private static void OnItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (NavigationPane)d;
        }
        #endregion

        #region DP NavigationPaneModel
        public static readonly DependencyProperty NavigationPaneModelProperty =
                    DependencyProperty.Register("NavigationPaneModel",
                    typeof(NavigationPaneModel),
                    typeof(NavigationPane),
                    new PropertyMetadata(null, new PropertyChangedCallback(OnNavigationPaneModelChanged)));

        public NavigationPaneModel NavigationPaneModel
        {
            get { return (NavigationPaneModel)GetValue(NavigationPaneModelProperty); }
            set { SetValue(NavigationPaneModelProperty, value); }
        }
        private static async void OnNavigationPaneModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (NavigationPane)d;

            if (control.NavigationPaneModel.IsExpanded)
            {
                control.IsPaneExpanded = true;
            }
        }
        #endregion

        #region DP IsPaneExpanded
        public static readonly DependencyProperty IsPaneExpandedProperty =
                    DependencyProperty.Register("IsPaneExpanded",
                    typeof(bool),
                    typeof(NavigationPane),
                    new PropertyMetadata(false, new PropertyChangedCallback(OnIsPaneExpandedChanged)));

        public bool IsPaneExpanded
        {
            get { return (bool)GetValue(IsPaneExpandedProperty); }
            set { SetValue(IsPaneExpandedProperty, value); }
        }

        private static async void OnIsPaneExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (NavigationPane)d;

            // This is getting called, but...
            if (control.IsPaneExpanded)
            {
                if (control.NavigationPaneModel != null)
                {
                    await control.Load();
                }
            }
        }
        #endregion
        #endregion

        #region CTOR
        static NavigationPane()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationPane),
                new FrameworkPropertyMetadata(typeof(NavigationPane)));
        }
        #endregion

        #region Private Methods
        private async Task Load()
        {
            if (NavigationPaneModel != null && NavigationPaneModel.DataSource != null)
            {
                var dataSource = NavigationPaneModel.DataSource();

                List<NavigationEntity>? data = null;

                if (dataSource != null)
                {
                    data = await Task.Run(() => dataSource);
                }

                if (data != null)
                {
                    Items = new ObservableCollection<NavigationEntity>(data);
                }
            }
        }
        #endregion
    }
}
