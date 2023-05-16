using CommunityToolkit.Mvvm.Input;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace NavigationContainer
{
    public class NavigationPane : _ControlBase
    {
        #region Private Fields
        // Stores the value of the DP IsPaneExpanded during initialization. See
        // comments in the DP IsPaneExpanded 
        private bool _isPaneExpanded = false;

        // Set to true after the control has finished initialising
        private bool _isInitialized = false;
        #endregion

        #region Commands
        private ICommand? _ItemClickedCommand;
        public ICommand ItemClickedCommand
        {
            get
            {
                if (_ItemClickedCommand == null)
                    _ItemClickedCommand = new RelayCommand<NavigationEntity>(x => ItemClickedExecuted(x), x => ItemClickedCanExecute(x));
                return _ItemClickedCommand;
            }
        }

        private ICommand? _ExpanderHeaderTemplateLoadedCommand;
        public ICommand ExpanderHeaderTemplateLoadedCommand
        {
            get
            {
                if (_ExpanderHeaderTemplateLoadedCommand == null)
                    _ExpanderHeaderTemplateLoadedCommand = new RelayCommand<RoutedEventArgs>(p => ExpanderHeaderTemplateLoadedExecuted(p), p => ExpanderHeaderTemplateLoadedCanExecute(p));
                return _ExpanderHeaderTemplateLoadedCommand;
            }
        }
        #endregion

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

        #region DP TextDecorations
        public static readonly DependencyProperty TextDecorationsProperty =
            Inline.TextDecorationsProperty.AddOwner(typeof(Hyperlink));

        public TextDecorationCollection TextDecorations
        {
            get { return (TextDecorationCollection)GetValue(TextDecorationsProperty); }
            set { SetValue(TextDecorationsProperty, value); }
        }
        #endregion

        #region DP HoverBrush
        public static readonly DependencyProperty HoverBrushProperty =
                    DependencyProperty.Register("HoverBrush",
                    typeof(SolidColorBrush),
                    typeof(Hyperlink),
                    new PropertyMetadata(new SolidColorBrush(Colors.Green)));

        public SolidColorBrush HoverBrush
        {
            get { return (SolidColorBrush)GetValue(HoverBrushProperty); }
            set { SetValue(HoverBrushProperty, value); }
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
        private static void OnNavigationPaneModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (NavigationPane)d;
            var model = e.NewValue as NavigationPaneModel;
            if (model != null)
            {
                // Store the expanded setting for later use in Initialize
                control._isPaneExpanded = model.IsExpanded;
            }
        }
        #endregion

        #region DP IsLoading
        public static readonly DependencyProperty IsLoadingProperty =
                    DependencyProperty.Register("IsLoading",
                    typeof(bool),
                    typeof(NavigationPane),
                    new PropertyMetadata(true, new PropertyChangedCallback(OnIsLoadingChanged)));

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        private static void OnIsLoadingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //var control = (NavigationPane)d;
        }
        #endregion

        #region DP IsPaneExpanded
        public static readonly DependencyProperty IsPaneExpandedProperty =
                DependencyProperty.Register("IsPaneExpanded",
                typeof(bool),
                typeof(NavigationPane),
                new PropertyMetadata(
                    defaultValue: false,
                    propertyChangedCallback: new PropertyChangedCallback(OnIsPaneExpandedChanged),
                    coerceValueCallback: new CoerceValueCallback(CoerceIsPaneExpanded)
        ));

        private static void OnIsPaneExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (NavigationPane)d;
            control._isPaneExpanded = (bool)e.NewValue;
            control.CoerceValue(IsPaneExpandedProperty);

            if (control._isInitialized && control._isPaneExpanded)
            {
                _ = control.Load();
            }
        }

        private static object CoerceIsPaneExpanded(DependencyObject d, object baseValue)
        {
            var control = (NavigationPane)d;
            var newVal = control._isInitialized ? baseValue : (object)false;
            return newVal;
        }
        #endregion
        #endregion

        #region Routed Events
        #region ItemClickedEvent
        public static readonly RoutedEvent ItemClickedEvent =
                    EventManager.RegisterRoutedEvent("ItemClicked",
                    RoutingStrategy.Bubble,
                    typeof(RoutedEventHandler),
                    typeof(NavigationPane));

        public event RoutedEventHandler ItemClicked
        {
            add { AddHandler(ItemClickedEvent, value); }
            remove { RemoveHandler(ItemClickedEvent, value); }
        }

        private void RaiseItemClickedEvent(NavigationEntity entity)
        {
            var args = new NavigationEventArgs(ItemClickedEvent, entity);
            RaiseEvent(args);
        }
        #endregion
        #endregion

        #region CTORs
        static NavigationPane()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationPane),
                new FrameworkPropertyMetadata(typeof(NavigationPane)));
        }

        public NavigationPane()
        {
            Initialized += NavigationPane_Initialized;
        }
        #endregion

        #region Private Methods
        private bool ExpanderHeaderTemplateLoadedCanExecute(RoutedEventArgs args)
        {
            return true;
        }

        private void ExpanderHeaderTemplateLoadedExecuted(RoutedEventArgs args)
        {
            var border = args.Source as Border;

            if (border != null)
            {
                var presenter = border.TemplatedParent as ContentPresenter;

                if (presenter != null)
                {
                    presenter.HorizontalAlignment = HorizontalAlignment.Stretch;
                }
            }
        }

        private bool ItemClickedCanExecute(NavigationEntity args)
        {
            return true;
        }

        private void ItemClickedExecuted(NavigationEntity args)
        {
            RaiseItemClickedEvent(args);
        }

        private async Task Load()
        {
            if (NavigationPaneModel != null && NavigationPaneModel.DataSource != null)
            {
                List<NavigationEntity>? data = null;

                await Task.Run(() => 
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        data = NavigationPaneModel.DataSource();
                    });
                    // This throws with
                    //
                    //  "The calling thread cannot access this object because a different thread owns it.'"
                    //
                    // So I need to figure out how to call the delegate async
                    //data = NavigationPaneModel.DataSource();
                });

                if (data != null)
                {
                    Items = new ObservableCollection<NavigationEntity>(data);
                }
            }
        }
        #endregion

        #region Event Handlers
        private void NavigationPane_Initialized(object? sender, EventArgs e)
        {
            _isInitialized = true;
            CoerceValue(IsPaneExpandedProperty);

            if (_isPaneExpanded) //<====== 2. THIS IS NEVER TRUE
            {
                _ = Load();
            }
        }
        #endregion
    }
}
