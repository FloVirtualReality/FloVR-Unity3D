using System;
using FloVR.AdTypes;
using UnityEngine;

namespace FloVR
{
	public class Ad : GearVRAd
	{
		private Action<AdState, Ad> _callback;

		public Ad(AdType type, bool isRewarded = false, Action<AdState, Ad> callback = null) : base( type, isRewarded)
		{
			_callback = callback;
		}

		public override AdState State
		{
			get
			{
				return base.State;
			}
			protected set
			{
				base.State = value;
				if (_callback != null)
					_callback(value, this);

			}
		} 
	}

	public enum AdState
	{
		Initializing,
		Ready,
		Running,
		Shown
	}

	public enum AdType
	{
		Interactive = 0,
		Video360 = 1
	}
}