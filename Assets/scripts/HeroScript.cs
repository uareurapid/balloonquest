using UnityEngine;
using System.Collections;

public class HeroScript : MonoBehaviour {

	private bool isVisible = true;
	PlayerScript player;
	private bool startedMovingTowards = false;
	private bool isBlinkingHit = false;

	// Use this for initialization
	void Start () {
	
		GameObject playerObj = GameObject.FindGameObjectWithTag ("Player");
		player = playerObj.GetComponent<PlayerScript> ();
		isBlinkingHit = false;
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
		if (!isBlinkingHit && player.IsPlayerAlive() && !player.PlayerTouchedGround()) {
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
	     Debug.Log("PLAYING PARTICLE AURA");
	     part.Play(true);
	   }
	   else Debug.Log("IS NULL!!!!!!");
	  }
	  return reached ;
	}

	public void BlinkWhenHit() { 
	 isBlinkingHit = true; //i need to know this otherwise OnBecameInvisble is called and the gameobject will get destroyed
	 //Player invisible for some Time 
	 StartCoroutine(HideSprite(0.5f));
	 StartCoroutine(MyWaitMethod(2.2f));
	 StartCoroutine(ShowSprite(0.5f));
	  //Player visible again
	 

	}

	IEnumerator HideSprite(float length) {

		for (float i = 0.0f; i < 1.0f; i += Time.deltaTime*(1/length)) {
			GetComponent<SpriteRenderer>().enabled = false; 
			yield return null;
		}

	}

	IEnumerator ShowSprite(float length) {

		for (float i = 0.0f; i < 1.0f; i += Time.deltaTime*(1/length)) {
			GetComponent<SpriteRenderer>().enabled = true; 

			yield return null;
		

		}isBlinkingHit = false;

	}

	IEnumerator MyWaitMethod(float seconds) {
		yield return new WaitForSeconds(seconds);
	} 

}
