using UnityEngine;
using System.Collections;
using BalloonQuest;

public class MainLoaderScript : MonoBehaviour {
	
	private bool isMobilePlatform = false;
	private GUISkin skin;
	private GUIResolutionHelper resolutionHelper;
	GUIStyle style;
	public Texture2D swipeLeftIcon;
	public Texture2D swipeRightIcon;
	private Rect swipeLeftRect;
	private Rect swipeRightRect;
	public Texture2D swipeTouchIcon;
	private Rect swipeTouchRect;

	//music settings
	public Texture2D musicOnIcon;
	public Texture2D musicOffIcon;
	//sound settings
	public Texture2D soundOnIcon;
	public Texture2D soundOffIcon;

	private bool accelerometerOn = false;

	private	Rect musicRect;
	private	Rect soundRect;
	private bool musicOn = true;
	private bool soundOn = true;


	//the interval to show hide the icons/draw them or not
	public float showSwipeIconsInterval = 2.0f;
	private bool showSwipeIcons = false;

	private bool isSettingsVisible = true;

	// Use this for initialization
	void Start () {
		isMobilePlatform = (Application.platform == RuntimePlatform.IPhonePlayer) || (Application.platform == RuntimePlatform.Android);
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
		//resolutionHelper.CheckScreenResolution ();
		checkSoundSettings();
		checkMusicSettings();
		CheckAccelerometerSavedSettings();

		if (showSwipeIconsInterval > 0.0f) {
			InvokeRepeating("ChangeIconsVisibility",showSwipeIconsInterval,showSwipeIconsInterval);
		}
	}

	void CheckAccelerometerSavedSettings() {
		int useAccelerometer = PlayerPrefs.GetInt (GameConstants.ACCELEROMETER_SETTINGS_KEY, 0);
		accelerometerOn = (useAccelerometer == 0) ? false : true;
		//sprite[0] = unchecked
		//sprite[1] = checked
		SetAccelerometerSettings(accelerometerOn ? 1 : 0);
	}

	void Awake() {
		
	}

	//check if we have sound enabled/disabled
	void checkMusicSettings() {
		if (!PlayerPrefs.HasKey (GameConstants.MUSIC_SETTINGS_KEY)) {
			musicOn = true;
			PlayerPrefs.SetInt (GameConstants.MUSIC_SETTINGS_KEY, 1);
			PlayerPrefs.Save ();
		} 
		else {
			int value = PlayerPrefs.GetInt (GameConstants.MUSIC_SETTINGS_KEY, 1);
			musicOn = (value == 1);
		}
	}


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
		

			if (isMobilePlatform && Input.touches.Length ==1) {

				Touch touch = Input.touches[0];

				if(touch.phase == TouchPhase.Ended && touch.phase != TouchPhase.Canceled)  {

					Vector2 fingerPos = GetFingerPosition(touch,resolutionHelper.isWidescreen);

					if(musicRect.Contains(fingerPos)) {
						musicOn = !musicOn;
						PlayerPrefs.SetInt(GameConstants.MUSIC_SETTINGS_KEY,musicOn ? 1 : 0);
						PlayerPrefs.Save();
					}
					if (soundRect.Contains (fingerPos)) {
						soundOn = !soundOn;
						PlayerPrefs.SetInt (GameConstants.SOUND_SETTINGS_KEY, soundOn ? 1 : 0);
						PlayerPrefs.Save ();
					}



				}

			}
			if(DetectAccelerometerTouchesMobile().Equals("AccelerometerSettings")) {
				ChangeAccelerometerSettings();
			}
		

	}

	void ChangeIconsVisibility() {
		showSwipeIcons = !showSwipeIcons;
	}

	void SetAccelerometerSettings(int index) {
		GameObject obj = GameObject.FindGameObjectWithTag("AccelerometerSettings");
		SwapSpriteScript swap = obj.GetComponent<SwapSpriteScript>();
		swap.SwapSprites(index);
	} 

	void ChangeAccelerometerSettings() {

		GameObject obj = GameObject.FindGameObjectWithTag("AccelerometerSettings");
		SwapSpriteScript swap = obj.GetComponent<SwapSpriteScript>();
		//if is on, will be off after this method
		//sprite[0] = unchecked
		//sprite[1] = checked
		swap.SwapSprites(accelerometerOn ? 0 : 1 );

		int useAccelerometer = PlayerPrefs.GetInt (GameConstants.ACCELEROMETER_SETTINGS_KEY, 0);

		//swap values
		useAccelerometer = (useAccelerometer == 0) ? 1 : 0;
		accelerometerOn = !accelerometerOn;
		PlayerPrefs.SetInt (GameConstants.ACCELEROMETER_SETTINGS_KEY, useAccelerometer);
		PlayerPrefs.Save();
	}
	void OnGUI() {
		
		
		GUI.skin = skin;

		if (style == null) {
			LoadStyle();
		}
		style.normal.textColor = Color.black;
		
		Matrix4x4 svMat = GUI.matrix;//save current matrix
		
		Vector3 scaleVector = resolutionHelper.scaleVector;
		bool isWideScreen = resolutionHelper.isWidescreen;
		int width = resolutionHelper.screenWidth;
		int height = resolutionHelper.screenHeight;


		GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, scaleVector);

		if (Event.current.type == EventType.Repaint) {
			//style.normal.textColor = Color.black;

		
			if(showSwipeIcons && IsSettingsVisible()) {

				swipeLeftRect = new Rect( width/2 - 350, height/2,128,64);
				GUI.DrawTexture(swipeLeftRect,swipeLeftIcon);
				
				swipeRightRect = new Rect( width/2 + 200, height/2,128,64);
				GUI.DrawTexture(swipeRightRect,swipeRightIcon);

				//GUI.Label (new Rect(width/2-170, height/2+200, 500, 50), "Swipe Left/Right to choose level!");//style

				//GUI.Label (new Rect(width/2-160, height/2-50, 500, 50), "Swipe Left/Right to move player!");//style
				
				//swipeTouchRect = new Rect( width/2 , height/2+200,100,100);
				//GUI.DrawTexture(swipeTouchRect,swipeTouchIcon,ScaleMode.ScaleToFit);
			}


			musicRect = new Rect(width-160 ,15,128,64);
			GUI.DrawTexture(musicRect, musicOn ? musicOnIcon : musicOffIcon);

			soundRect = new Rect(width-160 ,90,128,64);
			GUI.DrawTexture(soundRect, soundOn ? soundOnIcon : soundOffIcon);

		}

		//------------------------------------

		//change sound settings?
		if (!isMobilePlatform && Event.current.type == EventType.MouseUp ) {

				if (musicRect.Contains (Event.current.mousePosition)) {
					musicOn = !musicOn;
					PlayerPrefs.SetInt (GameConstants.MUSIC_SETTINGS_KEY, musicOn ? 1 : 0);
					PlayerPrefs.Save ();
				}
				if (soundRect.Contains (Event.current.mousePosition)) {
					soundOn = !soundOn;
					PlayerPrefs.SetInt (GameConstants.SOUND_SETTINGS_KEY, soundOn ? 1 : 0);
					PlayerPrefs.Save ();
				}

				if(DetectAccelerometerTouchesDesktop().Equals("AccelerometerSettings")) {
					ChangeAccelerometerSettings();
				}
		} 

		//----------------------------------

		GUI.matrix = svMat;
		
	}

	bool IsSettingsVisible() {

	    GameObject settings = GameObject.FindGameObjectWithTag("Settings");
		isSettingsVisible = (settings!=null && settings.GetComponent<SpriteRenderer>().isVisible);
		return isSettingsVisible;
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
	void LoadStyle() {
		style = skin.GetStyle("Label");
		style.alignment = TextAnchor.MiddleLeft;
		style.font = skin.label.font;
		style.fontSize = skin.label.fontSize+10;
		style.normal.textColor = Color.black;
	}

	private string DetectAccelerometerTouchesDesktop() {


		if(Input.GetMouseButtonDown(0)){
			Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		    Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);
			
			if(hitCollider){
			  return hitCollider.transform.gameObject.tag ;
			}
		}
		return "false";
	}

	private string DetectAccelerometerTouchesMobile() {


		for (int i = 0; i < Input.touchCount; ++i) {
			if (Input.GetTouch(i).phase == TouchPhase.Began) {
				Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
				RaycastHit2D hitInfo = Physics2D.Raycast(touchPosition, Vector2.zero);
				// RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
				if(hitInfo)
				{
					return hitInfo.transform.gameObject.tag;
					 
					// Here you can check hitInfo to see which collider has been hit, and act appropriately.
				}
			}
		}
		return "false";
	}
}