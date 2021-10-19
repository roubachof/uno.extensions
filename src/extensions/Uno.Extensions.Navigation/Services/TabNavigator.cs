﻿using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Uno.Extensions.Logging;
using Uno.Extensions.Navigation.Controls;
using Uno.Extensions.Navigation.ViewModels;
using Uno.Extensions.Navigation.Regions;
using System.Threading.Tasks;
#if WINDOWS_UWP || UNO_UWP_COMPATIBILITY
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#else
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
#endif

namespace Uno.Extensions.Navigation.Services;

public class TabNavigator : ControlNavigator<TabView>
{
    protected override FrameworkElement CurrentView => (Control.SelectedItem as TabViewItem)?.Content as FrameworkElement;

    protected override string CurrentPath => (Control.SelectedItem as TabViewItem)?.NavigationRoute();

    public TabNavigator(
        ILogger<TabNavigator> logger,
        IRegion region,
        IRouteMappings mappings,
        RegionControlProvider controlProvider)
        : base(logger, region, mappings, controlProvider.RegionControl as TabView)
    {
    }

    public override void ControlInitialize()
    {
        Control.SelectionChanged += Tabs_SelectionChanged;
    }

    private void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Logger.LazyLogDebug(() => $"Tab changed");
        var tvi = e.AddedItems?.FirstOrDefault() as TabViewItem;
        var tabName = tvi.Name;
        Logger.LazyLogDebug(() => $"Navigating to path {tabName}");
        //Navigation.NavigateByPathAsync(null, tabName);

        var request = Mappings.FindByPath(tabName).AsRequest(this);
        var context = request.BuildNavigationContext(Region.Services);

        InitialiseView(context);
    }

    private TabViewItem FindByName(string tabName)
    {
        Logger.LazyLogDebug(() => $"Looking for tab with name '{tabName}'");
        return (from t in Control.TabItems.OfType<TabViewItem>()
                where t.Name == tabName
                select t).FirstOrDefault();
    }

    protected override async Task Show(string path, Type view, object data)
    {
        try
        {
            var tab = FindByName(path);
            if (tab is not null)
            {
                Logger.LazyLogDebug(() => $"Selecting tab '{path}'");
                Control.SelectionChanged -= Tabs_SelectionChanged;
                Control.SelectedItem = tab;
                await (tab.Content as FrameworkElement).EnsureLoaded();
                Control.SelectionChanged += Tabs_SelectionChanged;
                Logger.LazyLogDebug(() => $"Tab '{path}' selected");
            }
            else
            {
                Logger.LazyLogWarning(() => $"Tab '{path}' not found");
            }
        }
        catch (Exception ex)
        {
            Logger.LazyLogError(() => $"Unable to show tab - {ex.Message}");
        }
    }
}
