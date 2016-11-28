using UnityEngine;
using UnityEngine.UI;

namespace FloVR {
	public class Gaze : MonoBehaviour
	{

		public AdType type;
		public bool IsRewarded;
		public float LoadTime = 3f;

		[SerializeField]
		private Text _display;

		private Ad _fetchedAd;
		private float _startTime;
		private bool _going;

		private void Awake()
		{
			_fetchedAd = new Ad(type, IsRewarded);
		}

		private void Update()
		{
			RaycastHit seen;
			Ray raydirection = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
			if (Physics.Raycast(raydirection, out seen, 1000f))
			{
				Transform objectHit = seen.transform;
				if (objectHit == transform)
				{
					if (!_going)
					{
						_startTime = Time.time;
						_display.gameObject.SetActive(true);
						_going = true;
					}
					if (LoadTime <= (Time.time - _startTime))
					{
						_fetchedAd.Show();
						return;
					}
					float remaining = (LoadTime - (Time.time - _startTime));
					_display.text = Mathf.CeilToInt(remaining).ToString();
				}
				else {
					_display.gameObject.SetActive(false);
					_startTime = Time.time;
					_going = false;
				}
			}
			else {
				_display.gameObject.SetActive(false);
				_startTime = Time.time;
				_going = false;
			}
		}
	}
}
