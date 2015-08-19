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
	Texture2D homeTexture ;
	//Texture2D exitTexture ;
	//Texture2D achievementsTexture;
	//Texture2D creditsTexture ;
	//Texture2D missionsTexture ;

	//Texture2D storeTexture;
	Rect storeTextureRect;

	Rect homeTextureRect ;
	//Rect exitTextureRect ;
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
	private bool blinkOnBestScore = false;
	private int blinkedTimes = 0;
	private int showingMessageTimes = 0;


	private GUIResolutionHelper resolutionHelper;



	private bool scoresHidden = false; 

	void Start() {
	
		// Load a skin for the buttons
		skin = Resources.Load("GUISkin") as GUISkin;
		//exitTexture = Resources.Load("menu") as Texture2D;
		homeTexture = Resources.Load("home") as Texture2D;

		//exitTexture = Resources.Load("button_quit") as Texture2D;
		//achievementsTexture = Resources.Load("button_achievements") as Texture2D;
		//creditsTexture = Resources.Load("button_credits") as Texture2D;
		//missionsTexture = Resources.Load("button_missions") as Texture2D;
		//storeTexture = Resources.Load("store") as Texture2D;

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
		//resolutionHelper.CheckScreenResolution();

		initialTime = 0f;
		isShowingMessage = true;
		scoresHidden = false;

				//handle translation language
		//translationManager = TextLocalizationManager.Instance;
		//translationManager.LoadSystemLanguage(Application.systemLanguage);
		platform = Application.platform;
		isMobilePlatform = (platform == RuntimePlatform.IPhonePlayer || platform == RuntimePlatform.Android || platform == RuntimePlatform.BlackBerryPlayer);

		//if(!settingsScene) {
		Invoke("PauseGame", 4f);
		//}

		//show them to the user
		ShowGameOverBoard ();
		
	}
	
	void Awake() {


		CheckInAppPurchases();


	}

	void ClearPlayerPrefs() {

		//put them all to zero
	

	}
	
	void CheckInAppPurchases() {
	  
	}

	//show the game over panel (it is hidden at start, and will be hidden by game controller afterwards)
	void ShowGameOverBoard() {
		GameObject gameOver = GameObject.FindGameObjectWithTag ("GameOver");
		if (gameOver != null) {
			SpriteRenderer spr = gameOver.GetComponent<SpriteRenderer>();
			if(spr!=null) {
				spr.enabled = true;
			}

			CircleCollider2D collider = gameOver.GetComponent<CircleCollider2D>();
			if(collider!=null) {
				collider.enabled = true;
			}
		}
		
		//TODO, this should be done on a score script
		int gameScore = PlayerPrefs.GetInt (GameConstants.HIGH_SCORE_KEY, 1);
		int previousBestScore = PlayerPrefs.GetInt (GameConstants.BEST_SCORE_KEY, 1);
		
		if (gameScore == previousBestScore) {
			//we have a new best score
			GameObject newBest = GameObject.FindGameObjectWithTag("NewBestScore");
			if(newBest!=null) {
				newBest.GetComponent<SpriteRenderer>().enabled = true;
			}
			//blinkOnBestScore = true;
			InvokeRepeating ("BlinkBestScore", 0.2f, 0.3f);
		}
		
		
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
			if(showingMessageTimes>=3) {
				//just blink 3 times
				isShowingMessage = true;//keep showing
			}
			else {
				//just swap
				isShowingMessage = !isShowingMessage;
			}
			showingMessageTimes+=1;
			
		}

		//desktop?
		if(!isMobilePlatform) {

			if(!scoresHidden && (DetectReplayTouchesDesktop() || DetectCloseGameOverTouchesDesktop()) ) {
					HideScores();
					PlayReplaySound();
					HideGameOver();
					ReloadCurrentLevel();
					
					
			}
		}
		else {
		//mobile
			if(!scoresHidden && (DetectReplayTouchesMobile() || DetectCloseGameOverTouchesMobile()) ) {
				HideScores();
				PlayReplaySound();
				HideGameOver();
				ReloadCurrentLevel();
				
			}
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
		
	    int width = resolutionHelper.screenWidth;
		int height = resolutionHelper.screenHeight;
		Vector3 scaleVector = resolutionHelper.scaleVector;
		
		bool isWideScreen = resolutionHelper.isWidescreen;

		//Matrix4x4 wideMatrix = Matrix4x4.TRS(new Vector3( (resolutionHelper.scaleX - scaleVector.y) / 2 * width, 0, 0), Quaternion.identity, scaleVector);
		Matrix4x4 normalMatrix = Matrix4x4.TRS(Vector3.zero,Quaternion.identity,scaleVector);
		
		//if(isWideScreen) {
		//	GUI.matrix =  wideMatrix;
			
			
		//}
		//else {
			GUI.matrix = normalMatrix;
			
		//}


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
						GUI.Label (new Rect(width/2-140, height/2-300, 300, 50), "Game Over!!!",style);
			       }



					//*******************************

					homeTextureRect = new Rect( width/2 - 80, height/2+160,96,96);
					GUI.DrawTexture(homeTextureRect,homeTexture);
				
					int score = PlayerPrefs.GetInt (GameConstants.HIGH_SCORE_KEY,1);
					int best = PlayerPrefs.GetInt (GameConstants.BEST_SCORE_KEY,1);
					//try to keep the scores aligned
					if(!scoresHidden) {
						GUI.Label (new Rect(width/2-45, height/2-160, 300, 50), score < 10 ? " "+ score : score.ToString(),style);
					}


					//default is not blink, the blink var will only be set to true if we have a best score
					//
					if (!blinkOnBestScore && !scoresHidden) { 
						//Just write it there!
						GUI.Label (new Rect (width / 2 - 45, height / 2 - 95, 300, 50), best < 10 ? " " + best : best.ToString (), style);
					}//else do not print it
					
					
					    

			}//end repaint
			
			
		//********************* CLICK / TOUCH CHECKS *******************

	
		if(Event.current.type == EventType.MouseUp && !isMobilePlatform) {

			Vector2 mousePosition = Event.current.mousePosition;

			    if(homeTextureRect.Contains(mousePosition) )
				{
					PlaySettingsSoundAndLoadMain ();
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


				if(resolutionHelper.isWidescreen) {
				//do extra computation
					fingerPos.x = fingerPos.x + (resolutionHelper.scaleX - resolutionHelper.scaleVector.y) / 2 * width;
				}

				if(homeTextureRect.Contains(fingerPos) )
				{
					PlaySettingsSoundAndLoadMain();
				}
				//else if(resumeTextureRect.Contains(fingerPos) )
				//{
				   //just resume the world, not the level
					Application.LoadLevel("Main");
				//}

			}
		}

		GUI.skin.label.normal.textColor = UnityEngine.Color.black;
			
		//restore the matrix	
		GUI.matrix = svMat;	
				   
	  
		

	}
	void PlaySettingsSoundAndLoadMain() {
	
		//we need a hack for PlayClipAtPoint(...)
		if (Time.timeScale == 0f) {
			//already paused the app
			Time.timeScale = 1;
			PlaySettingsSound();
			Time.timeScale = 0f;
		} 
		else {
			//still running, can play immediatelly
			PlaySettingsSound();

		}

		Application.LoadLevel ("Main");
	}
	//hide it and disable touches
	void HideGameOver() {
		GameObject gameOver = GameObject.FindGameObjectWithTag ("GameOver");
		if (gameOver != null) {
			SpriteRenderer spr = gameOver.GetComponent<SpriteRenderer>();
			if(spr!=null) {
				Debug.Log("HIDE GAME OVER");
				spr.enabled = false;
			}
			
			CircleCollider2D collider = gameOver.GetComponent<CircleCollider2D>();
			if(collider!=null) {
				collider.enabled = false;
			}
		}
		/*GameOverScript script = FindObjectOfType<GameOverScript>();
		if (script != null) {
			Destroy(script.gameObject);
		}*/

	}
	//buttons and sounds related stuff
	void PlayReplaySound() {
		GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
		if(scripts!=null) {
			SoundEffectsHelper fx = scripts.GetComponentInChildren<SoundEffectsHelper>();
			if(Time.timeScale==0) { //need this hack if game is already paused
				Time.timeScale = 1;
				fx.PlayReplaySound();
				Time.timeScale = 0;
			}
			else {
				//just play normally
				fx.PlayReplaySound();
			}


		}
	}
	


	void HideScores() {

	  scoresHidden = true;

	  GameObject currentScore = GameObject.FindGameObjectWithTag("NewBestScore");
	  if(currentScore!=null) {
		currentScore.GetComponent<SpriteRenderer>().enabled = false;
	  }
	  GameObject bestScore = GameObject.FindGameObjectWithTag("NewBestScore2");
	  if(bestScore!=null) {
	    bestScore.GetComponent<SpriteRenderer>().enabled = false;
	  }

	}



	void PlaySettingsSound() {

	  GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
	  if (scripts != null) {
			SoundEffectsHelper fx = scripts.GetComponentInChildren<SoundEffectsHelper> ();
			fx.PlaySettingsSound ();
	  } 
	}

	private bool DetectReplayTouchesDesktop() {
		
		
		if(Input.GetMouseButtonDown(0)){

		    Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Collider2D hitCollider = Physics2D.OverlapPoint(worldPosition);
			
			if(hitCollider){
			Debug.Log("YES!!!! " + hitCollider.transform.gameObject.tag);
				return hitCollider.transform.gameObject.tag.Equals("GameOver") ;
			}
		}
		return false;
	}
	
	private bool DetectReplayTouchesMobile() {
		
		
		for (int i = 0; i < Input.touchCount; ++i) {
			if (Input.GetTouch(i).phase == TouchPhase.Began) {
				Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
				RaycastHit2D hitInfo = Physics2D.Raycast(touchPosition, Vector2.zero);
				// RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
				if(hitInfo)
				{
					return hitInfo.transform.gameObject.tag.Equals("GameOver") ;
					
					// Here you can check hitInfo to see which collider has been hit, and act appropriately.
				}
			}
		}
		return false;
	}
	
	private bool DetectCloseGameOverTouchesDesktop() {
		
		
		if(Input.GetMouseButtonDown(0)){
			Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Collider2D hitCollider = Physics2D.OverlapPoint(worldPosition);
			
			if(hitCollider){
				return hitCollider.transform.gameObject.tag.Equals("GameOverClose") ;
				
			}
		}
		return false;
	}
	
	private bool DetectCloseGameOverTouchesMobile() {
		
		
		for (int i = 0; i < Input.touchCount; ++i) {
			if (Input.GetTouch(i).phase == TouchPhase.Began) {
				Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
				RaycastHit2D hitInfo = Physics2D.Raycast(touchPosition, Vector2.zero);
				// RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
				if(hitInfo)
				{
					return hitInfo.transform.gameObject.tag.Equals("GameOverClose") ;
					
					// Here you can check hitInfo to see which collider has been hit, and act appropriately.
				}
			}
		}
		return false;
	}

	void BlinkBestScore() {
		//only blink 5 times maximum
		if (blinkedTimes >= 5) {
			blinkOnBestScore = false;
		} 
		else {
			blinkOnBestScore = !blinkOnBestScore;
		}
		blinkedTimes += 1;
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

		blinkOnBestScore = false;
		CancelInvoke("BlinkBestScore");
		//HideGameOverBoard ();
	}

	void ReloadCurrentLevel() {
		GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
		GameControllerScript controller = scripts.GetComponent<GameControllerScript>();
		currentLevel = controller.GetCurrentLevel();
		Application.LoadLevel("Level"+currentLevel);
	}

	void HideGameOverBoard() {
		GameObject gameOver = GameObject.FindGameObjectWithTag ("GameOver");
		if (gameOver != null) {
			SpriteRenderer spr = gameOver.GetComponent<SpriteRenderer>();
			if(spr!=null) {
				spr.enabled = false;
			}
		}
		
		
		//we have a new best score showing?
		GameObject newBest = GameObject.FindGameObjectWithTag("NewBestScore");
		if(newBest!=null) {
			newBest.GetComponent<SpriteRenderer>().enabled = false;
		}


		
	}
	
	private bool IsShowingGameOverPanel() {
		GameObject gameOver = GameObject.FindGameObjectWithTag ("GameOver");
		if (gameOver != null) {
			SpriteRenderer spr = gameOver.GetComponent<SpriteRenderer> ();
			if (spr != null) {
				return spr.enabled;
			}
			
		}
		return false;
	}

	/*private SpriteRenderer GetNewBestScoreSprite() {
		//we have a new best score showing?
		GameObject newBest = GameObject.FindGameObjectWithTag("NewBestScore2");
		if(newBest!=null) {
			return newBest.GetComponent<SpriteRenderer>();
		}
		return null;
	}*/
}
