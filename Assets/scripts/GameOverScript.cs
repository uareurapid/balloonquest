using UnityEngine;
using UnityEngine.SocialPlatforms;
using BalloonQuest;
using System.Collections;


/// <summary>
/// Start or quit the game
/// </summary>
public class GameOverScript : MonoBehaviour
{

	//custom GUI skin
	private GUISkin skin;
	public Font freeTextFont;
	public int freeTextFontSize = 40;
	GUIStyle style;
	float initialTime = 0f;
	float interval = 2f;
	bool isShowingMessage = false;
	const int buttonWidth = 170;
	const int buttonHeight = 60;

	//Texture2D resumeTexture ;
	Texture2D startTexture ;
	Texture2D exitTexture ;
	//Texture2D achievementsTexture;
	//Texture2D creditsTexture ;
	//Texture2D missionsTexture ;

	//Texture2D storeTexture;
	Rect storeTextureRect;

	Rect startTextureRect ;
	Rect exitTextureRect ;
	Rect achievementsRect;
	Rect creditsTextureRect ;
	Rect missionsTextureRect ;
	Rect resumeTextureRect ;

	//GameControllerScript controller;
	int currentLevel = 1;
	int currentWorld = 1;

	//showGameName used on own SettingsScene
	public bool settingsScene = false;
	private bool isMobilePlatform = false;
	private static RuntimePlatform platform;

	private TextLocalizationManager translationManager;

	private bool showStore = false;

	void Start() {
	
		// Load a skin for the buttons
		skin = Resources.Load("GUISkin") as GUISkin;
		//exitTexture = Resources.Load("menu") as Texture2D;
		startTexture = Resources.Load("button_playstart") as Texture2D;

		exitTexture = Resources.Load("button_quit") as Texture2D;
		//achievementsTexture = Resources.Load("button_achievements") as Texture2D;
		//creditsTexture = Resources.Load("button_credits") as Texture2D;
		//missionsTexture = Resources.Load("button_missions") as Texture2D;
		//storeTexture = Resources.Load("store") as Texture2D;
		
		initialTime = 0f;
		isShowingMessage = true;

				//handle translation language
		//translationManager = TextLocalizationManager.Instance;
		//translationManager.LoadSystemLanguage(Application.systemLanguage);

		isMobilePlatform = (platform == RuntimePlatform.IPhonePlayer || platform == RuntimePlatform.Android || platform == RuntimePlatform.BlackBerryPlayer);

		//if(!settingsScene) {
			Invoke("PauseGame", 4f);
		//}


		
	}
	
	void Awake() {


		CheckInAppPurchases();


	}

	void ClearPlayerPrefs() {

		//put them all to zero
	

	}
	
	void CheckInAppPurchases() {
	  
	}

	
	void LoadStyle() {
		style = GUI.skin.GetStyle("Label");
		style.alignment = TextAnchor.MiddleLeft;
		style.font = freeTextFont;
		style.fontSize = freeTextFontSize;
		style.normal.textColor = Color.black;
	}

    void Update() {
    
		initialTime += Time.deltaTime;
		
		
		//blink game over message every 2 seconds
		if(initialTime >= interval ) {
			
			initialTime = 0f;
			isShowingMessage = !isShowingMessage;
			
		}
    
    }
    //Load next scene, showing an activity indicator



	void OnGUI()
	{
		
				
	  
		if(style==null) {
			LoadStyle();
		}
		// Set the skin to use
		GUI.skin = skin;

		//increase the font size
		style.fontSize = freeTextFontSize + 20;
		
		Matrix4x4 svMat = GUI.matrix;//save current matrix
		
	    int width = GUIResolutionHelper.Instance.screenWidth;
		int height = GUIResolutionHelper.Instance.screenHeight;
		Vector3 scaleVector = GUIResolutionHelper.Instance.scaleVector;
		
		bool isWideScreen = GUIResolutionHelper.Instance.isWidescreen;
		
		if(isWideScreen) {
			GUI.matrix = Matrix4x4.TRS(new Vector3( (GUIResolutionHelper.Instance.scaleX - scaleVector.y) / 2 * width, 0, 0), Quaternion.identity, scaleVector);
			
			
		}
		else {
			GUI.matrix = Matrix4x4.TRS(Vector3.zero,Quaternion.identity,scaleVector);
			
		}


		   // bool showNextLevel = false;


			GameObject playerPlaying = GameObject.FindGameObjectWithTag ("Player");
			//means player is dead
			bool playerAlive = false;
			if(playerPlaying!=null) {
			 PlayerScript playerScript = playerPlaying.GetComponent<PlayerScript>();
			 playerAlive = playerScript!=null && playerScript.IsPlayerAlive();
			}



			
			
			if(Event.current.type==EventType.Repaint) {


			       if(isShowingMessage) {

						style.normal.textColor = Color.black;
						GUI.Label (new Rect(width/2-120, height/2-300, 300, 50), "Game Over!!!",style);
			       }



					//*******************************

					startTextureRect = new Rect( width/2 - 100, height/2+160,200,80);
				    //resumeTextureRect = new Rect(width / 2-100,height-500,200,80);
					//missionsTextureRect = new Rect(width / 2-100,height -400,200,80);
					//achievementsRect = new Rect(width / 2-100,height -300,200,80);
					//creditsTextureRect = new Rect(width / 2-100,height-200,200,80);


					//}

					GUI.DrawTexture(startTextureRect,startTexture);
					//GUI.DrawTexture(missionsTextureRect,missionsTexture);
					//GUI.DrawTexture(achievementsRect,achievementsTexture);
					//GUI.DrawTexture(creditsTextureRect,creditsTexture);

					//GUI.DrawTexture(resumeTextureRect,resumeTexture);

					//if(!settingsScene && showStore) {
					//	storeTextureRect = new Rect(width -110,30,96,96);
					//    GUI.DrawTexture(storeTextureRect,storeTexture);
					//}

					int score = PlayerPrefs.GetInt (GameConstants.HIGH_SCORE_KEY,1);
					int best = PlayerPrefs.GetInt (GameConstants.BEST_SCORE_KEY,1);
					//try to keep the scores aligned
					GUI.Label (new Rect(width/2-45, height/2-160, 300, 50), score < 10 ? " "+ score : score.ToString(),style);
					GUI.Label (new Rect(width/2-45, height/2-95, 300, 50),best < 10 ? " " + best : best.ToString(),style);
					    

			}//end repaint
			
			
		//********************* CLICK / TOUCH CHECKS *******************
				//desktop checks
		if(Event.current.type == EventType.MouseUp && !isMobilePlatform) {

			Vector2 mousePosition = Event.current.mousePosition;

			    if(startTextureRect.Contains(mousePosition) )
				{
					LoadNextLevel(1);
				}
				else if(resumeTextureRect.Contains(mousePosition) )
				{
				  LoadNextLevel(1);
				}

		}
		//mobile checks
		else if(isMobilePlatform && Input.touchCount == 1 )
		{

			Touch touch = Input.touches[0];
			if(touch.phase == TouchPhase.Began) {

				Vector2 fingerPos = new Vector2(0,0);
				fingerPos = touch.position;
				
				fingerPos.y =  height - (touch.position.y / Screen.height) * height;
				fingerPos.x = (touch.position.x / Screen.width) * width;


				if(GUIResolutionHelper.Instance.isWidescreen) {
				//do extra computation
					fingerPos.x = fingerPos.x + (GUIResolutionHelper.Instance.scaleX - GUIResolutionHelper.Instance.scaleVector.y) / 2 * width;
				}

				if(startTextureRect.Contains(fingerPos) )
				{
					LoadNextLevel(1);
				}
				else if(resumeTextureRect.Contains(fingerPos) )
				{
				   //just resume the world, not the level
				   LoadNextLevel(1);
				}

			}
		}

		GUI.skin.label.normal.textColor = UnityEngine.Color.black;
			
		//restore the matrix	
		GUI.matrix = svMat;	
				   
	  
		

	}


	IEnumerator ShowCredits() {
 
		yield return new WaitForSeconds(3f);
		Application.LoadLevel("CreditsScene");
	}


	string GetTranslationKey(string key) {
		return	translationManager.GetText(key);
	}
	
	//this needs to be called about 3 seconds after showing something
	private void PauseGame() {
		Time.timeScale = 0f; 
	}
	
	void LoadNextLevel(int level) {

		//StartActivityMonitor();
		Application.LoadLevel("Level" + level);
		Destroy(gameObject);
	}
	
	void LoadFinalScene(string sceneName) {
		//StartActivityMonitor();
		Application.LoadLevel("FinalScene");
		Destroy(gameObject);
	}
	
	//Draw text on screen
	public void DrawText(string text, int fontSize) {
		
		
		GUIStyle centeredStyleSmaller = GUI.skin.GetStyle("Label");
		centeredStyleSmaller.alignment = TextAnchor.MiddleLeft;
		centeredStyleSmaller.font = freeTextFont;
		centeredStyleSmaller.fontSize = fontSize;
		
		GUI.Label (new Rect(Screen.width/2-150, Screen.height/2 -50, 400, 50), text);
	}
	
	//Draw text on screen
	public void DrawText(string text, int fontSize, int x, int y, int width, int height) {
		
		
		GUIStyle centeredStyleSmaller = GUI.skin.GetStyle("Label");
		centeredStyleSmaller.alignment = TextAnchor.MiddleLeft;
		centeredStyleSmaller.font = freeTextFont;
		centeredStyleSmaller.fontSize = fontSize;
		
		GUI.Label (new Rect(x, y, width, height), text);
	}
	


	
	void OnDestroy() {

	}

}
