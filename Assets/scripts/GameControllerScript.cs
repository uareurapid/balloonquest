using UnityEngine;
using System.Collections;
using BalloonQuest;

public class GameControllerScript : MonoBehaviour {

	//flaby_alien_level_two
	private static RuntimePlatform platform;
	private bool isMobilePlatform = false;
	private static GameControllerScript instance;
	
	//number of energy star pickups spread in level
	public int numberEnergyPickups = 20;

	public Texture2D pauseIcon;
	public Texture2D playIcon;

	public Texture2D backButton;
	public Texture2D forwardButton;
	public Texture2D JumpButton;

	private Texture2D rateTexture;
	private Texture2D exitTexture;

	
	public int screenWidth;
	public int screenHeight;
	
	Rect pausePlayRect;
	Rect exitTextureRect;
	Rect leaderboardsRect;
	Rect rateRect;
	Rect forwardRect;
	Rect backRect;
    Rect jumpRect;
	
	public bool isGamePaused = true;
	private bool isGameStarted = false;
	private bool isGameOver = true;
	private bool isGameComplete = false;
	private bool isLevelComplete = false;

	private GUITexture redTexture;
	
	//public bool isRestart = false;
	
	public Font messagesFont;
	public int messagesFontSizeSmaller;
	public int messagesFontSizeLarger;
	
	//when hurry up, increase scrolling speed of platforms by 1.8
	const float HURRY_UP_SPEED_INCREASE_FACTOR = 1.8f;
	
	//whne to hurry up 1/4 of total mission seconds
	const float HURRY_UP_START_FACTOR = 0.25f;
	
	
	private bool appliedHurryUpFactor = false;
	
	private string hurryUpMessage = "Hurry Up!";
    private float initialHurryUpMessageTime = 0f;
	private bool isShowingHurryUpMessage = false;
    private bool hasMovedSpikesLine = false;
	
	//controll first level howTo

	private float lastHowToTime = 0f;
	private float initialHowToTime = 0f;
	private bool isShowingHowTo = false;

	private GUISkin skin;
	
	public int numWorlds = 4;
	public int numberOfLevels = 10;
	public int currentLevel = 1;
	public int currentWorld = 1;
	//number of minutes to complete the mission


	//display time

	public int elapsedMissionMeters = 1000;
	
	//show in app for level xxx?
	private bool showUnlockLevel = true;
	//to control if we already released the Ground
	private bool movedGround = false;



	
	//Iads only
//	private ADBannerView banner = null;
	
	//#if UNITY_IPHONE
	//private ADBannerView banner; 
	//#endif
	

	
	private int currentTime = 0;

	
	//ads stuff
	
	//private BannerView bannerView;
	
	
	public bool isRollingFinalCredits = false;

	PlayerScript player;
	GameObject motherShip;
	
	bool buyedPremium;
	bool buyedNoads;

	
	private bool openedPlatform = false;
	GUIResolutionHelper resolutionHelper;

	private TextLocalizationManager translationManager;

	private int highScore = 0;

	Texture2D leaderBoardTexture;


	private GameObject[] paralaxLevels;
	void Awake()
	{
	
	    //DontDestroyOnLoad(this);
	    
	    if(instance!=null) {
	      Debug.Log("There is another instance gamecontroller running");
	    }
	    else {
		  instance = this;
	    } 
		

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
		screenWidth = resolutionHelper.screenWidth;
		screenHeight = resolutionHelper.screenHeight;
				
		//translations
		//translationManager.LoadSystemLanguage(Application.systemLanguage);
		
		InitPlayer();

	
		//get a reference to the object
		//socialAPIInstance = SocialAPI.Instance;
	  

		
		showUnlockLevel = false;
		isGameOver = true;
		isGameStarted = false;
		//this was true before
		isGamePaused = true;
		isRollingFinalCredits = false;

		//we need to do this before we do the time math, so we can update in case of an existing in app purchase
		//CheckInAppPurchases();


		//seconds to display
	    //elapsedMissionMeters = 5000;

		CheckPause();

		lastHowToTime = 0f;
		initialHowToTime = 0f;
		isShowingHowTo = false;

	}
	
	// Use this for initialization
	void Start () {
	
		skin = Resources.Load("GUISkin") as GUISkin;
		exitTexture	= Resources.Load("button_playstart") as Texture2D;
		rateTexture	= Resources.Load("button_rate") as Texture2D;
		//		exitTexture	= Resources.Load("button_playstart") as Texture2D;

		isGameComplete = false;
		movedGround = false;
		isLevelComplete = false;
		
		appliedHurryUpFactor = false;
		

		isGameOver = true;
		if(isGameOver && currentWorld==1 && currentLevel==1) {

			initialHowToTime = Time.realtimeSinceStartup;
			lastHowToTime = initialHowToTime;
		}

		//paralaxLevels = GameObject.FindGameObjectsWithTag("Scroller");
		isMobilePlatform = (platform == RuntimePlatform.IPhonePlayer) || (platform == RuntimePlatform.Android);
		
	}

	/**
	* Check if we have bought any boosters
	*/
	void CheckInAppPurchases() {
	   
	}

	IEnumerator Fade (float start,float end, float length) {

	 Color aux = redTexture.color;
		  //define Fade parmeters
		if (aux.a == start){

		  for (float i = 0.0f; i < 1.0f; i += Time.deltaTime*(1/length)) { 
		   //for the length of time
		   aux.a = Mathf.Lerp(start, end, i); 
		   //lerp the value of the transparency from the start value to the end value in equal increments
		   yield return null;
		   aux.a = end;
		  // ensure the fade is completely finished (because lerp doesn't always end on an exact value)
          redTexture.color = aux;
          } //end for
 
		} //end if
	
 
	} //end Fade

	/*
	The above example (in FlashWhenHit) will fade your texture from 100% transparent 
	(invisible) to 80% opaque (just slightly transparent) over 1/2 second. 
	It checks to make sure the texture is 100% transparent before attempting the fade 
	to eliminate visual errors, and at the end ensures it is at exactly 80% opacity. 
	It will then wait 1/100th second, and fade the texture back out to transparent. 
	You can, of course, adjust the starting and ending opacity by changing the start 
	and end values in the function call, as well as how long the fade takes and what object if affects. 
	The WaitForSeconds is in there so the texture will stay at its max opacity momentarily 
	(to make it more visually obvious); the length of time is adjustable there too. 
	Also, if you want the screen to flash a certain number of times, 
	you could use a for loop with a counter that goes to 0 from, say, 3, to get the screen to flash 3 times, etc.
	*/

	void FlashWhenInHurryUpMessage (){


		StartCoroutine(Fade (0f, 0.1f, 0.5f));
		StartCoroutine(MyWaitMethod());
		StartCoroutine(Fade (0.1f, 0f, 0.5f));
	
    	
    }

	IEnumerator MyWaitMethod() {
		yield return new WaitForSeconds(.01f);
	}
	
	//setup player stuff
	void InitPlayer() {
	  GameObject obj = GameObject.FindGameObjectWithTag("Player");
	  if(obj!=null) {
		 player = obj.GetComponent<PlayerScript>();
	  }
	  
		
	}
	

		
	
	//invoked every second
	void CheckElapsedMeters() {

		if(!isGamePaused && player!=null && player.IsPlayerAlive()) {

		  elapsedMissionMeters-=5;
													
		}//if !gamePaused

		//TODO this should be configurable
		if (elapsedMissionMeters <= GameConstants.METERS_BOUNDARY_TO_MOVE_GROUND && !movedGround) {
			GameObject ground = GameObject.FindGameObjectWithTag("Ground");
			if(ground!=null) {
				MovingPlatformScript move = ground.GetComponent<MovingPlatformScript>();
				move.enabled = true;
				//avoid that we enable this once more!
				movedGround = true;
			}
		}
		
	}
	/**
	* Add extra seconds
	*/
	public void	IncreaseTimeSecondsBy(int seconds) {
	//do we overlap the min?
		
		elapsedMissionMeters+=seconds;

	  
	}
	
	public int GetCurrentLevel() {
	  return currentLevel;
	}
	
	public int GetCurrentWorld() {
		return currentWorld;
	}
	
	public void SetCurrentLevel(int level) {
	  currentLevel = level;
	}
	
	public int GetNumberOfLevels() {
	  return numberOfLevels;
	}
	
	public static GameControllerScript Instance {

		get
		{
			if (instance == null)
			{
				GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
			    if(scripts!=null) {
					instance = scripts.GetComponentInChildren<GameControllerScript>();
			    }
			    else {
					instance = (GameControllerScript)FindObjectOfType(typeof(GameControllerScript));
					if (instance == null)
						instance = (new GameObject("GameControllerScript")).AddComponent<GameControllerScript>();
			    }
				
			}
			return instance;
		}
	}
	
	public bool IsGameStarted() {
	  return isGameStarted;
	}
	
	public bool IsShowUnlockNextLevel() {
	  return showUnlockLevel;
	}


	//#########  music handling ################

	public void StartMusic() {
    
	   AudioSource source = GetGameMusic();
	   if(source!=null) {
		  source.Play();
	   }

    }

	public void PauseMusic() {
    
		AudioSource source = GetGameMusic();
		if(source!=null) {
			source.mute = true;
		}
		

    }

	public void ResumeMusic() {
    
		AudioSource source = GetGameMusic();
		if(source!=null) {
			source.mute = false;
		}

    }
    
	public void StopMusic() {		
	
		AudioSource source = GetGameMusic();
		if(source!=null) {
			source.Stop();
		}

		foreach(AudioSource sourceAudio in GetGameAudios()) {
		  if(sourceAudio.isPlaying) {
		   sourceAudio.Stop();
		  }
		}

		
	
	}

	private AudioSource[] GetGameAudios() {
		AudioSource []audios = FindObjectsOfType<AudioSource>() as AudioSource[];
		return audios;
	}

	private AudioSource GetGameMusic() {
		/*GameObject music = GameObject.FindGameObjectWithTag("GameMusic");
		if(music!=null) {
			AudioSource source = music.GetComponentInChildren<AudioSource>();
			return source;
		}*/
	    return null;
	}

	//############################


	
	void FixedUpdate()
	{
		
	}
		
	
	
	// Update is called once per frame
	void Update () {

		if(isGameStarted && player!=null && player.IsPlayerAlive() && !isGameComplete) {
		
		}

	}

	private void SpeedUpForegroundPlatforms() {
		GameObject foreground = GameObject.FindGameObjectWithTag("Foreground");
		if(foreground!=null) {
			ScrollingScript script = foreground.GetComponent<ScrollingScript>();
			if(script!=null && script.enabled) {
				script.speed.x = script.speed.x * HURRY_UP_SPEED_INCREASE_FACTOR;
				appliedHurryUpFactor = true;

			}
		}
	 //also speedup any speedable object
	 SpeedUpSpeedables();
	}

	private void SpeedUpSpeedables() {
		GameObject [] allSpeedables = GameObject.FindGameObjectsWithTag("SpeedableRotator");
		foreach(GameObject speedable in allSpeedables) {
			Rotator script = speedable.GetComponent<Rotator>();
			if(script!=null && script.enabled) {
			  script.rotateSpeed = script.rotateSpeed * HURRY_UP_SPEED_INCREASE_FACTOR;
			}
		}
	}


	//invoked when final boss is destroyed
	public void CompletedGame() {
	  isGameComplete = true;
	  //disable camera follow
	  ShowNextScreen();
	}
	
	private void ShowNextScreen() {
		
		bool showNext = (currentLevel < numberOfLevels  || currentWorld < numWorlds);
		EndGame(showNext);

		//either we died or reached last level, guru time!
		if(!showNext) {


		   
			//PerformFinalComputation(true);

		}
		else {
		  if(currentLevel < numberOfLevels) {
		  	//just increase the level on the same world
		    currentLevel+=1;
		  }
		  else {
		  		//save the mission, completed the world
		  		/*
			    switch(currentWorld) {
				 case 1: PlayerPrefs.SetInt(GameConstants.MISSION_1_KEY,1);
			     	break;
			     case 2: PlayerPrefs.SetInt(GameConstants.MISSION_2_KEY,1);
			     	break;
			     case 3: PlayerPrefs.SetInt(GameConstants.MISSION_3_KEY,1);
					break;
				 case 4: PlayerPrefs.SetInt(GameConstants.MISSION_4_KEY,1);
				 	break;
			    }*/

			  //increase world, set first level
		      currentWorld+=1;
		      currentLevel=1;
		    }

		  //these values keep the next in line
		  //PlayerPrefs.SetInt(GameConstants.PLAYING_WORLD,currentWorld);
		  //PlayerPrefs.SetInt(GameConstants.PLAYING_LEVEL,currentLevel);
		    
		  //PerformFinalComputation(false);
		  //show board and do the math :-)
		  //Application.LoadLevel("NextLevelScene");


		  }
		  	
		
	}
	/**
	*performs some level calculations and report any achiviement reached
	*/
	void PerformFinalComputation(bool finishedGame) {
		 

	}

	/**
	* Check the achievements checkpoints
	*/
	void CheckIfReachedAnyAchievementCheckpoint(int totalSaved) {
	//saved more than 100 already?
	  /*if(totalSaved >= GameConstants.ACHIEVEMENT_BRAVE_CHECKPOINT) {
	    //write the achievement
		PlayerPrefs.SetInt(GameConstants.ACHIEVEMENT_BRAVE_KEY,1);
		socialAPIInstance.AddAchievement(GameConstants.ACHIEVEMENT_BRAVE_KEY,100f);
		
	  }
	  //saved more than 150 already?
	  else if(totalSaved >= GameConstants.ACHIEVEMENT_HERO_CHECKPOINT) {
	    //write the achievement
		PlayerPrefs.SetInt(GameConstants.ACHIEVEMENT_HERO_KEY,1);
		socialAPIInstance.AddAchievement(GameConstants.ACHIEVEMENT_HERO_KEY,100f);
	  }
				//saved more than 100 already?
	  else if(totalSaved >= GameConstants.ACHIEVEMENT_LEGEND_CHECKPOINT) {
	    //write the achievement
		PlayerPrefs.SetInt(GameConstants.ACHIEVEMENT_LEGEND_KEY,1);
		socialAPIInstance.AddAchievement(GameConstants.ACHIEVEMENT_LEGEND_KEY,100f);
	  }*/
	}

	/**
	* Are we on the last level??
	*/
	public bool IsFinalLevel() {
	   return currentWorld==numWorlds && currentLevel==numberOfLevels;
	}

	
	private IEnumerator Wait(long seconds)
		
	{		
		yield return new WaitForSeconds(seconds);

	}
	
	public bool IsGameOver() {
	  return isGameOver;
	}
	
	//check if we are on the last level
	//this is important because the mothership
	//will have different behaviours
	public bool IsLastLevel() {
	  return currentLevel == numberOfLevels;
	}
	
	void CheckPause() {
		Time.timeScale = isGamePaused ? 0f : 1.0f; 
	}
	
	public void PauseGame() {
	   /*ScreenShotScript screenshot = GetComponent<ScreenShotScript>();
	   if(screenshot!=null) {
		  screenshot.EnableScreenshots();
	   }*/
	   isGamePaused = true;
	   CheckPause();
	   PauseMusic();


		
	}

	public void ResumeGame() {

		/*ScreenShotScript screenshot = GetComponent<ScreenShotScript>();
		if(screenshot!=null) {
		  screenshot.DisableScreenshots();
		}*/
	    isGameOver = false;
		isGamePaused = false;
		isGameStarted = true;
		showUnlockLevel = false;
		CheckPause();
		ResumeMusic();
	}
	
	public void StartGame() {
	
		isGamePaused = false;
		isGameStarted = true;
		isGameOver = false;
		showUnlockLevel = false;

		/*ScreenShotScript screenshot = GetComponent<ScreenShotScript>();
		if(screenshot!=null) {
		  screenshot.DisableScreenshots();
		}*/

		CheckPause();
		StartMusic();

		if(currentLevel==1) {
		  //if we are on level 1, clear the history
			//ClearPlayerPrefs();
			//stop invoking the increase function
			if(currentWorld==1) {
				//CancelInvoke("IncreaseTimeForHowToTexture");
			}
		}
		
		InvokeRepeating("CheckElapsedMeters", 1.0f, 0.5f);
			
	 }
	 
	
	

	//i shoul stop the scroll of the level
	public void EndGame(bool showUnlockNextLevel) {

		isGameStarted = false;
		isGameOver = true;
		isGamePaused = false;
		StopMusic();
		showUnlockLevel = showUnlockNextLevel;
		//EnableScreenshots();
		CheckPause();
				
		
	}

	void OnGUI() {

			// Set the skin to use
			GUI.skin = skin;
			//We can reduce the draw calls from OnGUI() function by 
			//enclosing all the contents inside a if loop like this one
			//draw level
			skin.label.normal.textColor = Color.black;
			
			Matrix4x4 svMat = GUI.matrix;//save current matrix

			Vector3 scaleVector = resolutionHelper.scaleVector;
			bool isWideScreen = resolutionHelper.isWidescreen;
			int width = resolutionHelper.screenWidth;
			int height = resolutionHelper.screenHeight;

			Matrix4x4 normalMatrix;
			Matrix4x4 wideMatrix;
			//we use the center matrix for the buttons
			wideMatrix = Matrix4x4.TRS(new Vector3( (resolutionHelper.scaleX - scaleVector.y) / 2 * width, 0, 0), Quaternion.identity, scaleVector);
			normalMatrix = Matrix4x4.TRS(Vector3.zero,Quaternion.identity,scaleVector);

			//assign normal matrix by default
			GUI.matrix = normalMatrix;

		    if(Event.current.type==EventType.Repaint && !isGameOver) {

			 	//DrawText(GetTranslationKey(GameConstants.MSG_LEVEL) 
					//	+ " " + currentLevel, messagesFontSizeSmaller +10, 20, 10,200,50);
	
				if(elapsedMissionMeters>=1 && !isLevelComplete) {
					  DrawText(elapsedMissionMeters +" meters!"  , messagesFontSizeSmaller +10, 280, 10,200,50);
					
				}

			    if(isLevelComplete) {
					DrawText("Congratulations! Level " + currentLevel + " Complete."  , messagesFontSizeLarger, width/2-200, height/2,450,50);
				}

			}

         //Draw the final boss hits instead
         //TODO on last level show new instructions, like on first level
		 if(IsFinalLevel()) {

		  }
		    			
			if(Event.current.type==EventType.Repaint) {

				if(isGameStarted) {
			
						
				
					//we need this to put the play/pause at right
					if(isWideScreen){
						GUI.matrix = wideMatrix;
					}
					else{
						GUI.matrix = normalMatrix;
					}


					pausePlayRect = new Rect(width-160 ,15,128,64);


					if(isGamePaused) {
					//if not running
					   GUI.DrawTexture(pausePlayRect, playIcon);
					 
					}
					//game is not paused
				    //draw pause icon
				    else {
						
						GUI.DrawTexture(pausePlayRect, pauseIcon);
					}

				/*backRect = new Rect(60 ,height-160,64,64);
				forwardRect = new Rect(160,height - 160 ,64,64);
				jumpRect = new Rect(width-200,height - 160 ,64,64);
				GUI.DrawTexture(forwardRect, forwardButton);
				GUI.DrawTexture(backRect, backButton);
				GUI.DrawTexture(jumpRect, JumpButton);*/

				}
				else {
				
				  //Debug.Log("Not started yet");
				  //if null means it was destroyd, is game over
				  //besides i cannot start with a null player, and if not a restart
				  //neither if i'm rolling credits
			     if(player!=null && !showUnlockLevel && !isGameComplete) {
				  
					//make sure we draw this at the center of the screen
					if(isWideScreen){
						GUI.matrix = wideMatrix;
					}
					else{
						GUI.matrix = normalMatrix;
					}

					exitTextureRect = new Rect( width/2 - 100,screenHeight/2-60,200,80);
					GUI.DrawTexture(exitTextureRect, exitTexture);

					#if UNITY_ANDROID && !UNITY_EDITOR
					rateRect = new Rect( width/2 - 100,screenHeight/2+40,200,80);
					GUI.DrawTexture(rateRect, rateTexture);
					#endif
					//start playing //screenWidth

					//---------------------------------------------------------------------------------					
					/*#if UNITY_ANDROID || UNITY_IOS
					//GUI.Label(new Rect(width/2-69,(int)screenHeight / 3 * 2 - 15,200,40),"Leaderboards");
					leaderboardsRect = new Rect(width/2-50,screenHeight / 3 * 2 + 10 ,96,96);
					GUI.DrawTexture(leaderboardsRect, leaderBoardTexture,ScaleMode.ScaleToFit);
					#endif*/
					//---------------------------------------------------------------------------------



					//if(highScore > 0) {
						//GetTranslationKey(GameConstants.MSG_HIGH_SCORE)
					//	DrawText("High Score: " + highScore, messagesFontSizeSmaller +10,740, 10,220,40);
					//}
								
					
				 }
				
			   }
				
						
			}//end repaint

				

		//---------------------------------------------------------
		//*************** CHEK TEXTURE CLICKS *********************
		//---------------------------------------------------------
		//before checking the clicks we put the correct matrix

		if(isWideScreen){
			GUI.matrix = wideMatrix;
		}
		else{
			GUI.matrix = normalMatrix;
		}
		//---------------------------------------------

		if(!isMobilePlatform) { //desktop

			if(Event.current.type == EventType.MouseUp ) {
				
				if(isGameOver) {
						#if UNITY_ANDROID && !UNITY_EDITOR
						if(rateRect.Contains(Event.current.mousePosition) && player!=null) {
						  Application.OpenURL("market://details?id=com.pcdreams.superjellytroopers");
					    }
					    #endif

						if(exitTextureRect.Contains(Event.current.mousePosition) ) {
							StartGame();
						}
						/*else if(leaderboardsRect!=null && player!=null && leaderboardsRect.Contains(Event.current.mousePosition) ) {
						  if(socialAPIInstance.isAuthenticated) {
							socialAPIInstance.ShowLeaderBoards();
						  }
						  else {
							StartCoroutine(ShowMessage(GetTranslationKey(GameConstants.MSG_GAME_CENTER_ERROR), 1.5f));
						  }
							
						}*/
			    }
				else {

				//Did i paused the game???
				  if(pausePlayRect.Contains(Event.current.mousePosition)) {
						isGamePaused = !isGamePaused;
						if(isGamePaused) {
							PauseGame();
						}
						else {
							ResumeGame();
						}
			     } 
			     /*else if(backRect.Contains(Event.current.mousePosition)) {
					EnableLevelsScroll();
			        player.MoveBackward();
			        ScrollLevelsForward();
			     }
			     else if(forwardRect.Contains(Event.current.mousePosition)) {
			        
					EnableLevelsScroll();
					player.MoveForward();
					ScrollLevelsBackward();
			     }
				 else if(jumpRect.Contains(Event.current.mousePosition)) {
			        player.Jump();
			     }
			     else {
			       DisableLevelsScroll();
			     }*/
			  }
			      
			}

		  }
		  //if mobile platform
		  else {
		   //----------------------------------------------------------------
		      //detect touches on leaderboards
		      //for this we need the normal matrix

		    bool touchedLeaderBoard = false;
		    if (Input.touches.Length ==1) {
			    
				Touch touch = Input.touches[0];
			    
				if(touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)  {

					Vector2 fingerPos = GetFingerPosition(touch,isWideScreen);

						if(isGameOver) {

				
						/*#if UNITY_IOS || UNITY_ANDROID && !UNITY_EDITOR
				
						 if(leaderboardsRect.Contains(fingerPos) && player!=null) {
							  touchedLeaderBoard = true;

							  if(socialAPIInstance.isAuthenticated) {
								 socialAPIInstance.ShowLeaderBoards();
							  }
							  else {
								StartCoroutine(ShowMessage(GetTranslationKey(GameConstants.MSG_GAME_CENTER_ERROR), 1.5f));
							  }
						 }
					
						#endif*/

							//is game over, maybe not started yet?
							if(exitTextureRect.Contains(fingerPos) ) {
								StartGame();
							}
						    
						    #if UNITY_ANDROID && !UNITY_EDITOR
						    if(rateRect.Contains(fingerPos) && player!=null) {
								Application.OpenURL("market://details?id=com.pcdreams.superjellytroopers");
						    }
						    #endif
							
					   }
					   else if(pausePlayRect.Contains(fingerPos) ) {	
							//already started
							//Did i paused the game???			

							isGamePaused = !isGamePaused;
							if(isGamePaused) {
								PauseGame();
							}
							else {
								ResumeGame();
							}

					  }


					  if(backRect.Contains(fingerPos)) {
					  	//move player back
					  	EnableLevelsScroll();
					  	player.MoveBackward();
					  	ScrollLevelsForward();

					  }
					  else if(forwardRect.Contains(fingerPos)) {
					  	//move player forward
					  	EnableLevelsScroll();
					  	player.MoveForward();
					  	ScrollLevelsBackward();
					  }
					  else if(jumpRect.Contains(fingerPos)) {
					   player.Jump();
					  }
					  else {
					    player.PlayerStationary();
					    DisableLevelsScroll();
					  }
				 }
					
			  }  //end if (Input.touches.Length ==1) 
		   }//else is mobile platform
	
	  //******************** MOBILE TOUCHES ARE HANDLED ON UPDATE() ??? *************
	  //restore the matrix
	  GUI.matrix = svMat;
	
}	

	public void FinishLevel() {
		isLevelComplete = true;
	}

	public int GetNextLevel() {
		if (currentLevel < numberOfLevels) {
			return currentLevel + 1;
		}
		return 1;//the first again!!
	}

    void InvertParalaxScrollingDirection(int direction) {
	 foreach(GameObject obj in paralaxLevels) {
	    ScrollingScript scroll = obj.GetComponent<ScrollingScript>();
	    if(scroll!=null) {
	      scroll.direction.x = direction;
	    }
	  }
    }
	void ScrollLevelsForward() {
	  InvertParalaxScrollingDirection(1);
	}
	void ScrollLevelsBackward() {
	  InvertParalaxScrollingDirection(-1);
	}
	void DisableLevelsScroll() {

	  foreach(GameObject obj in paralaxLevels) {
	    ScrollingScript scroll = obj.GetComponent<ScrollingScript>();
	    if(scroll!=null) {
	      scroll.enabled = false;
	    }
	  }
	}

	void EnableLevelsScroll() {

	  foreach(GameObject obj in paralaxLevels) {
	    ScrollingScript scroll = obj.GetComponent<ScrollingScript>();
	    if(scroll!=null) {
	      scroll.enabled = true;
	    }
	  }
	}
	/**
	*Get the correct finger touch position
	*/
	Vector2 GetFingerPosition(Touch touch, bool isWideScreen) {

	  
		Vector2 fingerPos = new Vector2(0,0);
		float diference = 0f;
					
		fingerPos.y =  screenHeight - (touch.position.y / Screen.height) * screenHeight;
		fingerPos.x = (touch.position.x / Screen.width) * screenWidth;

	    return fingerPos;
	}

	//IEnumerator ShowMessage (string message, float delay) {
      /*GameObject textObj = GameObject.FindGameObjectWithTag("JellyTxt");
      if(textObj!=null) {
		GUIText guiText = textObj.GetComponent<GUIText>();
		if(guiText!=null) {
			guiText.text = message;
     		guiText.enabled = true;
     		yield return new WaitForSeconds(delay);
     		guiText.enabled = false;
		}

      }*/

 	//}


	string GetTranslationKey(string key) {
		return	translationManager.GetText(key);
	}

	
	public bool IsMobilePlatform() {
		return isMobilePlatform;
	}
	
	public bool IsIOSPlatform() {
		return platform == RuntimePlatform.IPhonePlayer; 
	}
	
	public bool IsAndroidPlatform() {
		return platform == RuntimePlatform.Android;
	}

	public void DrawLargerText(string text) {
	    DrawText(text,messagesFontSizeLarger);
	}
	
	public void DrawSmallerText(string text) {
		DrawText(text,messagesFontSizeSmaller);
	}
	
	public void DrawText(string text, int fontSize) {
	

		GUIStyle centeredStyleSmaller = GUI.skin.GetStyle("Label");
		centeredStyleSmaller.alignment = TextAnchor.MiddleLeft;
		centeredStyleSmaller.font = messagesFont;
		centeredStyleSmaller.fontSize = fontSize;
		GUI.Label (new Rect(screenWidth/2-200, screenHeight/2, 400, 50), text);
	}
	
	public void DrawText(string text, int fontSize, int x, int y,int width,int height) {
		
		
		GUIStyle centeredStyleSmaller = GUI.skin.GetStyle("Label");
		centeredStyleSmaller.alignment = TextAnchor.MiddleLeft;
		centeredStyleSmaller.font = messagesFont;
		centeredStyleSmaller.fontSize = fontSize;
		
		GUI.Label(new Rect(x, y, width, height), text);
	}

	public void DrawText(string text, int fontSize, float x, float y,int width,int height) {
		
		
		GUIStyle centeredStyleSmaller = GUI.skin.GetStyle("Label");
		centeredStyleSmaller.alignment = TextAnchor.MiddleLeft;
		centeredStyleSmaller.font = messagesFont;
		centeredStyleSmaller.fontSize = fontSize;
		
		GUI.Label(new Rect(x, y, width, height), text);
	}
	
	//release banner resources
	void OnDestroy() {
	
	
		
	}
	
	
	
	//only spwan and shoot if player is in sight
	public bool IsPlayerVisible() {
		bool checkPlayerVisible = (player==null) ? false : player.GetComponent<Renderer>().IsVisibleFrom(Camera.main);
		return checkPlayerVisible;
	}
	

	
}
