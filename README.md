# FloVR-Unity3D

##Overview
Unity SDK for advertising in VR unity games

##Integration

###I. Import

	Import flovr-X.X.X.unitypackage to the project.
	
OR
	
	Clone or download the repo and copy to the Assets folder.
###II. Initialize

	Drag the Assets/FloVR/Prefabs/FloVRManager prefab to the first scene in your game. Fill out the AppId and AppSecret.
		
OR
	
	Add to the following code to your game:
	 
```C#
	using FloVR;

	public void Awake() 
	{
		FloVRManager.Initialize("YourAppId", "YourAppSecret");
		// Other stuff you want to do
	}
```
###III. Show
```C#
public void ShowAd()
{
	Ad newAd = new Ad(AdType.Video360, false, (AdState adState, Ad adObject) => {
		if (adState == AdState.Ready) {
			adObject.Show();
 		}
 	});
}
```
