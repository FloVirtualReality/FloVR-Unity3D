using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace FloVR
{
	public class Preroller : MonoBehaviour
	{
		void Awake()
		{
			AsyncOperation sceneLoading = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
			sceneLoading.allowSceneActivation = false;
			Ad newAd = new Ad(AdType.Video360, false, (AdState adState, Ad adObject) =>
			{
				if (adState == AdState.Ready) {
					adObject.Show();
				}
				else if (adState == AdState.Shown) {
					sceneLoading.allowSceneActivation = true;
				}
			});
		}
	}
}