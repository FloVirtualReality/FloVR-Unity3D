using System;
using UnityEngine;
using System.Runtime.InteropServices;

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
				return IsShownIntern(_nativeObject);
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
			_nativeObject = iOSCardboardAdCtrIntern(ToIntPtr(this), (int)type, isRewarded);
		}

		/// <summary>
		/// Show the ad if loaded and not shown.
		/// </summary>
		public void Show()
		{
			State = AdState.Running;
			ShowIntern(_nativeObject);
			FloVRManagerIntern.Instance.PauseUnityPlayer();
		}

		/// <summary>
		/// Called from native code. Please don't call this function.
		/// </summary>
		public static void OnAdFinished(IntPtr obj,string result)
		{
			iOSCardboardAd current = FromIntPtr<iOSCardboardAd>(obj);
			current.State = AdState.Shown;
			FloVRManagerIntern.Instance.ResumeUnityPlayer();
		}

		/// <summary>
		/// Called from native code. Please don't call this function.
		/// </summary>
		public static void OnError(string error)
		{
#if DEBUG
			Debug.LogError(error);
#endif
		}

		/// <summary>
		/// Called from JNI. Please don't call this function.
		/// </summary>
		public void OnLoadFinished()
		{
			IsLoaded = true;
			State = AdState.Ready;
		}

		private static IntPtr ToIntPtr(object obj)
		{
			GCHandle handle1 = GCHandle.Alloc(obj);
			return (IntPtr)handle1;
		}

		private static T FromIntPtr<T>(IntPtr pointer)
		{
			return (T)((GCHandle)pointer).Target;
		}

		#region Native calls

		[DllImport("__Internal")]
		private static extern bool IsShownIntern(IntPtr _nativeObject);

		[DllImport("__Internal")]
		private static extern IntPtr iOSCardboardAdCtrIntern(IntPtr csharpAd, int type, bool isRewarded);

		[DllImport("__Internal")]
		private static extern void ShowIntern(IntPtr _nativeObject);

		#endregion
	}
}
