using UnityEngine;
using System.Collections;

public class BeeScript : MonoBehaviour {

    public bool isAttacking = false;
	public float attackDuration = 4.0f; //4 seconds
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	  
	}

	//start attacking (or stop it too)
	public void Attack(bool attack) {

		isAttacking = attack;
		if(isAttacking) {
		    GetComponent<Animator>().enabled = true;//show blinking
			GetComponent<EnemyScript>().enabled = true;//behave as enemy
			GetComponent<CircleCollider2D>().enabled = true;
			Invoke("StopAttack",attackDuration);
		}
		else {
			GetComponent<Animator>().enabled = false;
			GetComponent<EnemyScript>().enabled = false;
			GetComponent<CircleCollider2D>().enabled = false;
		}
	}

	//stop attacking
	void StopAttack() {
		Attack(false);
	}


}
