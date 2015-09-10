using UnityEngine;
using System.Collections;


public class AddForceScript : MonoBehaviour {

    //add this force to another collider object (trigger)

    public Vector2 forceVector;
    public float moveForce = 0.4f;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//some other trigger enters the attached collider
	void OnTriggerEnter2D(Collider2D otherCollider) {
		AddForce(otherCollider.gameObject);
	}

	void AddForce(GameObject gameObject) {
		TakeForceScript take = gameObject.GetComponent<TakeForceScript>();
		if(take!=null) {
		Debug.Log("apply force");
			take.TakeForce(forceVector * moveForce);
		}
	}


}
