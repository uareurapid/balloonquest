using UnityEngine;
using System.Collections;

public class BounceScript : MonoBehaviour {

	public float bounceForce = 10f;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D hit) 
	{ 
		if (hit.gameObject.tag.Equals("Player") ){ 
		  hit.rigidbody.AddForce(new Vector2(0f,bounceForce), ForceMode2D.Impulse); 
	    } 
	}

}
