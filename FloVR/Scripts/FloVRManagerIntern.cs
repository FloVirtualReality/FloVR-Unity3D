using System;
using UnityEngine;
using System.Runtime.InteropServices;


namespace FloVR {
	
	internal class FloVRManagerIntern : AndroidJavaProxy
	{
#if UNITY_ANDROID
		private AndroidJavaObject _nativeObject;
		private AndroidJavaObject _unityPlayer;
#elif UNITY_IOS
		private IntPtr _nativeObject;
#endif

		private SystemState _state;
		internal SystemState State {
			get {
				return _state;
			}
			set {
				_state = value;
				Debug.Log(State);
				if (_initCallback != null)
					_initCallback(value);
			}
		}


		internal static FloVRManagerIntern Instance {
			get;
			private set;
		}

		private Action<SystemState> _initCallback; 

		private FloVRManagerIntern(string appId, string appSecret, Action<SystemState> initCallback) : base("com.flovr.sdk.CSFloVRManager")
		{
			_initCallback = initCallback;
			State = SystemState.Initializing;
#if UNITY_ANDROID
			try
			{
				_unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				using (AndroidJavaObject unityActivity = _unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					_nativeObject = new AndroidJavaObject("com.flovr.sdk.FloVRManager", this, unityActivity, appId, appSecret);
				}
			}
			catch { 
			}
#elif UNITY_IOS
			_nativeObject = FloVRManagerCtrIntern(ToIntPtr(this), appId, appSecret);
			#endif
		}

		internal void OnInitCompleted()
		{
			State = SystemState.Initialized;
		}

		internal void PauseUnityPlayer()
		{
			#if UNITY_ANDROID
			_unityPlayer.Call ("pause");
			#endif
		}

		internal void ResumeUnityPlayer()
		{
			#if UNITY_ANDROID
			_unityPlayer.Call ("resume");
			#endif
		}

		internal static void Initialize(string appId, string appSecret, Action<SystemState> initCallback)
		{
			if (Instance == null) {
				Instance = new FloVRManagerIntern (appId, appSecret, initCallback);
			} else {
				Debug.LogWarning ("FloVR: SDK is already initialized.");
				return;
			}
		}

		#if UNITY_IOS
		#region Native calls

		[DllImport("__Internal")]
		private static extern IntPtr FloVRManagerCtrIntern(IntPtr csharpAd, string appId, string appSecret);

		#endregion
		#endif


		private static IntPtr ToIntPtr(object obj)
		{
			GCHandle handle1 = GCHandle.Alloc(obj);
			return (IntPtr)handle1;
		}

		private static T FromIntPtr<T>(IntPtr pointer)
		{
			return (T)((GCHandle)pointer).Target;
		}
	}

	public enum SystemState {
		Initializing = 0,
		FailedToInitialize = 1,
		Initialized = 2
	}
}
