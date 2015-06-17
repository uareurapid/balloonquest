using UnityEngine;
using System.Collections;

public class LeverPuller : MonoBehaviour {

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
	    GameObject lever = GameObject.FindGameObjectWithTag("Lever1");
	    lever.GetComponent<Animation>().Play("right");
			GameObject ladder = GameObject.FindGameObjectWithTag("MoveLadder");
			ladder.GetComponent<MovingPlatformScript>().UnlockMovement();
	  }
	}

}
