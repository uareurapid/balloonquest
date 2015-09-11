using UnityEngine;
using System.Collections;

public class SnakeScript : MonoBehaviour {

	public bool enableMovementOnlyVisible = true;

	private bool isVisible = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnBecameVisible() {
		if(isVisible) {
			return; //UNITY BUG
		}
		
		isVisible = true;
		EnableMovement();
		
	}

	void EnableMovement() {
		MoveScript move = GetComponent<MoveScript> ();
		if (move != null) {
			move.enabled = true;
		}
	}
	
	//TODO; IS NOT TURNING BACK ANYMORE
	void OnBecameInvisible (){
		
		if(!isVisible) {
			return;
		}
		
		isVisible = false;
	}
}
