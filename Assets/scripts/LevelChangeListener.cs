using UnityEngine;
using System.Collections;
using BalloonQuest;

public class LevelChangeListener : MonoBehaviour {

	public int level = 1;
	private bool isMobilePlatform = false;
	private bool levelLocked = true;
	private bool loading = false;

	// Use this for initialization
	void Start () {

		isMobilePlatform = (Application.platform == RuntimePlatform.IPhonePlayer) || (Application.platform == RuntimePlatform.Android);

		/*if (level > 1) {
			bool hasKey = PlayerPrefs.HasKey(GameConstants.UNLOCKED_LEVEL_KEY + level);
			levelLocked = !hasKey;
		} 
		else {*/
			levelLocked = false;
		//}
	}
	
	// Update is called once per frame
	void Update () {
	
		int levelTouched = 0;
	  	if (isMobilePlatform) {
			levelTouched = DetectLevelTouchesMobile();
		} 
		else {
			levelTouched = DetectLevelTouchesDesktop();
		}

		if ( (levelTouched > 0 && levelTouched==level) && !loading && !levelLocked) {

			loading = true;
			Application.LoadLevel ("Level" + levelTouched);
		
		}	

	}


	private int DetectLevelTouchesMobile() {
		
	
		for (int i = 0; i < Input.touchCount; ++i) {

			Touch touch = Input.GetTouch(i);
			if (touch.phase == TouchPhase.Ended && touch.tapCount==1) {
				Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
				RaycastHit2D hitInfo = Physics2D.Raycast(touchPosition, Vector2.zero);
				// RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
				if(hitInfo)
				{
					string tag = hitInfo.transform.gameObject.tag;
					if(tag.Contains("start") ) {
						string level = tag.Substring(tag.Length-1);
						return int.Parse(level);
					}
					// Here you can check hitInfo to see which collider has been hit, and act appropriately.
				}

			}
		}
		return 0;
	}
	
	//desktop click on button
	private int DetectLevelTouchesDesktop() {

		
		if(Input.GetMouseButtonDown(0)){
			Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);
			
			if(hitCollider){
				string tag = hitCollider.transform.gameObject.tag;
				if(tag.Contains("start")) {
					string level = tag.Substring(tag.Length-1);
					return int.Parse(level);
				}
				
			}
			
			
		}
		return 0;
	}
}
