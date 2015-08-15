﻿using UnityEngine;
using System.Collections;

public class GroundScript : MonoBehaviour {

	public bool isVisible = false;
	private MovingPlatformScript platformScript;
	// Use this for initialization
	void Start () {
	
		GameObject ground = GameObject.FindGameObjectWithTag("Ground");
		platformScript = ground.GetComponent<MovingPlatformScript>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnBecameInvisible() {
	  if(!isVisible) {
	   return;
	  }
	  isVisible = false;
	}
	
	void OnBecameVisible() {

	if(isVisible) {
	 return; //UNITY BUG
	}

	//isVisible = true;

	Debug.Log("VISIBLE NOW: " + gameObject.tag);
		//disable movement when collider becames visible
		isVisible = true;
		if (platformScript != null) {
			//Stop the platform
			StopMovement ();

			//enable player gravity scale to touch the ground
			MakePlayerFall ();
		
			//disable the layers scrolling
			DisableScrolling ();

			//maybe show some fireworks here? ;.)
			DisableSpawning();

			 
		} 
	}

	//disables movement and set gravity scale to 1.0f
	void MakePlayerFall() {

		GameObject player = GameObject.FindGameObjectWithTag("Player");
		if(player!=null) {
			PlayerScript script = player.GetComponent<PlayerScript>();
			script.EnableGravityScale();
			script.DisableMovement();
		}
	}

	void DisableScrolling() {
		ScrollingScript[] scrolls= FindObjectsOfType(typeof(ScrollingScript)) as ScrollingScript[];
		foreach (ScrollingScript scroll in scrolls) {
			scroll.enabled = false;
		}
	}

	void DisableSpawning() {
		GameObject [] spawners = GameObject.FindGameObjectsWithTag ("Spawner");
		if (spawners != null && spawners.Length > 0) {
			foreach(GameObject spawner in spawners) {
				SpawnerScript script = spawner.GetComponent<SpawnerScript>();
				if(script!=null) {
					script.canSpawn = false;
				}
			}
		}
	}


	
	void StopMovement() {
		platformScript.enabled = false;
	}
	

}
