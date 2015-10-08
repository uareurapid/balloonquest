using UnityEngine;
using System.Collections;

public class BalloonScript : MonoBehaviour {


    public bool undestructibleThroughTime = false;
	public bool undestructibleThroughNumberOfHits = false;
	public int undestructibleForNHits = 0;
	public int undestructibleForNSeconds = 0;

	//use only for time ones
	public bool startDestroying = false;

	public bool destroyable = false;

	GUISkin skin;
	GUIStyle centeredStyleSmaller;
	// Use this for initialization
	void Start () {
	  destroyable = false;
	  skin = Resources.Load("GUISkin") as GUISkin;
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
		Debug.Log("DECREASING TIMER SINCE NOW");
	    startDestroying = false;//avoid call this part again
		InvokeRepeating("DecreaseSecondsCounter",1.0f,1.0f);
	  }
	}

	public void DecreaseHitsCounter() {
	  Debug.Log("DECREASING COUNTER");
	  undestructibleForNHits-=1;
	  if(undestructibleForNHits < 0) {
	    undestructibleForNHits = 0;
	  }

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

	void OnGUI() {
	
		GUI.skin = skin;

		if(centeredStyleSmaller==null) {
		  BuildStyle();
		}
		
		if(Event.current.type==EventType.Repaint ) {//&& !controller.IsGameOver()

			
			//Matrix4x4 svMat = GUI.matrix;//save current matrix
			//GUI.matrix = Matrix4x4.TRS(Vector3.zero,Quaternion.identity,resolutionHelper.scaleVector);

			if(undestructibleForNHits > 0) {

			  DrawText("X " + undestructibleForNHits,20,100,100,600,60);
			}
			else if(undestructibleForNSeconds > 0) {
				DrawText(undestructibleForNSeconds + " seconds",20,100,100,600,60);
			}
		

		}
	}

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
}
