using UnityEngine;
using System.Collections;

public class MainLoaderScript : MonoBehaviour {
	
	private bool isMobilePlatform = false;
	private static RuntimePlatform platform;
	private GUISkin skin;
	private GUIResolutionHelper resolutionHelper;
	GUIStyle style;
	public Texture2D swipeLeftIcon;
	public Texture2D swipeRightIcon;
	private Rect swipeLeftRect;
	private Rect swipeRightRect;
	public Texture2D swipeTouchIcon;
	private Rect swipeTouchRect;


	//the interval to show hide the icons/draw them or not
	public float showSwipeIconsInterval = 2.0f;
	private bool showSwipeIcons = false;

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
		if (showSwipeIconsInterval > 0.0f) {
			InvokeRepeating("ChangeIconsVisibility",showSwipeIconsInterval,showSwipeIconsInterval);
		}
	}
	
	// Update is called once per frame
	void Update () {
		

	}

	void ChangeIconsVisibility() {
		showSwipeIcons = !showSwipeIcons;
	}
	
	void OnGUI() {
		
		
		GUI.skin = skin;

		/*if (style == null) {
			LoadStyle();
		}*/
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
			//style.normal.textColor = Color.black;
			GUI.Label (new Rect(width/2-120, height/2-300, 400, 50), "Swipe Left or Right to choose level!");//style
		
			if(showSwipeIcons) {

				swipeLeftRect = new Rect( width/2 - 300, height/2-200,200,80);
				GUI.DrawTexture(swipeLeftRect,swipeLeftIcon);
				
				swipeRightRect = new Rect( width/2 + 200, height/2-200,200,80);
				GUI.DrawTexture(swipeRightRect,swipeRightIcon);
				
				swipeTouchRect = new Rect( width/2 , height/2+200,200,80);
				GUI.DrawTexture(swipeTouchRect,swipeTouchIcon);
			}

		}

		GUI.matrix = svMat;
		
	}
	
	void OnDestroy() {
		CancelInvoke ("ChangeIconsVisibility");
	}
	/*void LoadStyle() {
		style = GUI.skin.GetStyle("Label");
		style.alignment = TextAnchor.MiddleLeft;
		style.font = freeTextFont;
		style.fontSize = freeTextFontSize;
		style.normal.textColor = Color.black;
	}*/
}