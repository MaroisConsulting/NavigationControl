using CommunityToolkit.Mvvm.Input;
using Shared;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace NavigationContainer
{
    public class NavigationContainer : _ControlBase
    {
        #region Commands
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

        #region Routed Events
        #region NavigationItemSelectedEvent
        public static readonly RoutedEvent NavigationItemSelectedEvent =
                    EventManager.RegisterRoutedEvent("NavigationItemSelected",
                    RoutingStrategy.Tunnel,
                    typeof(RoutedEventHandler),
                    typeof(NavigationContainer));


        public event RoutedEventHandler NavigationItemSelected
        {
            add { AddHandler(NavigationItemSelectedEvent, value); }
            remove { RemoveHandler(NavigationItemSelectedEvent, value); }
        }

        private void RaiseNavigationItemSelectedEvent(NavigationEntity entity)
        {
            var args = new NavigationEventArgs(NavigationItemSelectedEvent, entity);
            RaiseEvent(args);
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
                        NavigationPaneModel = navigationPaneModel
                        
                    };
                    ContainerItems.Add(navigationPane);
                }
            }
        }

        private bool NavigationItemSelectedCanExecute(NavigationEventArgs args)
        {
            return true;
        }

        private void NavigationItemSelectedExecuted(NavigationEventArgs args)
        {
            RaiseNavigationItemSelectedEvent(args.NavigationEntity);
        }
        #endregion
    }
}
