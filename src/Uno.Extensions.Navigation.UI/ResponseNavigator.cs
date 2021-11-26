﻿using System;
using System.Threading.Tasks;

namespace Uno.Extensions.Navigation
{
	internal interface IResponseNavigator : INavigator
	{
		NavigationResponse AsResponseWithResult(NavigationResponse? response);
	}

	public class ResponseNavigator<TResult> : IResponseNavigator
	{
		private INavigator Navigation { get; }

		private TaskCompletionSource<Options.Option<TResult>> ResultCompletion { get; }

		public Route? Route => Navigation.Route;

		public ResponseNavigator(INavigator internalNavigation, NavigationRequest request)
		{
			Navigation = internalNavigation;
			ResultCompletion = new TaskCompletionSource<Options.Option<TResult>>();


			if (request.Cancellation.HasValue)
			{
				request.Cancellation.Value.Register(() =>
				{
					ApplyResult(Options.Option.None<TResult>());
				});
			}


			// Replace the navigator
			Navigation.Get<IServiceProvider>()?.AddInstance<INavigator>(this);
		}

		public async Task<NavigationResponse?> NavigateAsync(NavigationRequest request)
		{
			var navResponse = await Navigation.NavigateAsync(request);

			if (request.Route.FrameIsBackNavigation() ||
				(request.Route.IsRoot() && request.Route.TrimScheme(Schemes.Root).FrameIsBackNavigation() && this.Navigation.GetParent() == null))
			{
				var responseData = request.Route.ResponseData();
				var result = responseData as Options.Option<TResult>;
				if (result is null)
				{
					if(responseData is Options.Option<object> objectResponse)
					{
						responseData = objectResponse.GetValue();
					}

					if (responseData is TResult data)
					{
						result = Options.Option.Some(data);
					}
					else
					{
						result = Options.Option.None<TResult>();
					}
				}
				ApplyResult(result);
			}


			if (navResponse is NavigationResultResponse<TResult> typedResponse &&
				typedResponse.Result is not null)
			{
				typedResponse.Result.ContinueWith(x => ApplyResult(x.Result));

				return typedResponse with { Result = ResultCompletion.Task };
			}

			return navResponse;
			//return new NavigationResultResponse<TResult>(navResponse?.Route ?? Route.Empty, ResultCompletion.Task);
		}

		private void ApplyResult(Options.Option<TResult> responseData)
		{
			if (ResultCompletion.Task.Status == TaskStatus.Canceled ||
				ResultCompletion.Task.Status == TaskStatus.RanToCompletion)
			{
				return;
			}

			// Restore the navigator
			Navigation.Get<IServiceProvider>()?.AddInstance<INavigator>(this.Navigation);

			ResultCompletion.TrySetResult(responseData);
		}

		public NavigationResponse AsResponseWithResult(NavigationResponse? response)
			=> new NavigationResultResponse<TResult>(response?.Route ?? Route.Empty, ResultCompletion.Task, response?.Success ?? false);
	}
}