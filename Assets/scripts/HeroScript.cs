using UnityEngine;
using System.Collections;

public class HeroScript : MonoBehaviour {

	private bool isVisible = true;
	PlayerScript player;
	private bool startedMovingTowards = false;

	// Use this for initialization
	void Start () {
	
		GameObject playerObj = GameObject.FindGameObjectWithTag ("Player");
		player = playerObj.GetComponent<PlayerScript> ();
	}
	
	// Update is called once per frame
	void Update () {
	  
	}

	void OnBecameVisible() {
		isVisible = true;
	}
	
	void OnBecameInvisible() {
		isVisible = false;
		player.setVisible(false);
		if (!player.CanPlayerMove() && player.IsPlayerAlive() && !player.PlayerReleasedBalloon()) {
			player.KillPlayer();
		}
		//else {
		//  moveTowardsSign = true;
		//}
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		PerformUpdate(collision.gameObject);
	}
	
	void OnCollisionEnter2D(Collision2D collision)
	{
		PerformUpdate(collision.gameObject);
	}
	
	void PerformUpdate(GameObject collisionObject) {
		
		
		EnemyScript enemy = collisionObject.GetComponent<EnemyScript> ();
		//collided with enemy
		if (enemy != null && player != null && player.IsPlayerAlive ()) {

			player.KillPlayer ();

		} 
		else {
			//is a gem?
			GemScript gem = collisionObject.GetComponent<GemScript>();
			if(gem!=null && player!=null) {
				player.HandleGemCollision(gem);
			}
		}
		
		
	}	

	//start moving towards the level sign
	public void StartMovingTowardsSign() {
		MoveTowardsScript moveTowards =  GetComponent<MoveTowardsScript>();
		if(moveTowards!=null) {
		  moveTowards.StartMovingTowards(player,true);
		  startedMovingTowards = true;
		}
	}

	public bool HasStartedMovingTowards() {
	 return startedMovingTowards;
	}

	public bool HasHeroReachedTarget() {
	  MoveTowardsScript moveTowards =  GetComponent<MoveTowardsScript>();
	  bool reached = startedMovingTowards && moveTowards.HasReachedTarget();

	  if(reached) {
	   //play the teleportation effect
	   ParticleSystem part = GetComponentInChildren<ParticleSystem>();
	   if(part!=null) {
	     part.Play(true);
	   }
	  }
	  return reached ;
	}

}
