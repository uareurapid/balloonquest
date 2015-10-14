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
			MakePlayerFallToLand ();
		
			//disable the layers scrolling
			DisableScrolling ();

			//maybe show some fireworks here? ;.)
			DisableSpawning();

			 
		} 
	}

	//disables movement and set gravity scale to 1.0f
	void MakePlayerFallToLand() {

		GameObject player = GameObject.FindGameObjectWithTag("Player");
		if(player!=null) {
			PlayerScript script = player.GetComponent<PlayerScript>();
			script.MakePlayerFallToLand();
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
	
	/*void OnTriggerEnter2D(Collider2D otherCollider)
	{

		PlayerScript player = otherCollider.gameObject.GetComponent<PlayerScript>();
		if(player!=null) {
		  player.HandleGroundCollision(isVisible);
		}
		else {
		  HeroScript hero = otherCollider.gameObject.GetComponent<HeroScript>();
		  if(hero!=null) {
			player = otherCollider.GetComponent<PlayerScript>();
			player.HandleGroundCollision(isVisible);
		  }
		}
	}

	//handle the collision with another sprite (not other trigger)
	void OnCollisionEnter2D(Collision2D collision)
	{
		PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
		if(player!=null) {
		  player.HandleGroundCollision(isVisible);
		}
		else {
			HeroScript hero = collision.gameObject.GetComponent<HeroScript>();
		  if(hero!=null) {
				player = collision.gameObject.GetComponent<PlayerScript>();
			player.HandleGroundCollision(isVisible);
		  }
		}
	}*/
}
