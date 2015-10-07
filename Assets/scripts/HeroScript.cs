﻿using UnityEngine;
using System.Collections;

public class HeroScript : MonoBehaviour {

	private bool isVisible = true;
	PlayerScript player;
	private bool startedMovingTowards = false;
	private bool isBlinkingHit = false;

	private int burnedCount = 0;
	private bool hasOpenedChest = false;

	private GameObject skull;

	// Use this for initialization
	void Start () {
	
		GameObject playerObj = GameObject.FindGameObjectWithTag ("Player");
		skull = GameObject.FindGameObjectWithTag("Skull");
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

	}

	public bool IsPlayerAlive() {
	  return player!=null && player.IsPlayerAlive();
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
			Debug.Log("COLLIDED HERO");
		    if(enemy.isBurner && skull!=null) {
		     //TODO
		     Debug.Log("start burn");
			 if(burnedCount == 0) {
				InvokeRepeating("SwapBurnedSprite",0f,0.2f);
		     	StartCoroutine(StartBurnAndDieAnimation());
			 }
		     
		    }

		    else {
		     //if the ground is still not visible, kill it!!!
		     if(!player.IsGroundVisible()){
				player.KillPlayer ();
		     }
		     else {
		        //ok, ground is visible, but i´m not falling, so die mother fucker die!!!
				if(!player.IsPlayerFallingToGround()) {
					player.KillPlayer ();
				}
		     }
				
		    

		    }

		} 
		else {
			//is a gem?
			GemScript gem = collisionObject.GetComponent<GemScript>();
			if(gem!=null && player!=null) {
				player.HandleGemCollision(gem);
			}
		}
		
		
	}	

	void SwapBurnedSprite() {
	  burnedCount+=1;
		SpriteRenderer skullRenderer = skull.GetComponent<SpriteRenderer>();
		skullRenderer.enabled = !skullRenderer.enabled;

		GetComponent<SpriteRenderer>().enabled = !skullRenderer.enabled;

		if(burnedCount>=5) {
			GetComponent<SpriteRenderer>().enabled = true;
			skullRenderer.enabled = false;
			CancelInvoke("SwapBurnedSprite");
		}
	}

	IEnumerator StartBurnAndDieAnimation()
	{
		yield return new WaitForSeconds(2);
		player.KillPlayer ();
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

	   if(hasOpenedChest) {
			//play the teleportation effect
			ParticleSystem part = GetComponentInChildren<ParticleSystem>();
			if(part!=null) {
				part.Play(true);
			}


			HidePointer();
	   }
	   else {
		//say it did not reached yet
		reached = false;
	   }
	   
	  }
	  return reached ;
	}

	void HidePointer() {
		//shows a pointer blinking

		GameObject obj = GameObject.FindGameObjectWithTag("pointer");
		if(obj!=null) {
		  BlinkSpriteScript blink = obj.GetComponent<BlinkSpriteScript>();
		  if(blink) {
		    blink.DisableBlink();
		  }
		}
	
	}
	public void BlinkWhenHit() { 
	 isBlinkingHit = true; //i need to know this otherwise OnBecameInvisble is called and the gameobject will get destroyed
	 //Player invisible for some Time 
	 StartCoroutine(HideSprite(0.5f));
	 StartCoroutine(MyWaitMethod(2.2f));
	 StartCoroutine(ShowSprite(0.5f));
	  //Player visible again
	 

	}

	public void BurnHero(bool dieAfterBurn) {
		Debug.Log ("BURNING NOW");
		if(skull!=null) {
			//TODO
			Debug.Log("start burn");
			if(burnedCount == 0) {
				InvokeRepeating("SwapBurnedSprite",0f,0.3f);
				if(dieAfterBurn) {
					StartCoroutine(StartBurnAndDieAnimation());
				}

			}
			
		}
	}

	public void OpenChest(bool open) {
		hasOpenedChest = open;
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
