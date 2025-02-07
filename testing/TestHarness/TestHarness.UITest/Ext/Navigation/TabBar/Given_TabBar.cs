﻿namespace TestHarness.UITest;

public class Given_TabBar : NavigationTestBase
{
	[Test]
	public async Task When_TabBar()
	{
		InitTestSection(TestSections.Navigation_TabBar);


		// Load the TabBar home page
		App.WaitThenTap("ShowTabBarHomeButton");
		App.WaitElement("TabBarHomeNavigationBar");

		// Check basic nav item selection
		App.WaitThenTap("ProductsTabBarItem");
		CheckProductsVisible();
		App.WaitThenTap("DealsTabBarItem");
		CheckDealsVisible();
		App.WaitThenTap("ProfileTabBarItem");
		CheckProfileVisible();
		App.WaitThenTap("ProductsTabBarItem");
		CheckProductsVisible();

		// Check nav from buttons in views
		App.WaitThenTap("ProductsTabBarItem");
		CheckProductsVisible();
		App.WaitThenTap("ProductsDealsButton");
		CheckDealsVisible();
		App.WaitThenTap("ProductsTabBarItem");
		CheckProductsVisible();
		App.WaitThenTap("ProductsProfileButton");
		CheckProfileVisible();

		App.WaitThenTap("DealsTabBarItem");
		CheckDealsVisible();
		App.WaitThenTap("DealsProductsButton");
		CheckProductsVisible();
		App.WaitThenTap("DealsTabBarItem");
		CheckDealsVisible();
		App.WaitThenTap("DealsProfileButton");
		CheckProfileVisible();

		App.WaitThenTap("ProfileTabBarItem");
		CheckProfileVisible();
		App.WaitThenTap("ProfileProductsButton");
		CheckProductsVisible();
		App.WaitThenTap("ProfileTabBarItem");
		CheckProfileVisible();
		App.WaitThenTap("ProfileDealsButton");
		CheckDealsVisible();

	}

	private void CheckProductsVisible()
	{
		var text = App.Marked("CurrentTabBarItemTextBlock").GetText();
		text.Should().Be("Products");
		var isVisible = App.Marked("ProductsStackPanel").IsVisible();
		isVisible.Should().Be(true);
		isVisible = App.Marked("DealsStackPanel").IsVisible();
		isVisible.Should().Be(false);
		isVisible = App.Marked("ProfileStackPanel").IsVisible();
		isVisible.Should().Be(false);
	}

	private void CheckDealsVisible()
	{
		var text = App.Marked("CurrentTabBarItemTextBlock").GetText();
		text.Should().Be("Deals");
		var isVisible = App.Marked("ProductsStackPanel").IsVisible();
		isVisible.Should().Be(false);
		isVisible = App.Marked("DealsStackPanel").IsVisible();
		isVisible.Should().Be(true);
		isVisible = App.Marked("ProfileStackPanel").IsVisible();
		isVisible.Should().Be(false);

	}


	private void CheckProfileVisible()
	{
		var text = App.Marked("CurrentTabBarItemTextBlock").GetText();
		text.Should().Be("Profile");
		var isVisible = App.Marked("ProductsStackPanel").IsVisible();
		isVisible.Should().Be(false);
		isVisible = App.Marked("DealsStackPanel").IsVisible();
		isVisible.Should().Be(false);
		isVisible = App.Marked("ProfileStackPanel").IsVisible();
		isVisible.Should().Be(true);

	}
}
