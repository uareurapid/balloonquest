using UnityEngine;
using System.Collections;
using BalloonQuest;

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

	public Texture2D soundIcon;
	public Texture2D muteIcon;
	private	Rect soundRect;
	private bool soundOn = true;


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

	void Awake() {
		checkSoundSettings ();
	}

	//check if we have sound enabled/disabled
	void checkSoundSettings() {
		if (!PlayerPrefs.HasKey (GameConstants.SOUND_SETTINGS_KEY)) {
			soundOn = true;
			PlayerPrefs.SetInt (GameConstants.SOUND_SETTINGS_KEY, 1);
			PlayerPrefs.Save ();
		} 
		else {
			int value = PlayerPrefs.GetInt (GameConstants.SOUND_SETTINGS_KEY, 1);
			soundOn = (value == 1);
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

		if (isWideScreen) {
			GUI.matrix = wideMatrix;
		} 
		else {
			//assign normal matrix by default
			GUI.matrix = normalMatrix;
		}

		
		if (Event.current.type == EventType.Repaint) {
			//style.normal.textColor = Color.black;
			GUI.Label (new Rect(width/2-120, height/2-300, 400, 50), "Swipe Left or Right to choose level!");//style
		
			if(showSwipeIcons) {

				swipeLeftRect = new Rect( width/2 - 300, height/2-200,100,100);
				GUI.DrawTexture(swipeLeftRect,swipeLeftIcon,ScaleMode.ScaleToFit);
				
				swipeRightRect = new Rect( width/2 + 200, height/2-200,100,100);
				GUI.DrawTexture(swipeRightRect,swipeRightIcon,ScaleMode.ScaleToFit);
				
				swipeTouchRect = new Rect( width/2 , height/2+200,100,100);
				GUI.DrawTexture(swipeTouchRect,swipeTouchIcon,ScaleMode.ScaleToFit);
			}

			soundRect = new Rect(width-300 ,15,96,96);
			GUI.DrawTexture(soundRect, soundOn ? soundIcon : muteIcon,ScaleMode.ScaleToFit);

		}

		//change sound settings?
		if (!isMobilePlatform) {
			if (Event.current.type == EventType.MouseUp) {
				if (soundRect.Contains (Event.current.mousePosition)) {
					soundOn = !soundOn;
					PlayerPrefs.SetInt (GameConstants.SOUND_SETTINGS_KEY, soundOn ? 1 : 0);
					PlayerPrefs.Save ();
				}
			}
		} 
		else {
			if (Input.touches.Length ==1) {
				
				Touch touch = Input.touches[0];
				
				if(touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)  {
					
					Vector2 fingerPos = GetFingerPosition(touch,isWideScreen);
					
					if(soundRect.Contains(Event.current.mousePosition)) {
						soundOn = !soundOn;
						PlayerPrefs.SetInt(GameConstants.SOUND_SETTINGS_KEY,soundOn ? 1 : 0);
						PlayerPrefs.Save();
					}
				
				}
				
			}  //end if (Input.touches.Length ==1) 
		}


		GUI.matrix = svMat;
		
	}

	/**
	*Get the correct finger touch position
	*/
	Vector2 GetFingerPosition(Touch touch, bool isWideScreen) {
		
		
		Vector2 fingerPos = new Vector2(0,0);
		float diference = 0f;
		
		fingerPos.y =  resolutionHelper.screenHeight - (touch.position.y / Screen.height) * resolutionHelper.screenHeight;
		fingerPos.x = (touch.position.x / Screen.width) * resolutionHelper.screenWidth;
		
		return fingerPos;
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