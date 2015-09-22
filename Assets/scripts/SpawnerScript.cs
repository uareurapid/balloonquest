using UnityEngine;
using System.Collections;
//using RescueJelly;

public class SpawnerScript : MonoBehaviour
{
	public float spawnTime = 5f;		// The amount of time between each spawn.
	public float spawnDelay = 2f;		// The amount of time before spawning starts.
	public GameObject[] enemies;		// Array of enemy prefabs.

	//the parent of the spawned child
	public Transform spawnedParent;

	public Texture2D[] thumbnails;
	
    public bool canSpawn = false;
    
    //index of the next thumbnail
	private int nextThumbnail=-1;
    
    
	Vector3 newPosition = new Vector3(0f,0f,0f);
	//margin regarding the transform position
	public int marginUp = 5;
	public int marginDown = 5;
	public int marginLeft = 0;
	public int marginRight = 0;

	public int maxSpawns = 20;
	private int spawnCount = 0;

	//shoudl the
	public bool cameraFollowSpawned = false;
	
	GUISkin skin;
	
	private GameObject nextSpawned;

	//aux var
	private bool spawnOnHold = false;

	void Start ()
	{
		// Start calling the Spawn function repeatedly after a delay .
		//skin = Resources.Load("GUISkin") as GUISkin;
		GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
		if(scripts!=null) {

		}
		else {

		}
		//Position of the spawner
		//transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2,Screen.height+10f,transform.position.z));
		//save the original position (world position);
		//originalPosition = Camera.main.WorldToScreenPoint(transform.position);
		// transform.position;

		InvokeRepeating("Spawn",spawnDelay,spawnTime);
	}
	
	void Awake() {
	   

	}
	
	void SpawnFirstThumbnail() {


		
		
	}



	//check if we need to follow something
	void CheckCameraFollow(Transform target) {
		//make the camera follow this new jelly
		/*if(cameraFollowSpawned) {
 			GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
 			if(camera!=null) {
			  SmoothFollow2D scriptFollow = camera.GetComponent<SmoothFollow2D>();
 			  if(scriptFollow!=null && scriptFollow.enabled) {
 			    scriptFollow.target = target;
 			  }
 			}
		}*/
	}
		 
	public void Spawn ()
	{

	  //check also if controller action is not stopped
	  //stop spawining if we are stopped
	  if(canSpawn ) { //controller.SpawnAllowed() check the number of jellies
	  
			int spawnedIndex = 0;
			if(enemies.Length>1) {
				spawnedIndex = Random.Range(0,enemies.Length);
			}
	        //Added random position (-40,+40) for the spawn position
				  float randomX = Random.Range(transform.position.x - marginLeft, transform.position.x + marginRight);
					newPosition.x = randomX;
					newPosition.y = transform.position.y;
					newPosition.z = transform.position.z;
	        //the spawned object will have the same rotation of the spawner object itself
				    nextSpawned = (GameObject)Instantiate(enemies[spawnedIndex], newPosition, transform.rotation);
					
					if(spawnedParent!=null) {
						nextSpawned.transform.parent = spawnedParent;
					}
				
				    spawnCount+=1;
					//CheckCameraFollow(nextJelly.transform);
	           
	        

	  
			
			//canSpawn = false;
			// Play the spawning effect from all of the particle systems.
			//foreach(ParticleSystem p in GetComponentsInChildren<ParticleSystem>())
			//{		    
			//	p.transform.position = newPosition;
			//	p.Play();
			//}
			//SoundEffectsHelper.Instance.MakeRandomizeSound();
	  }
		
	}
	
	void Update() {

	  if(spawnCount>=maxSpawns) {
	    	canSpawn = false;
			CancelInvoke("Spawn");
	  }
		/*if(controller.SpawnAllowed()) {
		//check if we are performing zooming operations
			CameraZoomInOutScript camScript = Camera.main.GetComponent<CameraZoomInOutScript>();
			if(camScript!=null && camScript.IsCameraZoming()) {
			  canSpawn = false;
			}
			else if(spawnOnHold) {
			  canSpawn = false;

			}
			else {
			  canSpawn = true;
			}
			
		}
		else {
		    canSpawn = false;
		}*/
	}
	
	void OnGUI(){
		/*GUI.skin = skin;
		
		Matrix4x4 svMat = GUI.matrix;//save current matrix
		
		int width = GUIResolutionHelper.Instance.screenWidth;
		Vector3 scaleVector = GUIResolutionHelper.Instance.scaleVector;
		bool isWideScreen = GUIResolutionHelper.Instance.isWidescreen;
		int increaseFactor = 0;
		
		if(isWideScreen) {
			GUI.matrix = Matrix4x4.TRS(new Vector3( (GUIResolutionHelper.Instance.scaleX - scaleVector.y) / 2 * width, 0, 0), Quaternion.identity, scaleVector);
			increaseFactor = 100;
		}
		else {
			
			GUI.matrix = Matrix4x4.TRS(Vector3.zero,Quaternion.identity,scaleVector);
		}

		//don´t show nothing if game not running!
		if(!controller.IsGameOver()) {
			//was   //740
			DrawText(translationManager.GetText(GameConstants.MSG_NEXT) + " ", controller.messagesFontSizeSmaller +10,740, 10,220,50);//Screen.width-150
		
			if(nextThumbnail>-1) {//TODO &&player still alive
				Rect next = new Rect(850,20,32,32);//was ,850Screen.width-50+increaseFactor
				GUI.DrawTexture(next, thumbnails[nextThumbnail]);
			}
		}


		
		//restore the matrix
		GUI.matrix = svMat;*/

	}
	
	public void DrawText(string text, int fontSize, int x, int y,int width,int height) {
		
		/*
		GUIStyle centeredStyleSmaller = GUI.skin.GetStyle("Label");
		centeredStyleSmaller.alignment = TextAnchor.MiddleLeft;
		centeredStyleSmaller.font = controller.messagesFont;
		centeredStyleSmaller.fontSize = fontSize;
		
		GUI.Label (new Rect(x, y, width, height), text);*/
	}
	
	void OnBecameVisible() {
		canSpawn = true;
	}
	
	void OnBecameInvisible() {
		canSpawn = false;
	}

	public void PutSpawnOnHold(bool hold) {
	 spawnOnHold = hold;
	 canSpawn = !hold;
	 if(canSpawn && !spawnOnHold) {
	   Spawn();
	 }
	}

    public bool IsSpawningOnHold() {
      return spawnOnHold;
    }

		//alows spawning again
	public void UnlockSpawning() {
		
		//or maybe as workaround have a dummy object in same position of spawner
		/*SmoothFollow2D cameraFollow = Camera.main.GetComponent<SmoothFollow2D>();
		if(cameraFollow!=null && cameraFollow.enabled) {
			cameraFollow.ResetCameraPosition();
		}

		if(spawnOnHold) {
			//allow follow the jelly again (was following the platform)
		 	cameraFollowSpawned = true;
		 	PutSpawnOnHold(false);
		}*/
		
	}
	//override previous
	/*public void UnlockSpawning(SmoothFollow2D cameraFollow) {
		
		//or maybe as workaround have a dummy object in same position of spawner
		if(cameraFollow.enabled) {
			cameraFollow.ResetCameraPosition();
		}

		if(spawnOnHold) {
			//allow follow the jelly again (was following the platform)
		 	cameraFollowSpawned = true;
		 	PutSpawnOnHold(false);
		}
		
	}*/

}
