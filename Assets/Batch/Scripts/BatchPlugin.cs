using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LitJson;

using Batch;
using Batch.Internal;
using MrBalloony;

namespace Batch
{
	/// <summary>
	/// Batch plugin.
	/// </summary>
	public class BatchPlugin : MonoBehaviour
	{
		public const string VERSION = "1.3.2";
		public const string UNITY_GAME_OBJECT_NAME = "BatchUnityPlugin";
		public const string DEFAULT_PLACEMENT = "DEFAULT";

		private static bool started = false;

		private Bridge bridge;
		private UnlockModule unlock;
		private PushModule push;
		private AdsModule ads;
	    
	    void Awake()
	    {
			gameObject.name = UNITY_GAME_OBJECT_NAME;
			bridge = new Bridge();
			unlock = new UnlockModule(bridge);
			push = new PushModule(bridge);
			ads = new AdsModule(bridge);
	    }

	    void Start()
	    {
			bool unsupportedPlatform = true;
			#if UNITY_ANDROID || UNITY_IPHONE
			if(!UnityEngine.Application.isEditor)
			{
				unsupportedPlatform = false;
			}
			#endif
			if(unsupportedPlatform)
			{
				Logger.Log(false, "Plugin Load", string.Format("{0} - Disabled because running in the editor or on an unsupported platform", VERSION));
			}
			else
			{
				Logger.Log(false, "Plugin Load", string.Format("{0} Loaded", VERSION));
			}

			Config config = new Config();
			//TODO change this on release for production
        	config.IOSAPIKey = GameConstants.BATCH_DEV_KEY_IOS;
        	config.AndroidAPIKey = GameConstants.BATCH_DEV_KEY_ANDROID;
			StartPlugin(config);//BatchPlugin.
			Push.RegisterForRemoteNotifications();
		}

		/// <summary>
		/// Unlock submodule.
		/// </summary>
		public UnlockModule Unlock
		{
			get
			{
				return unlock;
			}
			private set
			{
			}
		}

		/// <summary>
		/// Push submodule.
		/// </summary>
		public PushModule Push
		{
			get
			{
				return push;
			}
			private set
			{
			}
		}

		/// <summary>
		/// Ads submodule.
		/// </summary>
		public AdsModule Ads
		{
			get
			{
				return ads;
			}
			private set
			{
			}
		}

		/// <summary>
		/// Check if Batch is running in dev mode.
		/// </summary>
		public bool IsRunningInDevMode
		{
			get
			{
				return isDevMode();
			}
			private set
			{
			}
		}

		/// <summary>
		/// Access the default user profile object.
		/// </summary>
		public UserProfile DefaultUserProfile
		{
			get
			{
				return new Batch.Internal.DefaultUserProfile(bridge);
			}
			private set
			{
			}
		}

		/// <summary>
		/// Starts the plugin if not already started.
		/// </summary>
		public void StartPlugin(Config config)
		{
			if ( started )
			{
				return;
			}

			bool unsupportedPlatform = true;
			#if UNITY_ANDROID || UNITY_IPHONE
			if (!UnityEngine.Application.isEditor)
			{
				unsupportedPlatform = false;
			}
			#endif
			if (unsupportedPlatform)
			{
				return;
			}

			if (config == null)
			{
				Logger.Error(false, "StartPlugin", "Cannot start the plugin without a config.");
				return;
			}

			#if UNITY_ANDROID
			if(config.AndroidAPIKey == null)
			{
				Logger.Error(false, "StartPlugin", "Cannot start the plugin without an API key.");
				return;
			}
			#endif

			#if UNITY_IPHONE
			if(config.IOSAPIKey == null)
			{
				Logger.Error(false, "StartPlugin", "Cannot start the plugin without an API key.");
				return;
			}
			#endif

			try
			{
				setConfig(config);
				bridge.Call("start", "");
				started = true;
			}
			catch(Exception e)
			{
				Logger.Error(true, "StartPlugin", e);
			}
		}

		private void setConfig(Config config)
		{
			try
			{
				JsonData data = new JsonData();
				if ( config.CanUseIDFA != null )
				{
					data["useIDFA"] = config.CanUseIDFA;
				}
				if ( config.CanUseAndroidID != null )
				{
					data["useAndroidID"] = config.CanUseAndroidID;
				}
				string apiKey = "";
				#if UNITY_ANDROID
				apiKey = config.AndroidAPIKey;
				#elif UNITY_IPHONE
				apiKey = config.IOSAPIKey;
				#endif
				data["APIKey"] = apiKey;
				bridge.Call("setConfig", JsonMapper.ToJson(data));
			}
			catch(Exception e)
			{
				Logger.Error(true, "setConfig", e);
			}
		}

		private bool isDevMode()
		{
			return Bridge.ResultToBool(bridge.Call("isDevMode", ""));
		}

		// ------------------------------------------------------
		// Callbacks
		
		/**
		 * Batch Bridge internal error
		 */
		void onBridgeFailure(string response)
		{
			try
			{
				Response answer = new Response(response);

				// TODO: treat the error.
				answer.GetFailReason();

			}
			catch(Exception e)
			{
				Logger.Error(true, "onBridgeFailure", e);
			}
		}
		
		void onRedeemAutomaticOffer(string response)
		{
			unlock.OnRedeemAutomaticOffer(response);
		}

		void onRedeemURLSuccess(string response)
		{
			unlock.OnRedeemURLSuccess(response);
		}

		void onRedeemURLFailed(string response)
		{
			unlock.OnRedeemURLFailed(response);
		}

		void onRedeemURLCodeFound(string response)
		{
			unlock.OnRedeemURLCodeFound(response);
		}

		void onRedeemCodeSuccess(string response)
		{
			unlock.OnRedeemCodeSuccess(response);
		}

		void onRedeemCodeFailed(string response)
		{
			unlock.OnRedeemCodeFailed(response);
		}

		void onRestoreSuccess(string response)
		{
			unlock.OnRestoreSuccess(response);
		}

		void onRestoreFailed(string response)
		{
			unlock.OnRestoreFailed(response);
		}

		void adListener_onInterstitialReady(string response)
		{
			ads.OnInterstitialReady(response);
		}

		void adListener_onFailedToLoadInterstitial(string response)
		{
			ads.OnFailedToLoadInterstitial(response);
		}

#if UNITY_IPHONE
		void onAdDisplayListenerEvent(string response)
		{
			ads.OnAdsDisplayListenerEvent(response);
		}
#endif
	}
}
