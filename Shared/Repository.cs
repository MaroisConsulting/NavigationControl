namespace Shared
{
    /// <summary>
    /// Sample repo
    /// </summary>
    public static class Repository
    {
        public static List<NavigationEntity> GetNavigationItems(NavigationItemType itemType)
        {
            var results = new List<NavigationEntity>();

            switch (itemType)
            {
                case NavigationItemType.Inventory:
                    // Don't return any data
                    break;

                case NavigationItemType.Company:
                    for (int i = 1; i < 76; i++)
                    {
                        results.Add(new NavigationEntity()
                        {
                            Id = i,
                            Caption = $"Company {i}",
                            ItemType= NavigationItemType.Company
                        });
                    }

                    break;

                case NavigationItemType.Project:
                    for (int i = 1; i < 151; i++)
                    {
                        results.Add(new NavigationEntity()
                        {
                            Id = i,
                            Caption = $"Project {i}",
                            ItemType = NavigationItemType.Project
                        });
                    }

                    break;

                case NavigationItemType.Employee:

                    for (int i  =1; i < 81; i++)
                    {
                        results.Add(new NavigationEntity()
                        {
                            Id = i,
                            Caption = $"Employee {i}",
                            ItemType = NavigationItemType.Employee
                        });
                    }
                    break;
            }

            // Simulate a long runnning query
            Thread.Sleep(3000);

            return results;
        }
    }
}
