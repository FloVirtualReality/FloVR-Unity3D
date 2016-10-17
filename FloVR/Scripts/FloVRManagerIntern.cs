using System;
using UnityEngine;
using System.Collections;
using System.Net;

namespace FloVR {
	
	internal class FloVRManagerIntern : AndroidJavaProxy
	{
		private AndroidJavaObject _nativeObject;
		private AndroidJavaObject _unityPlayer;

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
		}

		internal void OnInitCompleted()
		{
			State = SystemState.Initialized;
		}

		internal void PauseUnityPlayer()
		{
			_unityPlayer.Call ("pause");
		}

		internal void ResumeUnityPlayer()
		{
			_unityPlayer.Call ("resume");
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
	}

	public enum SystemState {
		Initializing = 0,
		FailedToInitialize = 1,
		Initialized = 2
	}
}
