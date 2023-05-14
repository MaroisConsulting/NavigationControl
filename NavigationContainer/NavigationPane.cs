using CommunityToolkit.Mvvm.Input;
using Marois.Framework.Core.WPF.Controls;
using Shared;
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
            Inline.TextDecorationsProperty.AddOwner(typeof(MaroisHyperlink));

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
                    typeof(MaroisHyperlink),
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

        #region CTOR
        static NavigationPane()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationPane),
                new FrameworkPropertyMetadata(typeof(NavigationPane)));
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
