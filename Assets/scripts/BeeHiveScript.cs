using UnityEngine;
using System.Collections;

public class BeeHiveScript : MonoBehaviour {

	private bool isVisible = false;
	//destroy on became invisible;
	public bool autoDestroy = true;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnBecameInvisible() {
		if(!isVisible) {
			return;
		}
		isVisible = false;
		MakeBeesAttack();
		if(autoDestroy) {
			//destroy the hive
			Destroy(gameObject);
		}

	}
	
	void OnBecameVisible() {
		
		if (isVisible) {
			return; //UNITY BUG
		}

		isVisible = true;

	}

	//the fallen script is set to autodestroy object on beehives
	void OnDestroy() {
		//MakeBeesAttack();
	}

	void MakeBeesAttack() {
	    //get all bees that belong to this hive
		BeeScript[] bees = gameObject.GetComponentsInChildren<BeeScript>();// ("Bee");
		Debug.Log ("FOUND " + bees.Length + " bees!");
		foreach (BeeScript bee in bees) {
		    //get them out of the hive
			bee.gameObject.transform.SetParent(gameObject.transform.parent);
			bee.Attack(true);
		}


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
				fall.enabled = true;
				fall.StartFallingAfterDelay();
			}
		}
	}
}
