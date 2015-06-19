using UnityEngine;
using System.Collections;

public class PickupCoinScript : MonoBehaviour {

	SoundEffectsHelper sfx;
	// Use this for initialization
	void Start () {
	  GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
	  if(scripts!=null) {
	    sfx = scripts.GetComponent<SoundEffectsHelper>();
	  }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D collision)
	{

	  PerformUpdate(collision.gameObject);
	}

	void OnTriggerEnter2D(Collider2D collision)
	{

	  PerformUpdate(collision.gameObject);
   }
   
   void PerformUpdate(GameObject collisionObject) {
   
		PlayerScript player = collisionObject.GetComponent<PlayerScript>();
		if(player!=null) {
			if(sfx!=null) {
				sfx.PlayPickupCoinSound();
			}
			player.IncreaseCoins(1);
			Destroy(gameObject);
		}
   }
}
