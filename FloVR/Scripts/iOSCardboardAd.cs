using System;
using UnityEngine;
using System.Runtime.InteropServices;
using AOT;
using System.Collections.Generic;

namespace FloVR.AdTypes
{
	public class iOSCardboardAd
	{
		private IntPtr _nativeObject;

		private AdState _state;
		public virtual AdState State
		{
			get
			{
				return _state;
			}
			protected set
			{
				_state = value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this ad is shown.
		/// </summary>
		/// <value><c>true</c> if is shown; otherwise, <c>false</c>.</value>
		public bool IsShown
		{
			get
			{
				return _IsShownIntern(_nativeObject);
			}
		}

		/// <summary>
		/// Gets a value indicating whether this ad is loaded and ready to be shown.
		/// </summary>
		/// <value><c>true</c> if is loaded; otherwise, <c>false</c>.</value>
		public bool IsLoaded
		{
			get;
			private set;
		}

		/// <summary>
		/// Initializes a new ad.
		/// </summary>
		/// <param name="type">Type of the ad.</param>
		/// <param name="isRewarded">If set to <c>true</c> is rewarded.</param>
		public iOSCardboardAd(AdType type, bool isRewarded = false)
		{
			State = AdState.Initializing;
			_adLookup.Add(_counter, this);
			_onAdFinished = new SignalCallback(OnAdFinished);
			_onError = new SignalCallback(OnError);
			_onLoadFinished = new SignalCallback(OnLoadFinished);
			_nativeObject = _iOSCardboardAdCtrIntern(_counter, _onAdFinished, _onError, _onLoadFinished, (int)type, isRewarded); //_iOSCardboardAdCtrIntern(ToIntPtr(this), (int)type, isRewarded);
			_counter++;
		}

		/// <summary>
		/// Show the ad if loaded and not shown.
		/// </summary>
		public void Show()
		{
			State = AdState.Running;
			_ShowIntern(_nativeObject);
			FloVRManagerIntern.Instance.PauseUnityPlayer();
		}

		/// <summary>
		/// Called from native code. Please don't call this function.
		/// </summary>
		[MonoPInvokeCallback(typeof(SignalCallback))]
		public static void OnAdFinished(int adId)
		{
			iOSCardboardAd currentAd = _adLookup[adId];
			currentAd.State = AdState.Shown;
			FloVRManagerIntern.Instance.ResumeUnityPlayer();
		}

		/// <summary>
		/// Called from native code. Please don't call this function.
		/// </summary>
		[MonoPInvokeCallback(typeof(SignalCallback))]
		public static void OnError(int adId)
		{
#if DEBUG
			Debug.LogError("Error occured");
#endif
		}

		/// <summary>
		/// Called from JNI. Please don't call this function.
		/// </summary>
		[MonoPInvokeCallback(typeof(SignalCallback))]
		public static void OnLoadFinished(int adId)
		{
			Debug.Log("callback");
			iOSCardboardAd currentAd = _adLookup[adId];
			currentAd.IsLoaded = true;
			currentAd.State = AdState.Ready;
		}

		private static int _counter = 0;
		private static Dictionary<int, iOSCardboardAd> _adLookup = new Dictionary<int, iOSCardboardAd>();

		private static IntPtr ToIntPtr(object obj)
		{
			var data = GCHandle.ToIntPtr(GCHandle.Alloc(obj));
			return data;
		}

		private static T FromIntPtr<T>(IntPtr pointer)
		{
			return (T)((GCHandle)pointer).Target;
		}

		#region Native calls

		[DllImport("__Internal")]
		private static extern bool _IsShownIntern(IntPtr _nativeObject);

		[DllImport("__Internal")]
		private static extern IntPtr _iOSCardboardAdCtrIntern(int adId, SignalCallback onAdFinished, SignalCallback onError, SignalCallback onLoadFinished, int type, bool isRewarded);

		[DllImport("__Internal")]
		private static extern void _ShowIntern(IntPtr _nativeObject);

		internal delegate void SignalCallback(int adId);

		private SignalCallback _onAdFinished;
		private SignalCallback _onError;
		private SignalCallback _onLoadFinished;


		#endregion
	}
}
