using UnityEngine;
using System.Collections;
/// <summary>
/// Simply moves the current game object
/// </summary>
public class MoveScript : MonoBehaviour
{
	// 1 - Designer variables
	
	/// <summary>
	/// Object speed
	/// </summary>
	public Vector2 speed = new Vector2(10, 10);
	
	/// <summary>
	/// Moving direction
	/// </summary>
	public Vector2 direction = new Vector2(-1, 0);
	
	private Vector2 movement;
	
	public bool alternateYMovement = false;
	public bool alternateXMovement = false;
	
	public bool chasePlayer = false;
	
	private float lastDirectionChange = 0f;
	private float changeDirectionInterval = 1.0f;
	
	//only for X axis
	private float lastXDirectionChange = 0f;
	private float changeXDirectionInterval = 1.0f;

	
	public int limitedMovementXInterval = 0;
	public int limitedMovementYInterval = 0;
	
	//This is related with the 2 intervals above, basically stops movement (with script still enabled)
	public bool stopOnMaxX=false;
	public bool stopOnMaxY=false;
	private bool stopMovement = false;
	
	public bool allowRevertXDirection = false;
	public bool allowRevertYDirection = false;
	//revert the transform, not just the direction
	public bool revertAlsoTransform = false;

	private bool revert = false;
	
	private GameObject target;
	
	private Vector3 startPosition;

	//public bool startOnlyIfVisible = false;
	private bool isVisible = false;

	//some thing that triggers the movement, even if the object is not visible
	//and we only allow to move if visible
	//NOTE: the SpriteRenderer associated to that object must be visible
	public GameObject movementTrigger = null;

	
	//allows the object to move even if not visible
	//by default is always false, so must be explicit set to true
	//like on asteroid spawner (of level 1) for instance
	public bool allowMoveIfNotVisible = false;
	private bool wasMovementTriggeredByOther = false;

	
	void Start() {
	
		revert = false;
		startPosition =  Camera.main.WorldToScreenPoint (transform.position);
	}
	
	void Awake() {
		target = GameObject.FindGameObjectWithTag("Player");
		
	}
	
	void CheckPositions() {

		Vector3 currentPosition = Camera.main.WorldToScreenPoint (transform.position);
		
		//check on X
		if(limitedMovementXInterval>0) {

			//moving to the left
			if(direction.x < 0 ) {

			  if(!revert) {
				if(currentPosition.x < startPosition.x - (0.0f + limitedMovementXInterval /*/2*/ ) ){
				
				    if(!stopOnMaxX) {
					  revert = true;
					  startPosition.x = currentPosition.x;
				    }
				    else {
				      stopMovement = true;
				    }
				
					
				 }
		
			  }
			
			
			
		   }
		   //moving to the right
		   else if(direction.x > 0) {
			
			  if(!revert) {
				if(currentPosition.x > startPosition.x + (0.0f+ limitedMovementXInterval /*/ 2*/) ){

				  //if not stop on max, then is time to revert direction
				   if(!stopOnMaxX) {
					revert = true;
					startPosition.x = currentPosition.x;
				   }
				   else {
				   //otherwise just stop
					 stopMovement = true;
				   }
					
				}
		
				
			  }	
			
		  }
		}
        else {
        //check on Y
		if(direction.y > 0 ) { 
			//is actually moving down

			  
			if(!revert) {
				if(currentPosition.y > startPosition.y + (0.0f- limitedMovementYInterval) ){
				
				 //if(!stopOnMaxY) {
					revert = true;
					startPosition.y = currentPosition.y;
				 //}
				 //else {
				   Debug.Log("stop movement");
				   stopMovement = true;
				 //}
				}
		
				
			  }
			
			
		   }
		   else if(direction.y < 0) {

			if(!revert) {
				if(currentPosition.y < startPosition.y - (0.0f - limitedMovementYInterval ) ){
					
					//if(!stopOnMaxY) {
						revert = true;
						startPosition.y = currentPosition.y;
					//}
					//else {
					  stopMovement = true;
					//}
				 }
		
			  }
		
		   }
        }//end check on y


		
		
	}

	bool AllowRevertDirection() {
		return allowRevertXDirection || allowRevertYDirection;
	}

	bool HasLimitedMovementInterval() {
		return limitedMovementXInterval > 0 || limitedMovementYInterval>0;
	}
	
	void Update()
	{

	  //if(!isVisible && startOnlyIfVisible) {
	  //  return;
	  //}

       if(!revert && (stopOnMaxX || stopOnMaxY) ) {
         CheckPositions();
         //if(!stopMovement) {
			movement = new Vector2(
			speed.x * direction.x,
			speed.y * direction.y);
         //}
		
       }
	   else if(!revert && ( AllowRevertDirection() &&  HasLimitedMovementInterval() ) ) {
	
			CheckPositions();
		   if(revert) {

		       if(limitedMovementXInterval>0) {
					// 1 - revert current direction on x axis
				    direction.x = direction.x*-1;
		       }
			   else {
		        	//revert is on y axis, instead
					direction.y = direction.y*-1;
		       }
				
				// 3 - Define new movement, based on new direction
				movement = new Vector2(
					speed.x * direction.x,
					speed.y * direction.y);
				revert = false;
		   }
		
			
		}

	  else if(allowMoveIfNotVisible && allowRevertXDirection && revert) {
	     
			// 1 - revert current direction on x axis
	            direction.x = direction.x*-1;
				// 3 - Define new movement, based on new direction
				movement = new Vector2(
					speed.x * direction.x,
					speed.y * direction.y);
				revert = false;

				if(revertAlsoTransform) {
					transform.localScale = new Vector3(transform.localScale.x* -1, transform.localScale.y, transform.localScale.z );
				}
	  }
	  else {
			//check if something triggered the movement, only when value is still false
			if(movementTrigger!=null && wasMovementTriggeredByOther==false) {
				wasMovementTriggeredByOther = IsMovementTriggerVisible();
			}


			if( allowMoveIfNotVisible || isVisible || wasMovementTriggeredByOther ) {
				
				lastDirectionChange += Time.deltaTime;
				lastXDirectionChange += Time.deltaTime;
				
				if(alternateYMovement && (lastDirectionChange >= changeDirectionInterval) ) {
					
					if(chasePlayer && target!=null) {
						Vector3 targetPosition = target.transform.position;
						if(targetPosition.y > transform.position.y) {
							direction.y = 1;
						}
						else if(targetPosition.y < transform.position.y) {
							direction.y = -1;
						}
						else {
							direction.y = 0;
						}
						
					}
					else {
						GetRandomYDirection();
					}
					
					lastDirectionChange = 0f;
					
					//transform.rotation = Rotate2D.SmoothLookAt(transform, Vector3 target, Vector3 axis, float speed);
					
					
				}
				else if(alternateXMovement && (lastXDirectionChange >= changeXDirectionInterval) ) {
					GetRandomXDirection();
					lastXDirectionChange = 0f;
				}
				
				
				//transform.RotateAround(transform.position, Vector3.forward, 20 * Time.deltaTime);
				// 2 - Movement
				movement = new Vector2(
					speed.x * direction.x,
					speed.y * direction.y);
					
					
				//lastXDirection = direction.x;
				
			}
	  }
	
		
		
	}
	
	void FixedUpdate()
	{

		//if(!isVisible && startOnlyIfVisible) {
		//	return;
		//}
	  
		// Apply movement to the rigidbody
	    //check if in camera
		if( (allowMoveIfNotVisible || isVisible || wasMovementTriggeredByOther) && !stopMovement  ) {
			 Rigidbody2D body = GetComponent<Rigidbody2D>();
			 if(gameObject.tag!=null && gameObject.CompareTag("Player")) {
			   //just move on the y axis
				Vector2 vel = new Vector2(body.velocity.x,movement.y);
			 	body.velocity = vel;
			 }
			 else {
				body.velocity = movement;
			 }
		     
			
		}
		
	}
	//gets a random y direction, either 0,-1 or 1
	private void GetRandomYDirection() {
		direction.y = Random.Range(-1,2);

	}
	//gets a random x direction, either 0,-1 or 1
	private void GetRandomXDirection() {
		direction.x = Random.Range(-1,2);
		
	}
	
	//called from outside, to allow or not, move if not vivible
	public void AllowMoveWhenInvisible(bool allowInvisibleMove) {
		allowMoveIfNotVisible = allowInvisibleMove;
	}
	
	
	
	void OnBecameVisible() {
		if(isVisible) {
	 		return; //UNITY BUG
		}

		isVisible = true;

	}

	//TODO; IS NOT TURNING BACK ANYMORE
	void OnBecameInvisible (){

		if(!isVisible) {
			return;
		}
		
		if(isVisible) {
			if (allowMoveIfNotVisible && allowRevertXDirection && limitedMovementXInterval == 0) {
			  revert = true;
			}

		}
		
		isVisible = false;
	}

	bool IsMovementTriggerVisible() {
		if (movementTrigger == null)
			return false;

		SpriteRenderer rend = movementTrigger.GetComponent<SpriteRenderer> ();
		if (rend != null) {
			return rend.isVisible;
		}

		//no renderer, check the position (for 2D, x and y should be between 0 and 1)
		// and z positive??

		Vector3 objectPos = Camera.main.WorldToViewportPoint(movementTrigger.transform.position);
		//Debug.Log ("Position of the movement Trigger: " + objectPos);
		bool trigger = (objectPos.x >= 0f) && (objectPos.x <= 1f) && (objectPos.y >= 0f) && (objectPos.y <= 1f);
		if (trigger) {
			//Debug.Log("Trigger position:" + objectPos);
			wasMovementTriggeredByOther = true;//this triggered the movement, so independent of other settings it can move now
		}
		return trigger;

	}

}
