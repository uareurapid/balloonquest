using UnityEngine;
using System.Collections;

public class ParachuteScript : MonoBehaviour {

	PlayerScript player;
	// Use this for initialization
	void Start () {
		GameObject playerObj = GameObject.FindGameObjectWithTag ("Player");
		player = playerObj.GetComponent<PlayerScript> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log ("PARACHUTE TRIGGER WITH" + collision.gameObject.tag);
		PerformUpdate(collision.gameObject);
	}
	
	void OnCollisionEnter2D(Collision2D collision)
	{
		Debug.Log ("PARACHUTE COLLISION WITH" + collision.gameObject.tag);
		PerformUpdate(collision.gameObject);
	}

	void PerformUpdate(GameObject collisionObject) {
		

		EnemyScript enemy = collisionObject.GetComponent<EnemyScript> ();
		//collided with enemy
		if (enemy != null && player!=null) {
				
			if (player.PlayerHasParachute ()) {
				Debug.Log ("burst patrachute");
				player.BurstParachute ();
			} 

		}


	}
}
