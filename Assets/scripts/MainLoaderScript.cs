using UnityEngine;
using System.Collections;

public class MainLoaderScript : MonoBehaviour {

    private bool isMobilePlatform = false;
	private RuntimePlatform platform;
	// Use this for initialization
	void Start () {
		isMobilePlatform = (platform == RuntimePlatform.IPhonePlayer) || (platform == RuntimePlatform.Android);
	}
	
	// Update is called once per frame
	void Update () {
	
	  int level = 0;
	  if(isMobilePlatform)
	    level = DetectLevelTouches();
	  else
	    level = DetectDesktopLevelTouches(); 
	    
	  if(level!=0) {
	    Application.LoadLevel("Level" + level);
	  } 
	}
	
	private int DetectLevelTouches() {

		
		for (int i = 0; i < Input.touchCount; ++i) {
			if (Input.GetTouch(i).phase == TouchPhase.Began) {
				Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
				RaycastHit2D hitInfo = Physics2D.Raycast(touchPosition, Vector2.zero);
				// RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
				if(hitInfo)
				{
					string tag = hitInfo.transform.gameObject.tag;
					if(tag!=null && tag.Contains("start")) {
					
					
						return int.Parse(tag.Substring(tag.Length-1));
					}
					// Here you can check hitInfo to see which collider has been hit, and act appropriately.
				}
				
			}
		}
		return 0;
	}
	
	//desktop click on Jelly
	private int DetectDesktopLevelTouches() {
		
	
		if(Input.GetMouseButtonDown(0)){
			Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);
			
			if(hitCollider){
				string tag = hitCollider.transform.gameObject.tag;
				if(tag!=null && tag.Contains("start")) {
					return int.Parse(tag.Substring(tag.Length-1));
				}
				
			}
			
		}
		return 0;
	}
	
	void OnGUI() {
		if (GUI.Button(new Rect(10,10,50,50),"Level1")) {
			Application.LoadLevel("Level1");
		}
		if (GUI.Button(new Rect(10,70,50,30),"Level2")) {
			Application.LoadLevel("Level2");
		}
		if (GUI.Button(new Rect(10,130,50,30),"Level3")) {
			Application.LoadLevel("Level3");
		}
		if (GUI.Button(new Rect(10,160,50,30),"Level4")) {
			Application.LoadLevel("Level4");
		}
		if (GUI.Button(new Rect(10,190,50,30),"Level5")) {
			Application.LoadLevel("Level5");
		}
		if (GUI.Button(new Rect(10,220,50,30),"Level6")) {
			Application.LoadLevel("Level6");
		}
		if (GUI.Button(new Rect(10,250,50,30),"Level7")) {
			Application.LoadLevel("Level7");
		}
		if (GUI.Button(new Rect(10,280,50,30),"Level8")) {
			Application.LoadLevel("Level8");
		}
	}
}
