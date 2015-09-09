using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using LitJson;
using Batch;

namespace Batch.Internal
{
	public class AdsModule : ModuleBase
	{
		internal static bool setupCalled = false;	

		#region Handlers
		/// <summary>
		/// Called when an interstitial is ready for the given placement.
		/// </summary>
		public event InterstitialReadyHandler InterstitialReady;

		/// <summary>
		/// Called when no interstitial is available for the given placement.
		/// </summary>
		public event FailedToLoadInterstitialHandler FailedToLoadInterstitial;
		#endregion

		/// <summary>
		/// Called when an ad is available for the given placement.
		/// </summary>
		[Obsolete("Use InterstitialReady instead")]
		public event AdAvailableForPlacementHandler AdAvailableForPlacement;

		/// <summary>
		/// Called when no ad is available for the given placement.
		/// </summary>
		[Obsolete("Use FailedToLoadInterstitialForPlacement instead")]
		public event FailedToLoadAdForPlacementHandler FailedToLoadAdForPlacement;

		internal AdsModule (Bridge _bridge) : base(_bridge)
		{
		}

		/// <summary>
		/// Returns if Batch Ads has been setup or not. Can yell in the logs if non null method name.
		/// </summary>
		public static bool CheckModuleSetup(string methodName)
		{
			if (methodName != null && !setupCalled)
			{
				Logger.Error(false, methodName, "Cannot use this method before calling Ads.Setup()");
			}
			return setupCalled;
		}

		/// <summary>
		/// Sets whether Batch should automatically load ads or not.
		/// Should be called before StartPlugin !
		/// </summary>
		public bool AutoLoad
		{
			private get
			{
				//Dummy value, getter is never used
				return true;
			}
			set
			{
				setAutoLoad(value);
			}
		}

		/// <summary>
		/// Setup Batch Ads
		/// </summary>
		public void Setup()
		{
			setupCalled = true;
			bridge.Call("ads.setup", "");
		}

		/// <summary>
		/// Check if an interstitial is ready for the specified placement.
		/// Returns true if an interstitial is ready, false otherwise
		/// </summary>
		public bool HasInterstitialReady(String placement)
		{
			if (!CheckModuleSetup("HasInterstitialReady"))
			{
				return false;
			}
			JsonData data = new JsonData();
			data["placement"] = placement;
			return Bridge.ResultToBool(bridge.Call("ads.hasInstersitialReady", JsonMapper.ToJson(data)));
		}

		/// <summary>
		/// Load an interstitial. Only useful in manual load mode.
		/// You will be notified of success or failure in InterstitialReady or FailedToLoadInterstitialForPlacement
		/// </summary>
		public void LoadInterstitial(String placement)
		{
			if (!CheckModuleSetup("LoadInterstitial"))
			{
				return;
			}
			JsonData data = new JsonData();
			data["placement"] = placement;
			bridge.Call("ads.loadInstersitial", JsonMapper.ToJson(data));
		}

		/// <summary>
		/// Display an interstitial for a placement.
		/// Returns true on success, false when no interstitials are available
		/// </summary>
		public bool DisplayInterstitial(string placement)
		{
			if (!CheckModuleSetup("DisplayInterstitial"))
			{
				return false;
			}
			JsonData data = new JsonData();
			data["placement"] = placement;
			return Bridge.ResultToBool(bridge.Call("ads.displayInterstitial", JsonMapper.ToJson(data)));
		}

		// Deprecated methods

		/// <summary>
		/// Load an Ad. Only useful in manual load mode.
		/// You will be notified of success or failure in AdAvailableForPlacement or FailedToLoadAdForPlacement
		/// </summary>
		[Obsolete("Use LoadInterstitial")]
		public void LoadForPlacement(String placement)
		{
			LoadInterstitial(placement);
		}
		
		/// <summary>
		/// Display an Ad for a placement.
		/// Returns true on success, false when no ads are available
		/// </summary>
		[Obsolete("Use DisplayInterstitial")]
		public bool Display(string placement)
		{
			return DisplayInterstitial(placement);
		}
		
		/// <summary>
		/// Check if an Ad is ready for the specified placement.
		/// Returns true if an Ad is ready, false otherwise
		/// </summary>
		[Obsolete("Use HasInterstitialReady")]
		public bool HasAdReadyForPlacement(String placement)
		{
			return HasInterstitialReady(placement);
		}	

		public void OnInterstitialReady(string response)
		{
			if (InterstitialReady != null)
			{
				Response answer = new Response(response);
				InterstitialReady(answer.GetPlacement());
			}

#pragma warning disable 612, 618
			if (AdAvailableForPlacement != null)
			{
				Response answer = new Response(response);
				AdAvailableForPlacement(answer.GetPlacement());
			}
#pragma warning restore 612, 618		
		}

		public void OnFailedToLoadInterstitial(string response)
		{
			if (FailedToLoadInterstitial != null)
			{
				Response answer = new Response(response);
				FailedToLoadInterstitial(answer.GetPlacement());
			}

#pragma warning disable 612, 618
			if (FailedToLoadAdForPlacement != null)
			{
				Response answer = new Response(response);
				FailedToLoadAdForPlacement(answer.GetPlacement());
			}			
#pragma warning restore 612, 618
		}

		public void OnAdsDisplayListenerEvent(string response)
		{
#if UNITY_IPHONE
			Response answer = new Response(response);
			IntPtr handle = answer.GetHandle();

			try
			{
				GCHandle gchandle = GCHandle.FromIntPtr(handle);
				IAdDisplayListener ad = (IAdDisplayListener) gchandle.Target;
				string eventName = answer.GetAdDisplayEvent().ToLower();
				switch (eventName)
				{
				case "noaddisplayed":
					ad.FireNoAdDisplayed();
					gchandle.Free();
					break;
				case "addisplayed":
					ad.FireAdDisplayed();
					break;
				case "adclicked":
					ad.FireAdClicked();
					break;
				case "adclosed":
					ad.FireAdClosed();
					gchandle.Free();
					break;
				case "adcancelled":
					ad.FireAdCancelled();
					break;
				default:
					Logger.Error(true, "OnAdsDisplayListenerEvent", "Invalid OnAdsDisplayListenerEvent event name " + eventName + ". Probably leaking !!!");
					break;
				}
			}
			catch (InvalidOperationException e)
			{
				Logger.Error(true, "OnAdsDisplayListenerEvent", "Failed to get Batch.Interstitial instance from Handle");
				Logger.Error(true, "OnAdsDisplayListenerEvent", e);
			}
#endif
		}

		private void setAutoLoad(bool enableAutoLoad)
		{
			JsonData data = new JsonData();
			data["enable"] = enableAutoLoad;
			bridge.Call("ads.setAutoLoad", JsonMapper.ToJson(data));
		}
	}
}

