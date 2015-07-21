using UnityEngine;
using System.Collections;

public class MainLoaderScript : MonoBehaviour {

    private bool isMobilePlatform = false;
	private static RuntimePlatform platform;

	private GUISkin skin;
	private GUIResolutionHelper resolutionHelper;
	GUIStyle style;
	// Use this for initialization
	void Start () {
		platform = Application.platform;
		isMobilePlatform = (platform == RuntimePlatform.IPhonePlayer) || (platform == RuntimePlatform.Android);
		skin = Resources.Load("GUISkin") as GUISkin;

		//check screen size, this was breaking, probably some place we are calling Instance
		GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
		if(scripts!=null) {
			resolutionHelper = scripts.GetComponent<GUIResolutionHelper>();
			//translationManager = scripts.GetComponent<TextLocalizationManager>();
		}
		else {
			resolutionHelper = GUIResolutionHelper.Instance;
			//handle translation language
			//translationManager = TextLocalizationManager.Instance;
			
		}
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


		/*GUI.skin = skin;


		skin.label.normal.textColor = Color.black;
		
		Matrix4x4 svMat = GUI.matrix;//save current matrix
		
		Vector3 scaleVector = resolutionHelper.scaleVector;
		bool isWideScreen = resolutionHelper.isWidescreen;
		int width = resolutionHelper.screenWidth;
		int height = resolutionHelper.screenHeight;
		
		Matrix4x4 normalMatrix;
		Matrix4x4 wideMatrix;
		//we use the center matrix for the buttons
		wideMatrix = Matrix4x4.TRS (new Vector3 ((resolutionHelper.scaleX - scaleVector.y) / 2 * width, 0, 0), Quaternion.identity, scaleVector);
		normalMatrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, scaleVector);
		
		//assign normal matrix by default
		GUI.matrix = normalMatrix;
		
		if (Event.current.type == EventType.Repaint) {
			style.normal.textColor = Color.black;
			GUI.Label (new Rect(width/2-120, height/2-300, 400, 50), "Swipe Left or Right to choose level!",style);
		}

		GUI.matrix = svMat;*/

	}

	/*
	void LoadStyle() {
		style = GUI.skin.GetStyle("Label");
		style.alignment = TextAnchor.MiddleLeft;
		style.font = freeTextFont;
		style.fontSize = freeTextFontSize;
		style.normal.textColor = Color.black;
	}*/
}
