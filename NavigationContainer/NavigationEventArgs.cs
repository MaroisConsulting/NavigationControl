using Shared;
using System.Windows;

namespace NavigationContainer
{
    public class NavigationEventArgs : RoutedEventArgs
    {
        public NavigationEntity NavigationEntity { get; private set; }

        public NavigationEventArgs(RoutedEvent routedEvent, NavigationEntity navigationEntity)
            :base(routedEvent)
        {
            NavigationEntity = navigationEntity;
        }
    }
}
