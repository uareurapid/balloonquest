using UnityEngine;
using System.Collections;

public class BeeHiveScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		CheckIfPlayer(otherCollider.gameObject);
	}

	//
	void OnCollisionEnter2D(Collision2D collision)
	{
		CheckIfPlayer(collision.gameObject);


	}

	void CheckIfPlayer(GameObject collisionObject){

		PlayerScript player = collisionObject.GetComponent<PlayerScript>();
		if(player!=null) {

		    ShakeScript shake = GetComponent<ShakeScript>();
		    if(shake!=null) {
		      shake.enabled = true;
		      shake.StartShaking(1.5f);
		    }

			FallenTreeScript fall = GetComponent<FallenTreeScript>();
			if(!fall.enabled) {
				Debug.Log("Enable Fallen tree script on bee hive!!!");
				fall.enabled = true;
				fall.StartFallingAfterDelay();
			}
		}
	}
}
