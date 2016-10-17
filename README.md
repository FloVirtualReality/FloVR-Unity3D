# FloVR-Unity3D
Unity SDK for advertising in VR unity games

Integration:

1.	Step:
          a: Import flovr-X.X.X.unitypackage to the project.
          b: Clone or download the repo and copy to the Assets folder.
2.	Step:
          a.	Drag the Assets/FloVR/Prefabs/FloVRManager prefab to the first scene in your game.
          Fill out the AppId and AppSecret.
          b:	Add to the to the top of the script “using FloVR”;
          Call “FloVRManager.Initialize("YourAppId", "YourAppSecret");”.

3.	Step: Show the ads like on the following example

```
public void ShowAd()
{
	Ad newAd = new Ad(AdType.Video360, false, (AdState adState, Ad adObject) => {
		if (adState == AdState.Ready) {
			adObject.Show();
 		}
 	});
}
```
