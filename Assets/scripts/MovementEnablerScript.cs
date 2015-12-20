using UnityEngine;
using System.Collections;

public class SnakeScript : MonoBehaviour {

	public bool enableMovementOnlyVisible = true;
	public float enableDelay = 0f;
	private bool isVisible = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnBecameVisible() {
		Debug.Log("OnBecameVisible!!");
		if(isVisible) {
			return; //UNITY BUG
		}
		
		isVisible = true;

		if(enableDelay==0f) {
			EnableMovement();//enable right now
		}
		else {
		 StartCoroutine(EnableDelayedMovement());//only after the delay
		}

		
	}

	IEnumerator EnableDelayedMovement() {

	  yield return new WaitForSeconds(enableDelay);
	  EnableMovement();
	}

	void EnableMovement() {
	  Debug.Log("enable movement!!");
		MoveScript move = GetComponent<MoveScript> ();
		if (move != null) {
			move.enabled = true;
		}
	}
	
	//TODO; IS NOT TURNING BACK ANYMORE
	void OnBecameInvisible (){
		Debug.Log("OnBecameInvisible!!");
		if(!isVisible) {
			return;
		}
		
		isVisible = false;
	}
}
