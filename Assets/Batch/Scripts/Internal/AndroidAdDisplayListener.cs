using System;
using UnityEngine;

namespace Batch.Internal
{
#if UNITY_ANDROID
	/**
	 * Class that relays Java listener calls to native C# calls
	 */
	internal class AndroidAdDisplayListener : AndroidJavaProxy 
	{
		private IAdDisplayListener listener;

		public AndroidAdDisplayListener(IAdDisplayListener listener) : base("com.batch.android.AdDisplayListener")
		{
			this.listener = listener;
		}

		public void onNoAdDisplayed()
		{
			listener.FireNoAdDisplayed();
		}
		
		public void onAdDisplayed()
		{
			listener.FireAdDisplayed();
		}
		
		public void onAdClosed()
		{
			listener.FireAdClosed();
		}
		
		public void onAdCancelled()
		{
			listener.FireAdCancelled();
		}
		
		public void onAdClicked()
		{
			listener.FireAdClicked();
		}
	}
#endif
}