using System;
using UnityEngine;
using System.Collections;

namespace FloVR
{
	public class FloVRManager : MonoBehaviour
	{
		public string AppId;
		public string AppSecret;

		void Awake()
		{
			if (FloVRManagerIntern.Instance != null)
				FloVRManagerIntern.Initialize(AppId, AppSecret, null);
		}

		/// <summary>
		/// Gets the current state of the FloVR manager.
		/// </summary>
		/// <value>The state.</value>
		public static SystemState State
		{
			get {
				return FloVRManagerIntern.Instance.State;
			}
		}

		/// <summary>
		/// Initialize the without the need for a MonoBehaviour.
		/// </summary>
		/// <param name="appId">App identifier.</param>
		/// <param name="appSecret">App secret.</param>
		/// <param name="stateChangeCallback">If system state is changed, this callback will be fired.</param>
		public static void Initialize(string appId, string appSecret, Action<SystemState> stateChangeCallback = null) {
			FloVRManagerIntern.Initialize(appId, appSecret, stateChangeCallback);
		}
	}
}
