using UnityEngine;
using System.Collections;

public class BalloonScript : MonoBehaviour {


    public bool undestructibleThroughTime = false;
	public bool undestructibleThroughNumberOfHits = false;
	public int undestructibleForNHits = 0;
	public int undestructibleForNSeconds = 0;

	private int initialNumberOfHits = 3;
	private int initialNumberOfSeconds = 10;

	//use only for time ones
	public bool startDestroying = false;

	public bool destroyable = false;
	GameObject healthbar;

	UnityEngine.UI.Text boostersText= null;

	GUISkin skin;
	GUIStyle centeredStyleSmaller;
	// Use this for initialization

	void Start () {
	  destroyable = false;
	  skin = Resources.Load("GUISkin") as GUISkin;
	  initialNumberOfHits = undestructibleForNHits;
	  initialNumberOfSeconds = undestructibleForNSeconds;
	  healthbar = GameObject.FindGameObjectWithTag("HealthBar");
	  GameObject txt = GameObject.FindGameObjectWithTag("BoostersText");
	  if(txt!=null) {
	    boostersText = txt.GetComponent<UnityEngine.UI.Text>();
	    boostersText.text = "";//insane check
	  }
	}
	
	// Update is called once per frame
	void Update () {
	  if(destroyable) {

	    GameObject p = GameObject.FindGameObjectWithTag("Player");
	    if(p!=null) {
	      PlayerScript player = p.GetComponent<PlayerScript>();
	      player.ResetDefaultSprite();
	    }

	    Destroy(this);
	  }
	  else if(startDestroying) {
		//Debug.Log("DECREASING TIMER SINCE NOW");
	    startDestroying = false;//avoid call this part again
		InvokeRepeating("DecreaseSecondsCounter",1.0f,1.0f);
	  }

		if(boostersText!=null) {
			if(undestructibleForNHits > 0) {
			    boostersText.text = "X " + undestructibleForNHits;
			}
		    else if(undestructibleForNSeconds > 0) {
				boostersText.text = undestructibleForNSeconds + " seconds";
			}
			else {
			  boostersText.text = "";//make sure nothing stays there!
			}
		}
	}

	public void DecreaseHitsCounter() {

	  undestructibleForNHits-=1;

	  if(undestructibleForNHits < 0) {
	    undestructibleForNHits = 0;
	  }

	  UpdateHealthBar(undestructibleForNHits);

	  if(undestructibleForNHits==0) {
	    destroyable = true;
	  }
	}

	//only for time
	public void StartCountdownDestruction() {
	  if(startDestroying)
	    return;

	  startDestroying = true;
	}

	public void DecreaseSecondsCounter() {
	  undestructibleForNSeconds-=1;
	  if(undestructibleForNSeconds < 0) {
		undestructibleForNSeconds = 0;
	  }

	  UpdateHealthBar(undestructibleForNSeconds);

	  if(undestructibleForNSeconds==0) {
	    destroyable = true;
	  }
	}

	public bool IsUndestructibleThroughTime() {
	  return undestructibleForNSeconds > 0;
	}

	public bool IsUndestructibleThroughHits() {
	  return undestructibleForNHits > 0;
	}

	public bool IsDestroyable() {
	 return destroyable;
	}

	public int GetInitialNumberOfHits() {
	 return initialNumberOfHits;
	}

	public int GetInitialNumberOfSeconds() {
	  return initialNumberOfSeconds;
	}

	/*void OnGUI() {
	
		GUI.skin = skin;

		if(centeredStyleSmaller==null) {
		  BuildStyle();
		}
		
		if(Event.current.type==EventType.Repaint ) {//&& !controller.IsGameOver()

			
			//Matrix4x4 svMat = GUI.matrix;//save current matrix
			//GUI.matrix = Matrix4x4.TRS(Vector3.zero,Quaternion.identity,resolutionHelper.scaleVector);


			  

			 
		

		}
	}*/

	public void DrawText(string text, int fontSize, int x, int y,int width,int height) {
		
		GUI.Label (new Rect(x, y, width, height), text,centeredStyleSmaller);
	}

	void BuildStyle() {

		centeredStyleSmaller = new GUIStyle(GUI.skin.label);
		centeredStyleSmaller.alignment = TextAnchor.MiddleLeft;
		//centeredStyleSmaller.font = controller.messagesFont;
		//centeredStyleSmaller.fontSize = fontSize;
	}

	//just avoid calling this again
	void OnDestroy() {
		destroyable = false;
	}

	void UpdateHealthBar(float currentValue) {

		if(healthbar!=null) {
		   HealthBar bar = healthbar.GetComponent<HealthBar>();
			//set visible
		   bar.SetCurrentHealth(currentValue);
		}
	}
}
