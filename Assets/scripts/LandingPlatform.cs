using UnityEngine;
using System.Collections;

public class LandingPlatform : MonoBehaviour {

    public bool isDetachable = false;
	PlayerScript player;
	BoxCollider2D colliderBox;
	// Use this for initialization
	void Start () {

	 GameObject pl = GameObject.FindGameObjectWithTag("Player");
	 if(pl!=null) {
		player = pl.GetComponent<PlayerScript>();
	 }

	 colliderBox = GetComponent<BoxCollider2D>();

	}
	
	// Update is called once per frame
	void Update () {

	  if(colliderBox!=null && player!=null) {
	    colliderBox.enabled = (player.IsPlayerFalling() && !player.PlayerHasBalloon());
	  }
	}
	
	
	void OnCollisionEnter2D(Collision2D collision)
	{
		  //go down
		  HandleCollision(collision.gameObject);
		
	}

	private void HandleCollision(GameObject gameObject) {

		PlayerScript playerObj = gameObject.GetComponent<PlayerScript>();
		if(playerObj!=null && playerObj.IsPlayerFalling()) {
			playerObj.PlayerLandedOnPlatform(true);
		}//otherwise ignore

	}

	void OnTriggerEnter2D(Collider2D collision) {
		
		  HandleCollision(collision.gameObject);
		
	}



}
