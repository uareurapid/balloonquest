using UnityEngine;
using System.Collections;
//using UnityEngine.Advertisements;
using MrBalloony;

public class SimpleAdsScript : MonoBehaviour {
	
	void Start () 
	{

		//Advertisement.Initialize (GameConstants.UNITY_ADS_ANDROID_GAME_ID, true);
		
		//StartCoroutine (ShowAdWhenReady());
	}
	
	IEnumerator ShowAdWhenReady()
	{
		//while (!Advertisement.isReady())
			yield return null;
		
		//Advertisement.Show ();
	}

	// Update is called once per frame
	//void Update () {
		
	//}
}