using UnityEngine;
using System.Collections;

public class MonkeyBomberScript : MonoBehaviour {

    public bool startBombingOnVisible = true;
	private bool isVisible = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//TODO; IS NOT TURNING BACK ANYMORE
	void OnBecameInvisible (){
		if(!isVisible) {
			return;
		}
		
		isVisible = false;
	}

	void OnBecameVisible (){
		if(isVisible) {
			return;
		}

		isVisible = true;
		if(startBombingOnVisible) {
			SpawnerScript bombSpawner = GetComponentInChildren<SpawnerScript>();
			if(bombSpawner!=null) {
			  bombSpawner.EnableSpawn();
			}
		}

	}
}
