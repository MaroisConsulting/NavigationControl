using Shared;
using System.Collections.ObjectModel;
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
                    new PropertyMetadata(""));

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        #endregion

        #region DP ItemType
        public static readonly DependencyProperty ItemTypeProperty =
                    DependencyProperty.Register("ItemType",
                    typeof(NavigationItemType?),
                    typeof(NavigationPane),
                    new PropertyMetadata(null, new PropertyChangedCallback(OnItemTypeChanged)));

        public NavigationItemType? ItemType
        {
            get { return (NavigationItemType?)GetValue(ItemTypeProperty); }
            set { SetValue(ItemTypeProperty, value); }
        }

        private static void OnItemTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NavigationPane control = (NavigationPane)d;
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
            control.BusyIndicatorVisibility = Visibility.Collapsed;
        }
        #endregion

        #region DP BusyIndicatorVisibility
        public static readonly DependencyProperty BusyIndicatorVisibilityProperty =
                    DependencyProperty.Register("BusyIndicatorVisibility",
                    typeof(Visibility),
                    typeof(NavigationPane),
                    new PropertyMetadata(Visibility.Visible, new PropertyChangedCallback(OnBusyIndicatorVisibilityChanged)));

        public Visibility BusyIndicatorVisibility
        {
            get { return (Visibility)GetValue(BusyIndicatorVisibilityProperty); }
            set { SetValue(BusyIndicatorVisibilityProperty, value); }
        }

        private static void OnBusyIndicatorVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //var control = (NavigationPane)d;
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
    }
}
