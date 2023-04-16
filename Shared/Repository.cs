﻿namespace Shared
{
    /// <summary>
    /// Sample repo
    /// </summary>
    public class Repository
    {
        public List<NavigationEntity> GetNavigationItems(NavigationItemType itemType)
        {
            var results = new List<NavigationEntity>();

            switch (itemType)
            {
                case NavigationItemType.Inventory:
                    // Don't return any data
                    break;

                case NavigationItemType.Company:
                    for (int i = 0; i < 75; i++)
                    {
                        results.Add(new NavigationEntity()
                        {
                            Caption = $"Company {i}"
                        });

                        Thread.Sleep(1000);
                    }

                    break;

                case NavigationItemType.Project:
                    for (int i = 0; i < 150; i++)
                    {
                        results.Add(new NavigationEntity()
                        {
                            Caption = $"Project {i}"
                        });

                        Thread.Sleep(500);
                    }

                    break;

                case NavigationItemType.Employee:

                    for (int i = 0; i < 80; i++)
                    {
                        results.Add(new NavigationEntity()
                        {
                            Caption = $"Employee {i}"
                        });

                        Thread.Sleep(250);
                    }
                    break;
            }

            return results;
        }
    }
}
