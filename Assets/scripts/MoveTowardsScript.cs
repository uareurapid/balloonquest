using UnityEngine;
using System.Collections;

public class MoveTowardsScript : MonoBehaviour {

    //if there is a tag, find the associated game object
    public string targetTag;

	public Transform target;//where to move after landing... center marker, near the wood sign
	public bool startMoveTowards = false;
	public float moveTowardsSpeed = 1.3f;
	private bool reachedTarget = false;
	public bool adjustYOnStart = false;
	public bool adjustXOnStart =  false;
	public bool adjustZOnStart =  true;
	public bool destroyWhenReach = false;
	//in this case the movement is not automatic, but manual
	public bool allowManualMovement = true;

	// Use this for initialization
	void Start () {
	  if(targetTag!=null && target==null) {
	    target = GameObject.FindGameObjectWithTag(targetTag).transform;
	  }
	}
	
	// Update is called once per frame
	void Update () {

	 if(startMoveTowards && !reachedTarget && target!=null) {

		 if(!allowManualMovement) {

			// The step size is equal to speed times frame time.
		 	var step = moveTowardsSpeed * Time.deltaTime;
		 	// Move our position a step closer to the target.
		 	transform.position = Vector3.MoveTowards(transform.position, target.position, step);


		 }


		if( Mathf.Abs(transform.position.x - target.position.x) < 0.1f ){
 			//It is within ~0.1f range, do stuff
			reachedTarget = true;
			Debug.Log("Reached position");
			if(destroyWhenReach) {
			  Destroy(gameObject);
			}
 		}
	 }

	}

	public void StartMovingTowards(PlayerScript player, bool start) {
		startMoveTowards = start;
		//*********************************************************
		Vector3 aux = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		//maybe adjust to make smoother transitions, and avoid jumps if in top of platform collider...
		if (adjustYOnStart) {
			aux.y = target.position.y;
		}
		
		if (adjustZOnStart) {
			aux.z = target.position.z;
		}
		
		if (adjustXOnStart) {
			aux.x = target.position.x;
		}
		
		transform.position = aux;
		//*********************************************************

		bool isOnLeft = transform.position.x < target.position.x;
		if(isOnLeft && !player.IsPlayerFacingRight()) {
		  player.Flip();
		}
		else if(!isOnLeft && player.IsPlayerFacingRight()) { //is on the right and facing right, also need to flip
		  player.Flip();
		}
	}

	public void StartMovingTowards(bool start) {
		startMoveTowards = start;
		//*********************************************************
		Vector3 aux = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		//maybe adjust to make smoother transitions, and avoid jumps if in top of platform collider...
		if (adjustYOnStart) {
			aux.y = target.position.y;
		}
		
		if (adjustZOnStart) {
			aux.z = target.position.z;
		}
		
		if (adjustXOnStart) {
			aux.x = target.position.x;
		}
		
		transform.position = aux;
		//*********************************************************

		/*bool isOnLeft = transform.position.x < target.position.x;
		if(isOnLeft && !player.IsPlayerFacingRight()) {
		  player.Flip();
		}
		else if(!isOnLeft && player.IsPlayerFacingRight()) { //is on the right and facing right, also need to flip
		  player.Flip();
		}*/
	}

	public bool HasReachedTarget() {
	  return reachedTarget;
	}

}
