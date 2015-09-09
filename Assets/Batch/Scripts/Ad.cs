using UnityEngine;
using Batch.Internal;
using System.Collections;
using System;
using System.Runtime.InteropServices;

namespace Batch
{
	public class Interstitial : IAdDisplayListener
	{

#if UNITY_ANDROID
		private AndroidJavaObject androidAdHelper;
#endif

#if UNITY_IPHONE
		[DllImport("__Internal")]
		private static extern void BAUnityInterstitialShow(IntPtr handle, string placement);

		private String placement;
#endif

#region Handlers
		/// <summary>
		/// Called when no ad has been displayed to the user.
		/// It can happens if no ads are available or on error.
		/// </summary>
		public EventHandler NoAdDisplayed;

		/// <summary>
		/// Called when the ad is displayed to the user.
		/// </summary>
		public EventHandler AdDisplayed;

		/// <summary>
		///  Called when the previously displayed ad is closed.
		/// </summary>
		public EventHandler AdClosed;

		/// <summary>
		/// Called when the user cancelled the ad.
		/// This can be due to the used pressing either the close button or the back button.
		/// </summary>
		public EventHandler AdCancelled;

		/// <summary>
		/// Called when the user clicked the Ad.
		/// AdClosed will be called afterwards
		/// </summary>
		public EventHandler AdClicked;
#endregion

		/// <summary>
		/// Get a new instance of <see cref="Batch.Interstitial"/> for a placement.
		/// You should bind event handlers once you instanciated this.
		/// </summary>
		/// <param name="placement">The placement to show this interstitial for</param>
		public Interstitial(String placement)
		{

#if UNITY_ANDROID
			androidAdHelper = new AndroidJavaObject("com.batch.android.unity.internal.Interstitial", placement, new AndroidAdDisplayListener(this));
#elif UNITY_IPHONE
			this.placement = placement;
#endif
		}

		/// <summary>
		/// Show the interstitial (if possible).
		/// </summary>
		public void Display()
		{
			if (!Batch.Internal.AdsModule.CheckModuleSetup("Display"))
			{
				return;
			}
#if UNITY_ANDROID
			androidAdHelper.Call("display");
#elif UNITY_IPHONE
			BAUnityInterstitialShow(GCHandle.ToIntPtr(GCHandle.Alloc(this)), placement);
#endif
		}

#region IAdDisplayListener
		void IAdDisplayListener.FireNoAdDisplayed()
		{
			if( NoAdDisplayed != null)
			{
				NoAdDisplayed(this, EventArgs.Empty);
			}
		}
		
		void IAdDisplayListener.FireAdDisplayed()
		{
			if( AdDisplayed != null)
			{
				AdDisplayed(this, EventArgs.Empty);
			}
		}
		
		void IAdDisplayListener.FireAdClosed()
		{
			if( AdClosed != null)
			{
				AdClosed(this, EventArgs.Empty);
			}
		}
		
		void IAdDisplayListener.FireAdCancelled()
		{
			if( AdCancelled != null)
			{
				AdCancelled(this, EventArgs.Empty);
			}
		}
		
		void IAdDisplayListener.FireAdClicked()
		{
			if( AdClicked != null)
			{
				AdClicked(this, EventArgs.Empty);
			}
		}
#endregion

	}

	[Obsolete("Please use Batch.Interstitial")]
	public class Ad : Interstitial
	{
		/// <summary>
		/// Get a new instance of <see cref="Batch.Ad"/> for a placement.
		/// You should bind event handlers once you instanciated this.
		/// </summary>
		/// <param name="placement">The placement to show this ad for</param>
		[Obsolete("Please use Batch.Interstitial")]
		public Ad(String placement) : base(placement)
		{
		}
	}
}
