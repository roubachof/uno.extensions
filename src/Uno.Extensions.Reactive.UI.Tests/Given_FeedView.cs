﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.UI.Xaml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uno.Extensions.Reactive.Core;
using Uno.Extensions.Reactive.Testing;
using Uno.Extensions.Reactive.UI;
using Uno.Toolkit;
using Uno.UI.RuntimeTests;

namespace Uno.Extensions.Reactive.WinUI.Tests;

[TestClass]
[RunsOnUIThread]
public class Given_FeedView : FeedTests
{
	[TestMethod]
	public async Task When_Loading()
	{
		var tcs = new TaskCompletionSource<int>();
		var src = Feed.Async(async ct => await tcs.Task);
		var sut = new FeedView { Source = src };
		var sutAsLoadable = sut as ILoadable;

		sutAsLoadable.IsExecuting.Should().BeTrue("The FeedView should consider itself as loading even before being inserted in the visual tree.");

		var isLoadingValues = new List<bool>();
		sutAsLoadable.IsExecutingChanged += (snd, e) => isLoadingValues.Add(sutAsLoadable.IsExecuting);

		await UIHelper.Load(sut, CT);

		isLoadingValues.Should().BeEmpty("The IsLoading should not have changed yet");

		tcs.SetResult(42);

		await UIHelper.WaitFor(() => isLoadingValues.Count > 0, CT);
	}

	[TestMethod]
	public async Task When_GetSource_Then_ContextContainsDispatcher()
	{
		var result = new TaskCompletionSource<bool>();
		var timeout = new CancellationTokenSource(UIHelper.DefaultTimeout).Token;
		using var _ = CancellationTokenSource.CreateLinkedTokenSource(CT, timeout).Token.Register(() => result.TrySetResult(false));
		var src = Feed.Async(async ct => result.TrySetResult(SourceContext.Current.FindDispatcher() is not null));
		var sut = new FeedView { Source = src };

		await UIHelper.Load(sut, CT);

		(await result.Task).Should().BeTrue();
	}
}
