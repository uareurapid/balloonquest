using UnityEngine;
using System.Collections;

public class MovingPlatformScript : MonoBehaviour {

private float xPosition;
private float yPosition;
private bool max;//reach max movement?

public bool vertical = false;//vertical movement?
public int maxAmount = 5;
public float step = 0.05f;//amount we move per frame

public bool stopOnMax = false;
private bool stopMovement = false;
//by default starts upwards
public bool startGoingDown = false;
public bool resetPositionOnMax = false;
public float startDelay = 0;
public bool lockMovement = false;

private float originalZPosition = 0f;

	// Use this for initialization
	void Start () {
	  xPosition = transform.position.x;
	  yPosition = transform.position.y;
	  originalZPosition = transform.position.z;

	  if(startDelay>0f && lockMovement) {
		Invoke("UnlockMovement",startDelay);
	  }
	}
	
	// Update is called once per frame
	void Update () {

	if(!lockMovement) {

						//SET THE MAX
		if(vertical) {
		
		   if(!startGoingDown) {
		   
				if(transform.position.y >=yPosition + maxAmount) {
					max = true;
				}
				else if(transform.position.y <= yPosition) {
					max= false;
				}
		   }
		   else {
		   //start going down
		   //y aumenta para cima, diminui para baixo
				if(transform.position.y <=yPosition - maxAmount) {
					max = true;
				}
				else if(transform.position.y >= yPosition) {
					max= false;
				}
		   }
		
			
		}
		else {
			if(transform.position.x >=xPosition + maxAmount) {
				max = true;
			}
			else if(transform.position.x <= xPosition) {
				max= false;
			}
		}
	//if we reach the max, and we want to stop, stop movement!
	 if(max && stopOnMax) {
	    stopMovement = true;
	 }

	 Vector2 currentPosition;

	 //go back to beginning
	 if(max && resetPositionOnMax) {
		currentPosition = new Vector3(xPosition,yPosition,originalZPosition);
	 }
	 else {

						//MOVING THE PLAFTORM
		  currentPosition = new Vector3(transform.position.x,transform.position.y,originalZPosition);

		  if(vertical) {
		   if(!max) {//continue until reach max
		     currentPosition.y +=step;
		   }
		   else if(!stopOnMax) {
		     currentPosition.y -=step;//go down
		   }
		  }
		  else {//horizontal
				if(!max) {//continue until reach max
					currentPosition.x +=step;
				}
				else if(!stopOnMax){
					currentPosition.x -=step;//go down
				}
		   }
	 }

	  
	 if(!stopMovement) {
	    
		transform.position = currentPosition;
	 }

	}
		
	 
	 
	 
	}

	public void UnlockMovement() {
	  lockMovement = false;
	}
	
}