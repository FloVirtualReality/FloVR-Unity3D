using UnityEngine;

namespace FloVR.AdTypes
{
	public class GearVRAd : AndroidJavaProxy
	{
		private AndroidJavaObject _nativeObject;

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
				return _nativeObject.Call<bool>("IsShown");
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
		public GearVRAd(AdType type, bool isRewarded = false) : base("com.flovr.sdk.CSAd")
		{
			State = AdState.Initializing;
			_nativeObject = new AndroidJavaObject("com.flovr.sdk.Ad", this, (int)type, isRewarded);
		}

		/// <summary>
		/// Show the ad if loaded and not shown.
		/// </summary>
		public void Show()
		{
			State = AdState.Running;
			_nativeObject.Call("Show");
			FloVRManagerIntern.Instance.PauseUnityPlayer();
		}

		/// <summary>
		/// Called from JNI. Please don't call this function.
		/// </summary>
		public void OnAdFinished(string result)
		{
			State = AdState.Shown;
			FloVRManagerIntern.Instance.ResumeUnityPlayer();
		}

		/// <summary>
		/// Called from JNI. Please don't call this function.
		/// </summary>
		public void OnError(string error)
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
	}
}