using UnityEngine;
using System.Collections;

public class BeeHiveScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//
	void OnCollisionEnter2D(Collision2D collision)
	{
		GameObject collisionObject = collision.gameObject;
		PlayerScript player = collisionObject.GetComponent<PlayerScript>();
		if(player!=null) {
		  FallenTreeScript fall = GetComponent<FallenTreeScript>();
		  if(!fall.enabled) {
			fall.enabled = true;
		  }
		}

	}
}
