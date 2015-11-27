using UnityEngine;
using System.Collections;

public class PickupCoinScript : MonoBehaviour {

	GameControllerScript gameEngine;
	SoundEffectsHelper sfx;
	SpecialEffectsHelper se;
	public int points = 2;
	// Use this for initialization
	void Start () {
	  GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
	  if(scripts!=null) {
		sfx = scripts.GetComponentInChildren<SoundEffectsHelper>();
		se = scripts.GetComponentInChildren<SpecialEffectsHelper>();
		gameEngine = scripts.GetComponent<GameControllerScript>();
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

			HandleCollisionWithPlayer(player);

		}//else could be the parachute or the hero objects
		else {
			//Get the reference for the player
			GameObject p = GameObject.FindGameObjectWithTag("Player");
			if(p!=null) {
				player = p.GetComponent<PlayerScript>();
			}
			
			
			ParachuteScript parachute = collisionObject.GetComponent<ParachuteScript>();
			if(parachute!=null) {
				if (player!=null && player.PlayerHasParachute ()) {
					HandleCollisionWithPlayer(player);
				} 
			}
			else {
				HeroScript hero = collisionObject.GetComponent<HeroScript>();
				if(player!=null && hero!=null)  {
					HandleCollisionWithPlayer(player);
				}
			}
		}
   }

   void HandleCollisionWithPlayer(PlayerScript player) {
		if (sfx != null) {
		   sfx.PlayPickupCoinSound();
		}
		if(se!=null) {
		   se.PlayCoinBurstEffect(transform.position);
		   se.Play2PointsEffect(transform.position);
		}

		if(gameEngine!=null) {
			gameEngine.IncreaseScoreBy(points);
		}

		player.IncreaseCoins(1);
		Destroy(gameObject);
	}

	void OnDestroy() {
    Debug.Log("Destroying coin");
   }
}
