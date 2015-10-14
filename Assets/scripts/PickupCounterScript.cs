using UnityEngine;
using System.Collections;

public class PickupCounterScript : MonoBehaviour {

	public int numberPickups = 0;
	public Font font;
	public int fontSize;
	public Texture2D icon;

	//something that we instantiate and add to player object
	public GameObject transformGift = null;
	//this will be the balloon script basically

	public int textureWidth = 128;
	public int textureHeight = 128; 

	public int yPosition = 70;

	public int xPosition = -1; 


	private GUIResolutionHelper resolutionHelper;

	// Use this for initialization
	GUISkin skin;
	void Start () {
		skin = Resources.Load("GUISkin") as GUISkin;
		GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
		if(scripts!=null) {
		  resolutionHelper = scripts.GetComponent<GUIResolutionHelper>();
		  resolutionHelper.CheckScreenResolution();
		}
		numberPickups = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Awake() {


	  
	}

	public void AddPickup() {
		numberPickups+=1;
	}
	public void RemovePickup() {
		if(numberPickups > 0)
			numberPickups--;
	}
	
	public void AddMultiplePickups(int count) {
		numberPickups+=count;
	}

	public void RemoveMultiplePickups(int count) {
		numberPickups-=count;
		if (numberPickups < 0) {
			numberPickups = 0;
		}
	}
	
	public void ResetPickups() {
		numberPickups=0;
	}
	
	void OnGUI() {
	    
	    GUI.skin = skin;
		Matrix4x4 svMat = GUI.matrix;//save current matrix
		int width = resolutionHelper.screenWidth;
		Vector3 scaleVector = resolutionHelper.scaleVector;
		bool isWideScreen = resolutionHelper.isWidescreen;
		
		//if(isWideScreen) {
		//	GUI.matrix = Matrix4x4.TRS(new Vector3( (resolutionHelper.scaleX - scaleVector.y) / 2 * width, 0, 0), Quaternion.identity, scaleVector);
		//}
		//else {
			
		GUI.matrix = Matrix4x4.TRS(Vector3.zero,Quaternion.identity,scaleVector);
		//}
		
		if(Event.current.type==EventType.Repaint) {

		    if(xPosition>0) {

				DrawText( "X " + numberPickups, fontSize,xPosition+textureWidth ,yPosition,120,60);
				GUI.DrawTexture(new Rect(xPosition,yPosition,textureWidth,textureHeight),icon);
		    }
		    else {

				DrawText( "X " + numberPickups, fontSize, width-textureWidth,yPosition,120,60);
				GUI.DrawTexture(new Rect(width-textureWidth*2,yPosition,textureWidth,textureHeight),icon);
		    }

       
			
		}
		GUI.matrix = svMat;
	}
	
	public void DrawText(string text, int fontSize, int x, int y,int width,int height) {
		
		
		GUIStyle centeredStyleSmaller = new GUIStyle(GUI.skin.label);
		centeredStyleSmaller.alignment = TextAnchor.MiddleLeft;
		centeredStyleSmaller.font = font;
		centeredStyleSmaller.fontSize = fontSize ;
		
		GUI.Label (new Rect(x, y, width, height), text,centeredStyleSmaller);
	}

	//add this transform to the player object
	public void AddGiftGameObjectToPlayer(PlayerScript player) {
	  if(transformGift!=null) {
		 GameObject newBalloonObject = (GameObject)Instantiate(transformGift, player.transform.position, player.transform.rotation);	
		 newBalloonObject.transform.parent = player.transform;

		 BalloonScript ball = newBalloonObject.GetComponent<BalloonScript>();
		 if(ball!=null) {
			//green
			//will start the countdown
			if(ball.IsUndestructibleThroughTime()) {

				SetHealthBar((float)ball.GetInitialNumberOfSeconds());
				ball.StartCountdownDestruction();
			}
			else if(ball.IsUndestructibleThroughHits()) {
				SetHealthBar((float)ball.GetInitialNumberOfHits());
				}
			}

		 }
		
	}

	void SetHealthBar(float initialValue) {
		GameObject healthbar = GameObject.FindGameObjectWithTag("HealthBar");
		if(healthbar!=null) {
		   HealthBar bar = healthbar.GetComponent<HealthBar>();
			//set visible
		   bar.SetMaxHealth(initialValue);
		//	foreach(UnityEngine.UI.Image img in bar.gameObject.GetComponentsInChildren<UnityEngine.UI.Image>()){
		//	   img.enabled=true;
		//	}
		}
	}
	

}
