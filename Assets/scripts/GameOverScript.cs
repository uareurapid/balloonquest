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
		
		initialTime = 0f;
		isShowingMessage = true;

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

					homeTextureRect = new Rect( width/2 - 80, height/2+160,128,64);
				    //resumeTextureRect = new Rect(width / 2-100,height-500,200,80);
					//missionsTextureRect = new Rect(width / 2-100,height -400,200,80);
					//achievementsRect = new Rect(width / 2-100,height -300,200,80);
					//creditsTextureRect = new Rect(width / 2-100,height-200,200,80);


					//}

					GUI.DrawTexture(homeTextureRect,homeTexture);
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

					//default is not blink, the blink var will only be set to true if we have a best score
					//
					if (!blinkOnBestScore) { //Just write it there!
					
						GUI.Label (new Rect (width / 2 - 45, height / 2 - 95, 300, 50), best < 10 ? " " + best : best.ToString (), style);
					}//else do not print it
					
					
					    

			}//end repaint
			
			
		//********************* CLICK / TOUCH CHECKS *******************
				//desktop checks
		if(Event.current.type == EventType.MouseUp && !isMobilePlatform) {

			Vector2 mousePosition = Event.current.mousePosition;

			    if(homeTextureRect.Contains(mousePosition) )
				{
				    PlaySettingsSound();
					Application.LoadLevel("Main");
				}
				else if(resumeTextureRect.Contains(mousePosition) )
				{
					Application.LoadLevel("Main");
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

				if(homeTextureRect.Contains(fingerPos) )
				{
					PlaySettingsSound();
					Application.LoadLevel("Main");
				}
				else if(resumeTextureRect.Contains(fingerPos) )
				{
				   //just resume the world, not the level
					Application.LoadLevel("Main");
				}

			}
		}

		GUI.skin.label.normal.textColor = UnityEngine.Color.black;
			
		//restore the matrix	
		GUI.matrix = svMat;	
				   
	  
		

	}

	void PlaySettingsSound() {
	  GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
	  if(scripts!=null) {
	   SoundEffectsHelper fx = scripts.GetComponentInChildren<SoundEffectsHelper>();
	   fx.PlaySettingsSound();
	  }
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
		HideGameOverBoard ();
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
