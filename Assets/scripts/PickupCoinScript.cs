using UnityEngine;
using System.Collections;

public class PickupCoinScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D collision)
	{

	  PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
	  if(player!=null) {
	    player.IncreaseCoins(1);
	    Destroy(gameObject);
	  }
	}

	void OnTriggerEnter2D(Collider2D collision)
	{

	  PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
	  if(player!=null) {
	    player.IncreaseCoins(1);
	    Destroy(gameObject);
	  }
	}
}
