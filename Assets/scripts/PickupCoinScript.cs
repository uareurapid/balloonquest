using UnityEngine;
using System.Collections;

public class PickupCoinScript : MonoBehaviour {

	AudioSource source;
	// Use this for initialization
	void Start () {
	  source =  gameObject.GetComponent<AudioSource>();
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
			if(source!=null && !source.isPlaying) {
				source.Play();
			}
			player.IncreaseCoins(1);
			Destroy(gameObject);
		}
   }
}
