using UnityEngine;
using System.Collections;

public class LevelChangeListener : MonoBehaviour {

	public int level = 1;
	public bool isMobilePlatform = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		int level = 0;
	  	if (isMobilePlatform) {
			level = DetectLevelTouchesMobile();
		} 
		else {
			level = DetectLevelTouchesDesktop();
		}
		if (level > 0) {
			Application.LoadLevel("Level"+level);
		}
	}


	private int DetectLevelTouchesMobile() {
		
	
		for (int i = 0; i < Input.touchCount; ++i) {
			if (Input.GetTouch(i).phase == TouchPhase.Began) {
				Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
				RaycastHit2D hitInfo = Physics2D.Raycast(touchPosition, Vector2.zero);
				// RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
				if(hitInfo)
				{
					string tag = hitInfo.transform.gameObject.tag;
					if(tag.Contains("start") ) {
						string level = tag.Substring(tag.Length-1);
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
