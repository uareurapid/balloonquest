using UnityEngine;
using System.Collections;

public class MoveTowardsScript : MonoBehaviour {

	public Transform target;//where to move after landing... center marker, near the wood sign
	private bool moveTowards = false;
	public float moveTowardsSpeed = 1.2f;
	private bool reachedTarget = false;
	private Vector3 targetPosition;

	// Use this for initialization
	void Start () {
	  targetPosition = target.position;
	}
	
	// Update is called once per frame
	void Update () {

		if(moveTowards && !reachedTarget) {
		 // The step size is equal to speed times frame time.
		 var step = moveTowardsSpeed * Time.deltaTime;
		 // Move our position a step closer to the target.
		 transform.position = Vector3.MoveTowards(transform.position, target.position, step);

			if( (Vector3.Distance(transform.position, target.position) < 0.1f) || transform.position.Equals(targetPosition) ){
 				//It is within ~0.1f range, do stuff
				reachedTarget = true;
				Debug.Log("Reached position");
 			}
	  }

	}

	public void StartMovingTowards(PlayerScript player, bool start) {
		moveTowards = start;
		bool isOnLeft = transform.position.x < target.position.x;
		if(isOnLeft && !player.IsPlayerFacingRight()) {
		  player.Flip();
		}
		else if(!isOnLeft && player.IsPlayerFacingRight()) { //is on the right and facing right, also need to flip
		  player.Flip();
		}
	}

	public bool HasReachedTarget() {
	  return reachedTarget;
	}

}
