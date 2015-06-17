using UnityEngine;
using System.Collections;
using JumpAndRun;

public class FlipScript : MonoBehaviour {

    //public bool flipHorizontal = false;
    public bool flipXAxis = false;
    public bool flipYAxis = false;
    public bool flipZAxis = false;
    public float angle = 180;
    public float flipInterval = 3.0f;
    private bool flip = false;
    private float currentTime = 0f;
	private GUISkin skin;
	private GUIResolutionHelper resolutionHelper;
	private TextLocalizationManager translationManager;
	private SoundEffectsHelper soundEffects;

	private bool drawFlipMessage = false;
	private bool invokedFlip = false;
	private bool playedSound = false;

    //private Vector3 flipVerticalVector = new Vector3(0f,-1f,0f);
	//private Vector3 flipHorizontalVector = new Vector3(-1f,0f,0f);
	// Use this for initialization
	void Start () {

	  currentTime = 0.0f;
	  flip = false;
	  drawFlipMessage = false;

	  skin = Resources.Load("GUISkin") as GUISkin;

	  GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
	  if(scripts!=null) {
		resolutionHelper = scripts.GetComponent<GUIResolutionHelper>();
		translationManager = scripts.GetComponent<TextLocalizationManager>();
		soundEffects = scripts.GetComponent<SoundEffectsHelper>();
	  }
	}
	
	// Update is called once per frame
	void Update () {

	  currentTime+=Time.deltaTime;
	  if(currentTime>flipInterval) {

		   flip = !flip;
		   //time to flip
		   invokedFlip = false;
		   playedSound = false;



		   drawFlipMessage = flip;
		   currentTime = 0f;

	  }

	  if(flip && !playedSound) {

			playedSound = true;
			Debug.Log("PLAYING SOUND");
			soundEffects.PlayFlipTimeSound();

	  }

 
	  if(flip && !invokedFlip) {

		    Invoke("Flip",0.5f);
	
	  }


	}



	void Flip() {

	 if(!invokedFlip){

		drawFlipMessage = false;
		
		//Vector3 flipVector = gameObject.transform.localScale;
		//Vector3 pos = gameObject.transform.position;//was local position
	    if(flipXAxis) {
	       //flipVector.x*=-1;
			transform.RotateAround (transform.position, Vector3.up, angle);
	    }
	    else if(flipYAxis) {
	    //vertical
			//flipVector.y*=-1;
			transform.RotateAround (transform.position, Vector3.down, angle);
	    }
	    else if(flipZAxis) {
			//flipVector.z*=-1;
		
			transform.RotateAround (transform.position, Vector3.forward, angle);
			//transform.localScale = flipVector;
	    }
	    else {
	      Debug.Log("nothing to flip. Adjust your script");
	    }

		//gameObject.transform.localScale = flipVector;
		//pos.y-=gameObject.transform.renderer.bounds.size.y;
		//gameObject.transform.position = pos;
		invokedFlip = true;



	}

		
	}

	void OnGUI() {

	  if(drawFlipMessage) {

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

			if(isWideScreen) {
			  GUI.matrix = wideMatrix;
			}
			else {
			  GUI.matrix = normalMatrix;
			}


		    if(Event.current.type==EventType.Repaint) {

			 	DrawText("Flip Time!", //text
			 	40, //font size
			 	resolutionHelper.screenWidth/2-100, //x
			 	resolutionHelper.screenHeight/2,  //y
			 	200,  //width
			 	50);//height

			}

	  }
	}


	public void DrawText(string text, int fontSize, float x, float y,int width,int height) {
		
		
		GUIStyle centeredStyleSmaller = GUI.skin.GetStyle("Label");
		centeredStyleSmaller.alignment = TextAnchor.MiddleLeft;
		//centeredStyleSmaller.font = messagesFont;
		centeredStyleSmaller.fontSize = fontSize;
		
		GUI.Label(new Rect(x, y, width, height), text);
	}

	string GetTranslationKey(string key) {
		return	translationManager.GetText(key);
	}
}
