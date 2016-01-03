using UnityEngine;
using System.Collections;
using MrBalloony;

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
	private Texture2D screenshotTexture;
	private bool touchedScreenshotTexture = false;
	private Rect screenshotTextureRect;
	private float maxTextureWidth = 512f;
	private float minTextureWidth = 256;
	private float maxTextureHeight = 384;
	private float minTextureHeight = 128;
	private float rotationAngle = 45f;

	private GUIStyle centeredStyleLarger;
	
	public int screenWidth;
	public int screenHeight;
	
	Rect pausePlayRect;
	//Rect exitTextureRect;
	Rect leaderboardsRect;
	Rect rateRect;
	Rect forwardRect;
	Rect backRect;
    Rect jumpRect;

	private bool soundOn = true;
	private bool musicOn = true;
	public bool isGamePaused = true;
	private bool isGameStarted = false;
	private bool isGameOver = true;
	private bool isGameComplete = false;
	private bool isLevelComplete = false;

	private GUITexture redTexture;
	
	public bool isDebug = false;
	
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

	public GameObject metersHealthBar;

	private MeshRenderer countDownMesh;
	private int countDown = 3;
	//display time

	public int missionTimeInSeconds;

	//number of minutes to complete the mission
	private int missionTimeInMinutes = 0;
	//display time
	private int elapsedMissionMinutes = 0;
	private int elapsedMissionSeconds = 0;
	private int totalRemainingMissionTimeInSeconds = 0;
	
	//show in app for level xxx?
	private bool showUnlockLevel = true;
	//to control if we already released the Ground
	private bool movedGround = false;


	
	private JetpackBar metersHealtBarScript;
	
	private int currentTime = 0;

	
	//ads stuff
	
	//private BannerView bannerView;
	
	
	public bool isRollingFinalCredits = false;

	PlayerScript player;
	GameObject motherShip;
	
	bool buyedPremium;
	bool buyedNoads;

	//remaining meters
	public UnityEngine.UI.Text metersCounter;
	//level finished/congratulations
	public UnityEngine.UI.Text levelFinishedTxt;
	
	private bool openedPlatform = false;
	GUIResolutionHelper resolutionHelper;

	private TextLocalizationManager translationManager;

	private int highScore = 0;

	Texture2D leaderBoardTexture;

	//when the level is loaded, starts counting on StartCountdown
	private float timeOnLoad = 0f;

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
	    missionTimeInSeconds = isDebug ? GameConstants.DEBUG_STARTING_TIME_IN_SECONDS : GameConstants.STARTING_TIME_IN_SECONDS;

		missionTimeInMinutes = (int)missionTimeInSeconds / 60 ;

		//minutes to display
		elapsedMissionMinutes = missionTimeInMinutes;
		//seconds to display
	    elapsedMissionSeconds = (int)missionTimeInSeconds % 60;

		totalRemainingMissionTimeInSeconds = missionTimeInSeconds;

		CheckPause();

		checkSoundSettings();
		checkMusicSettings();

		lastHowToTime = 0f;
		initialHowToTime = 0f;
		isShowingHowTo = false;

	}
	
	// Use this for initialization
	void Start () {
	
		skin = Resources.Load("GUISkin") as GUISkin;
		//exitTexture	= Resources.Load("button_playstart") as Texture2D;
		rateTexture	= Resources.Load("button_rate") as Texture2D;
		//		exitTexture	= Resources.Load("button_playstart") as Texture2D;

		metersCounter.text = "";

		//check screen size, this was breaking, probably some place we are calling Instance
		GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
		if(scripts!=null) {
			resolutionHelper = scripts.GetComponent<GUIResolutionHelper>();
		}
		else {
			resolutionHelper = GUIResolutionHelper.Instance;
		}
		screenWidth = resolutionHelper.screenWidth;
		screenHeight = resolutionHelper.screenHeight;

		isGameComplete = false;
		movedGround = false;
		isLevelComplete = false;
		
		appliedHurryUpFactor = false;
		
		rotationAngle = GameConstants.MAX_SCREENSHOT_ROTATION;
		maxTextureWidth = GameConstants.MAX_SCREENSHOT_WIDTH;
		maxTextureHeight = GameConstants.MAX_SCREENSHOT_HEIGHT;
		minTextureWidth = GameConstants.MIN_SCREENSHOT_WIDTH;
		minTextureHeight = GameConstants.MIN_SCREENSHOT_HEIGHT;

		isGameOver = true;
		if(isGameOver && currentWorld==1 && currentLevel==1) {

			initialHowToTime = Time.realtimeSinceStartup;
			lastHowToTime = initialHowToTime;
		}

		isMobilePlatform = (platform == RuntimePlatform.IPhonePlayer) || (platform == RuntimePlatform.Android);

		paralaxLevels = GetParalaxLevels ();
		//start game play countdown
		StartCountdown ();

		metersHealtBarScript = metersHealthBar.GetComponent<JetpackBar>();
		metersHealtBarScript.SetMaxValue(missionTimeInSeconds);
		metersHealtBarScript.SetCurrentValue(missionTimeInSeconds);
	}

	void OnLevelWasLoaded(int level) {

	    if(isMobilePlatform) {
			Handheld.StopActivityIndicator();
	    }
        
    }

    public void IncreaseScoreBy(int points) {

		GameObject levelScore = GameObject.FindGameObjectWithTag("LevelScore");
		if(levelScore!=null) {
			UnityEngine.UI.Text txtScore =levelScore.GetComponent<UnityEngine.UI.Text>();
		    if(txtScore!=null) {
		      int previousValue = int.Parse(txtScore.text);
		      previousValue+=points;
		      txtScore.text = previousValue.ToString();
		    }
		}
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
	/*void CheckElapsedMeters() {

		if(!isGamePaused && player!=null && player.IsPlayerAlive()) {

		  //do not go under 0
		  if(missionTimeInSeconds > 0) {
			if(missionTimeInSeconds <= 5) {
			    //on 5 start counting 1 by one, until reach 0
			    missionTimeInSeconds-=1;
		    }
		    else {
				missionTimeInSeconds-=GameConstants.METERS_STEP;
		    }
		 }


		  metersHealtBarScript.SetCurrentValue(missionTimeInSeconds);
													
		}//if !gamePaused

		
	}*/

	//invoked every second
	void CheckMissionTime() {

	if(!isGamePaused && player!=null && player.IsPlayerAlive()) {

	 totalRemainingMissionTimeInSeconds -=1;

	    //update the blue health bar
		metersHealtBarScript.SetCurrentValue(totalRemainingMissionTimeInSeconds);

		//time is up! player dead!
	    if(elapsedMissionMinutes==0 && elapsedMissionSeconds==0) {
	       //dead for good!!!!
	       player.HandleLooseAllLifes();
	    }
	    else {
					
					bool changeMinutes = false;
					elapsedMissionSeconds-=1;
					
					if(elapsedMissionSeconds<0) {
						
						if(elapsedMissionMinutes>0) {
							elapsedMissionSeconds = 59;
							changeMinutes = true;
						}
						else {
						 changeMinutes = false;
						 elapsedMissionSeconds = 0;
						}
						
					}
					
					if(changeMinutes && elapsedMissionMinutes>0) {
						elapsedMissionMinutes-=1;
					}
	    }
	 
		
		
		
	 //}
		 
						
	 }//if !gamePaused
		
		
		
	}
	/**
	* Add extra seconds
	*/
	//public void	IncreaseTimeSecondsBy(int seconds) {
	//do we overlap the min?
		
	//	elapsedMissionMeters+=seconds;

	  
	//}

	void PlaySettingsSound() {
		
		GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
		if (scripts != null) {
			SoundEffectsHelper fx = scripts.GetComponentInChildren<SoundEffectsHelper> ();
			fx.PlaySettingsSound ();
		} 
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
	      source.volume = 0.5f;
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
		GameObject music = GameObject.FindGameObjectWithTag("GameMusic");
		if(music!=null) {
			AudioSource source = music.GetComponentInChildren<AudioSource>();
			return source;
		}
	    return null;
	}

	//############################


	
	void FixedUpdate()
	{

	}
		
	
	
	// Update is called once per frame
	void Update () {

		if (!isGameStarted) {
			float realTimeSinceLoad = Time.realtimeSinceStartup;
			float diff = realTimeSinceLoad - timeOnLoad;
			if(diff > 4f && countDown==0) {
				countDown = 3;//need to reset it here, otherwise it will start game again
				countDownMesh.enabled = false;
				StartGame();
			}else if(diff > 3f && diff < 4f && countDown==1) {
				DecreaseCountdown();
			}
			else if( diff > 2f && diff < 3f && countDown==2) { 
				DecreaseCountdown();
			}
			else if(diff > 1f && diff < 2f && countDown==3) {
				DecreaseCountdown();
			}
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
	   PlaySettingsSound ();
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
		PlaySettingsSound ();
		//this call must be done either after/before considering the Time.timeScale of the moment
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
		EnableLevelsScroll ();
		if (musicOn) {
			StartMusic();
		}

		if(player!=null) {
		  player.GetLandingPlatform().SetHealthBar();
		  player.GetLandingPlatform().StartCountdownDestruction();
		  //disable the terrain colliders
		  player.DisableTerrainColliders();


		}

		if(currentLevel==1) {
		  //if we are on level 1, clear the history
			//ClearPlayerPrefs();
			//stop invoking the increase function
			if(currentWorld==1) {
				//CancelInvoke("IncreaseTimeForHowToTexture");
			}
		}

		InvokeRepeating("CheckMissionTime", 1.0f, 1.0f);
		//InvokeRepeating("CheckElapsedMeters", 1.0f, 0.5f);
			
	 }


	//i shoul stop the scroll of the level
	public void EndGame(bool showUnlockNextLevel) {

		isGameStarted = false;
		isGameOver = true;
		isGamePaused = false;
		StopMusic();
		showUnlockLevel = showUnlockNextLevel;
		//EnableScreenshots();
		SaveScores ();
		DisableLevelsScroll ();
		CheckPause();
				
		
	}

	public void SaveScores() {

	  int points = 0 ;
		GameObject levelScore = GameObject.FindGameObjectWithTag("LevelScore");
		if(levelScore!=null) {
			UnityEngine.UI.Text txtScore =levelScore.GetComponent<UnityEngine.UI.Text>();
		    if(txtScore!=null) {
		       points = int.Parse(txtScore.text);

		      }
		}
		int secondsElapsed = GameConstants.STARTING_TIME_IN_SECONDS - elapsedMissionSeconds;
		int highScore = secondsElapsed + points;
		PlayerPrefs.SetInt (GameConstants.HIGH_SCORE_KEY, highScore );
		int bestScore = PlayerPrefs.GetInt (GameConstants.BEST_SCORE_KEY, highScore);

		//if now i have a better score
		if (highScore > bestScore) {

			bestScore = highScore;
			//we have a new best score!
		}
		PlayerPrefs.SetInt (GameConstants.BEST_SCORE_KEY, bestScore);

	}

	void OnGUI() {

			// Set the skin to use
			GUI.skin = skin;

			if(centeredStyleLarger==null) {
				BuildLargerLabelStyle();
			} 
			
			Matrix4x4 svMat = GUI.matrix;//save current matrix

			Vector3 scaleVector = resolutionHelper.scaleVector;
			bool isWideScreen = resolutionHelper.isWidescreen;
			int width = resolutionHelper.screenWidth;
			int height = resolutionHelper.screenHeight;

			//assign normal matrix by default
			//only now define the matrix for the buttons
			GUI.matrix = Matrix4x4.TRS(Vector3.zero,Quaternion.identity,scaleVector);

		    if(Event.current.type==EventType.Repaint) {

		        //use normal matrix here
		        GUI.matrix = svMat;

				if(!isGameOver) {


					if (elapsedMissionMinutes>=1) {
					    if(elapsedMissionSeconds>=10) {
							metersCounter.text = " 0" + elapsedMissionMinutes +":" + elapsedMissionSeconds;
						}
					    else {
							metersCounter.text = " 0" + elapsedMissionMinutes +":0" + elapsedMissionSeconds;
						}
						
					}
					else {
					   if(elapsedMissionSeconds>=10) {
						    metersCounter.text = " 0:" + elapsedMissionSeconds;
						}
					   else {
					   
							metersCounter.text =" 0:0" + elapsedMissionSeconds;
						}
						
					}

					//if(missionTimeInSeconds>0 && !isLevelComplete) {
					//	metersCounter.text = missionTimeInSeconds +" meters!";
					//}
					//else {
					//    metersCounter.text = "";
					//}

				    if(isLevelComplete) {
					    //unlock some achievement here maybe
					    levelFinishedTxt.text = "Congratulations!" + 
						"Level " + currentLevel + " Complete.";
					}
				}//IS GAME OVER
				else {
					//draw any screenshot if available
				    if(screenshotTexture!=null) {

						Vector2 pivotPoint = new Vector2(Screen.width / 2, Screen.height / 2);

						//if not touhced or already hit max
						if(!touchedScreenshotTexture ) {
							//rotate the matrix to draw the texture
							//float rotAngle = 45f;
							GUIUtility.RotateAroundPivot(GameConstants.MAX_SCREENSHOT_ROTATION, pivotPoint);
		        			
					     	screenshotTextureRect = new Rect(Screen.width/2,Screen.height/2,GameConstants.MIN_SCREENSHOT_WIDTH,GameConstants.MIN_SCREENSHOT_HEIGHT);
							GUI.DrawTexture(screenshotTextureRect,screenshotTexture);

							//restore the matrix rotation afterdraw the texture
							float rotAngle = GameConstants.MAX_SCREENSHOT_ROTATION * -1;
		        			GUIUtility.RotateAroundPivot(rotAngle, pivotPoint);
						}
						else {
						   //draw it bigger, but it will cover other things, no???

								if(minTextureWidth < GameConstants.MAX_SCREENSHOT_WIDTH) {
									minTextureWidth +=2;
									minTextureHeight +=2;
								}

								if(minTextureWidth > GameConstants.MAX_SCREENSHOT_WIDTH) {
									minTextureWidth = GameConstants.MAX_SCREENSHOT_WIDTH;
								}
								if(minTextureHeight > GameConstants.MAX_SCREENSHOT_HEIGHT) {
									minTextureHeight = GameConstants.MAX_SCREENSHOT_HEIGHT;
								}
								//we need to count with image size, size the position is top left of the texture
								screenshotTextureRect = new Rect ((Screen.width / 2) - (minTextureWidth/2), 
								(Screen.height / 2 + 50) - (minTextureHeight/2),
								minTextureWidth,
								minTextureHeight);

								GUI.DrawTexture(screenshotTextureRect,screenshotTexture);

						}
				    	
				    }
				}
				//now use this one
				GUI.matrix = Matrix4x4.TRS(Vector3.zero,Quaternion.identity,scaleVector);

				if(isGameStarted) {
						if(player!=null && !player.PlayerTouchedGround()) {

							pausePlayRect = new Rect(width-100, height-100,96,96);
							if(isGamePaused) {
							//if not running
							   GUI.DrawTexture(pausePlayRect, playIcon);
							}
							//game is not paused
						    //draw pause icon
						    else {
								GUI.DrawTexture(pausePlayRect, pauseIcon);
							}
						}
					}
					else {
					  //Debug.Log("Not started yet");
					  //if null means it was destroyd, is game over
					  //besides i cannot start with a null player, and if not a restart
					  //neither if i'm rolling credits
				     if(player!=null && !showUnlockLevel && !isGameComplete) {
					  
				

						#if UNITY_ANDROID && !UNITY_EDITOR
						rateRect = new Rect( width/2 - 100,screenHeight/2+40,200,80);
						GUI.DrawTexture(rateRect, rateTexture);
						#endif
						//start playing //screenWidth
							
						
					 }
					
				   }
	
				

			}


				

		//---------------------------------------------------------
		//*************** CHEK TEXTURE CLICKS *********************
		//---------------------------------------------------------



		if(!isMobilePlatform) { //desktop



			if(Event.current.type == EventType.MouseUp ) {
				
				if(isGameOver) {
						//#if UNITY_ANDROID && !UNITY_EDITOR
						//if(rateRect.Contains(Event.current.mousePosition) && player!=null) {
						//  Application.OpenURL("market://details?id=com.pcdreams.superjellytroopers");
					    //}
					    //#endif

						//if(exitTextureRect.Contains(Event.current.mousePosition) ) {
						//	StartGame();
						//}
						/*else if(leaderboardsRect!=null && player!=null && leaderboardsRect.Contains(Event.current.mousePosition) ) {
						  if(socialAPIInstance.isAuthenticated) {
							socialAPIInstance.ShowLeaderBoards();
						  }
						  else {
							StartCoroutine(ShowMessage(GetTranslationKey(GameConstants.MSG_GAME_CENTER_ERROR), 1.5f));
						  }
							
						}*/

					if(!touchedScreenshotTexture && screenshotTextureRect.Contains(Event.current.mousePosition)) {
						touchedScreenshotTexture = true;
					}
			    }
				else {
				  if(pausePlayRect.Contains(Event.current.mousePosition)) {
						//Did i paused the game???

						isGamePaused = !isGamePaused;
						if(isGamePaused) {
							PauseGame();
						}
						else {
							ResumeGame();
						}
			     } 
			   
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
							//avoid multiple touches
							if(!touchedScreenshotTexture && screenshotTextureRect.Contains(fingerPos)) {
								touchedScreenshotTexture = true;
							}

				
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
							//if(exitTextureRect.Contains(fingerPos) ) {
							//	StartGame();
							//}
						    
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

				
				 }
					
			  }  //end if (Input.touches.Length ==1) 
		   }//else is mobile platform
	
	  //******************** MOBILE TOUCHES ARE HANDLED ON UPDATE() ??? *************
	  //restore the matrix
	  GUI.matrix = svMat;
	
}	


	void BuildLargerLabelStyle() {
	
		centeredStyleLarger =  new GUIStyle(GUI.skin.label);
		centeredStyleLarger.alignment = TextAnchor.UpperCenter;
		centeredStyleLarger.font = messagesFont;
		centeredStyleLarger.fontSize = messagesFontSizeSmaller;

	}

	public void FinishLevel() {
		isLevelComplete = true;
		PlayerPrefs.SetString(GameConstants.UNLOCKED_LEVEL_KEY + currentLevel,GameConstants.UNLOCKED_LEVEL_KEY + currentLevel);
		PlayerPrefs.Save ();

	}

	public void DisableGameElements(bool disableMovement) {
		//DisableCameraShake();
		if(disableMovement) {
			DisableMovingPlatforms();
		}
		DisableScrolling();
		DisableSpawning();
		DisableCameraShake();
		DisablePunchScripts();
	
	}

	public void DisablePunchScripts() {
		PunchScript[] punches= FindObjectsOfType(typeof(PunchScript)) as PunchScript[];
		foreach (PunchScript punch in punches) {
			punch.enabled = false;
		}
	}

	//disable camera shake if needed
	public void DisableCameraShake() {
	    if(Camera.main!=null) {
			CameraShake shake = Camera.main.GetComponent<CameraShake> ();
			if (shake != null) {
				shake.DisableShake();
				shake.enabled = false;
			}
	    }
		
	}

	//disable all the moving platforms, call when landed
	public void DisableMovingPlatforms() {
	  MovingPlatformScript[] objects = GameObject.FindObjectsOfType<MovingPlatformScript>();
	  foreach(MovingPlatformScript mv in objects) {
	    if(mv!=null && mv.tag!="Ground") {
	      mv.enabled = false;
	      //and make them fall
	      GameObject obj = mv.gameObject;
	      if(obj!=null) {
	          //stop playing any particle
			  ParticleSystem part = obj.GetComponentInChildren<ParticleSystem>();
	      	  if(part!=null) {
				part.Stop(true);
	      	  }
	      	  //if it has a body attached, we enable gravity scale on it
	      	  //we make sure it falls by the laws of gravity
			  Rigidbody2D body = obj.GetComponent<Rigidbody2D>();
			  if(body!=null) {
			    body.gravityScale = 1.0f;
			    body.isKinematic = false;
			  }
	      }

	    }
	  }
	}
	//disable all scrolling backgrounds
	public void DisableScrolling() {
		ScrollingScript[] scrolls= FindObjectsOfType(typeof(ScrollingScript)) as ScrollingScript[];
		foreach (ScrollingScript scroll in scrolls) {
			scroll.enabled = false;
		}
	}

	public void DisableSpawning() {
		SpawnerScript [] spawners = GameObject.FindObjectsOfType(typeof(SpawnerScript)) as SpawnerScript[];
		if (spawners != null && spawners.Length > 0) {
			foreach(SpawnerScript spawner in spawners) {
				spawner.canSpawn = false;
			}
		}
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

	GameObject[] GetParalaxLevels() {
		return GameObject.FindGameObjectsWithTag ("Scroller");
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

	  foreach(GameObject obj in paralaxLevels) {
		ParallaxScript scroll = obj.GetComponent<ParallaxScript>();
		 if(scroll!=null) {
		     scroll.enabled = true;
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

	  foreach(GameObject obj in paralaxLevels) {
		  ParallaxScript scroll = obj.GetComponent<ParallaxScript>();
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
	

		//GUIStyle centeredStyleSmaller = GUI.skin.GetStyle("Label");
		//centeredStyleSmaller.alignment = TextAnchor.MiddleLeft;
		//centeredStyleSmaller.font = messagesFont;
		//centeredStyleSmaller.fontSize = fontSize;
		GUI.Label (new Rect(screenWidth/2-200, screenHeight/2, 400, 50), text,centeredStyleLarger);
	}
	
	public void DrawText(string text, int fontSize, int x, int y,int width,int height) {
		
		
		//GUIStyle centeredStyleSmaller = GUI.skin.GetStyle("Label");
		//centeredStyleSmaller.alignment = TextAnchor.MiddleLeft;
		//centeredStyleSmaller.font = messagesFont;
		//centeredStyleSmaller.fontSize = fontSize;
		
		GUI.Label(new Rect(x, y, width, height), text,centeredStyleLarger);
	}

	public void DrawText(string text, int fontSize, float x, float y,int width,int height) {
		
		
		//GUIStyle centeredStyleSmaller = GUI.skin.GetStyle("Label");
		//centeredStyleSmaller.alignment = TextAnchor.MiddleLeft;
		//centeredStyleSmaller.font = messagesFont;
		//centeredStyleSmaller.fontSize = fontSize;
		
		GUI.Label(new Rect(x, y, width, height), text,centeredStyleLarger);
	}
	
	//release banner resources
	void OnDestroy() {
	
	
		
	}
	
	
	
	//only spwan and shoot if player is in sight
	public bool IsPlayerVisible() {
		bool checkPlayerVisible = (player==null) ? false : player.GetComponent<Renderer>().IsVisibleFrom(Camera.main);
		return checkPlayerVisible;
	}

	void StartCountdown() {
		countDown = 3;
		GameObject countdownTxt = GameObject.FindGameObjectWithTag("Countdown");

		if(countdownTxt!=null) {
			CenterObjectOnScreen(countdownTxt);
			countDownMesh = countdownTxt.GetComponent<MeshRenderer>();
			countDownMesh.enabled = true;
			countdownTxt.GetComponent<TextMesh>().text = "0" + countDown;
			//Time.timeScale = 1f;
			timeOnLoad = Time.realtimeSinceStartup;

		}
	}

	void DecreaseCountdown() {
		TextMesh txt = countDownMesh.GetComponentInParent<TextMesh>();
		int value = int.Parse(txt.text);
		if(value>0) {
			value-=1;
			countDown = value;
			if(countDown==0) {
				txt.text = "GO!";
			}
			else {
				txt.text = "0" + countDown;
			}

			//play countdown sound
			GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
			if (scripts != null) {
				SoundEffectsHelper fx = scripts.GetComponentInChildren<SoundEffectsHelper> ();
				Time.timeScale = 1f;
				fx.PlayCountdownSound();
				Time.timeScale = 0f;
			} 


		}
		else {
			countDown = 0;
			txt.text = "GO!";
			countDownMesh.enabled = false;
			//WILL START THE GAME
		}
		
	}

	void CenterObjectOnScreen(GameObject obj) {
		float z = obj.transform.position.z;
		Vector3 newPosition = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane));
		newPosition.z = z;
		obj.transform.position = newPosition;
	}

	//TODO rechek
	public void TakeScreenShot() {
	/*
		ScreenShotScript screenshot = GameObject.FindGameObjectWithTag("Scripts").GetComponent<ScreenShotScript>();
		if (screenshot != null) {
			screenshot.TakeScreenshotBeforeGameOver (this);
		} */

	}

	//callback for the screenshot script
	public void SetScreenshotTexture(Texture2D texture) {
	  screenshotTexture = texture;
	}

	
}
