using UnityEngine;
using System.Collections;

public class GroundScript : MonoBehaviour {

	public bool isVisible = false;
	private MovingPlatformScript platformScript;
	private GameControllerScript controller;
	// Use this for initialization
	void Start () {
	
		GameObject ground = GameObject.FindGameObjectWithTag("Ground");
		platformScript = ground.GetComponent<MovingPlatformScript>();
		controller = GameObject.FindGameObjectWithTag("Scripts").GetComponent<GameControllerScript> ();

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
			//script.DisableMovement();
		}
	}

	void DisableScrolling() {
		controller.DisableScrolling();
	}

	void DisableSpawning() {
		controller.DisableSpawning();
	}
	
	void StopMovement() {
		platformScript.enabled = false;
	}
	

}
