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

	//a reference Object, to spawn at the same location, or calculated!
	public Transform spawnReferencePoint;
	//the distance in y to the referece point
	public float yDistanceToReferencePoint = 0;

	public Texture2D[] thumbnails;

    public bool canSpawn = false;

    //add a specific z index to the spawned
    public float zIndex = 0f;
    
    //index of the next thumbnail
	private int nextThumbnail=-1;
	//how many can i spawn at the same time
    public int maxSpawnsSameTime = 1;
	public int minSpawnsSameTime = 1;
    
	
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

		/*GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
		if(scripts!=null) {

		}
		else {

		}*/
		spawnCount = 0;

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

	      
	      int numSpawns = 1;
	      //if i have 2 different values, get a random between
	      if(minSpawnsSameTime < maxSpawnsSameTime) {
			numSpawns= Random.Range(minSpawnsSameTime,maxSpawnsSameTime+1);
	      }

	      for(int i = 0; i < numSpawns; i++) {

			int spawnedIndex = 0;
			if(enemies.Length>1) {
				spawnedIndex = Random.Range(0,enemies.Length);
			}
					//TODO, i´m not considering Y
				  	float randomX = Random.Range(transform.position.x - marginLeft, transform.position.x + marginRight);

					Vector3 newPosition = new Vector3(0f,0f,0f);

					newPosition.x = randomX;

					if(spawnReferencePoint!=null && yDistanceToReferencePoint !=0) {
						//the position of the reference point, if any
						Vector3 referencePosition = spawnReferencePoint.position;
						newPosition.y = referencePosition.y + yDistanceToReferencePoint; //yDistanceToReferencePoint could be negative
					}
					else {
						newPosition.y = transform.position.y;
					}

					if(zIndex != 0f) {
						newPosition.z =  zIndex;
					}
					else {
						newPosition.z = transform.position.z;
					}


	        		//the spawned object will have the same rotation of the spawner object itself
				    nextSpawned = (GameObject)Instantiate(enemies[spawnedIndex], newPosition, transform.rotation);
					
					if(spawnedParent!=null) {
						nextSpawned.transform.parent = spawnedParent;
					}
				
				    spawnCount+=1;
					//CheckCameraFollow(nextJelly.transform);
	           
	        


	      }
			

		if(spawnCount>=maxSpawns) {
	    	canSpawn = false;
			CancelInvoke("Spawn");
	  	}
			
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

    public void EnableSpawn() {
    	canSpawn = true;
		spawnCount = 0;
		InvokeRepeating("Spawn",spawnDelay,spawnTime);
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
