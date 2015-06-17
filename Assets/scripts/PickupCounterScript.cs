using UnityEngine;
using System.Collections;

public class PickupCounterScript : MonoBehaviour {

	public int numberPickups = 0;
	public Font font;
	public int fontSize;
	public Texture2D icon;
	
	public int textureXPosition;
	public int textureYPosition;
	public int textureWidth = 48;
	public int textureHeight = 48; 

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
	
	public void AddMultiplePickups(int count) {
		numberPickups+=count;
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
		
		if(isWideScreen) {
			GUI.matrix = Matrix4x4.TRS(new Vector3( (resolutionHelper.scaleX - scaleVector.y) / 2 * width, 0, 0), Quaternion.identity, scaleVector);
			
			
		}
		else {
			
			GUI.matrix = Matrix4x4.TRS(Vector3.zero,Quaternion.identity,scaleVector);
		}
		
		if(Event.current.type==EventType.Repaint) {

			DrawText( "X " + numberPickups, fontSize, textureXPosition+60 ,textureYPosition,120,40);
			GUI.DrawTexture(new Rect(textureXPosition,textureYPosition,textureWidth,textureHeight),icon);
       
			
		}
		GUI.matrix = svMat;
	}
	
	public void DrawText(string text, int fontSize, int x, int y,int width,int height) {
		
		
		GUIStyle centeredStyleSmaller = GUI.skin.GetStyle("Label");
		centeredStyleSmaller.alignment = TextAnchor.MiddleLeft;
		centeredStyleSmaller.font = font;
		centeredStyleSmaller.fontSize = fontSize ;
		
		GUI.Label (new Rect(x, y, width, height), text);
	}
	
	public void RemovePickup() {
		if(numberPickups>0)
		   numberPickups--;
		
	}
}
