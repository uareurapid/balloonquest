using UnityEngine;
using System.Collections;

public class FallenTreeScript : MonoBehaviour {

	//seconds to fall, delay
	public float fallDelay = 2.0f;
	private bool isVisible = false;
	public bool fallOnlyIfVisible = true;
	public bool applyDelayOnlyVisible = true;//by default only start counting for fall, if visible
	public bool applyFallRotation = true;
	public float fallRotationSpeed = 10.0f;
	public int fallRotationDirection = 1;//either 1 (left) or -1 (right)

	//if this is greater than 0 and apply delay only if visible is set to true..
	//we count (decrease) vivibility counter and only apply the delay when reach 0
	public int visibilityCounter = 0;

	public bool autoDestroy = true;
	private bool isFalling = false;
	// Use this for initialization
	void Start () {
		isFalling = false;
		if (applyDelayOnlyVisible == false && !fallOnlyIfVisible) {//if false apply delay immediatelly
			if(fallDelay>0f){
				Invoke ("StartFalling", fallDelay);
			}
			else {
			    StartFalling();
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//called when the camera stops shaking!
	public void prepareFallingSequence() {
		Debug.Log("prepareFallingSequence");
	  if(fallOnlyIfVisible) {
	    if(isVisible) {
			if(fallDelay>0f){
				Invoke ("StartFalling", fallDelay);
			}
			else {
			    StartFalling();
			}
	    }
	  }
	  else {
			if(fallDelay>0f){
			//fall after initial delay
				Invoke ("StartFalling", fallDelay);
			}
			else {
			//start falling immediately
			    StartFalling();
			}
	  }
	}

	void FixedUpdate() {
		if (applyFallRotation && isFalling) {
			transform.Rotate(new Vector3(0,0,fallRotationSpeed * fallRotationDirection * Time.deltaTime));
		}

	}

	//could be invoked from outside, on camera shake for instance
	public void StartFallingAfterDelay() {
	
	 if (!isFalling && fallDelay > 0f) {
		Invoke ("StartFalling", fallDelay);
	 } 

	}

	//also public to allow calls from other objects
	public void StartFalling() {

		if(!isFalling) {
			isFalling = true;
			AudioSource audio = GetComponent<AudioSource> ();
			if (audio != null && !audio.isPlaying ) {
				audio.Play();
			}

			Rigidbody2D body = GetComponent<Rigidbody2D> ();
			if (body != null) {
				body.gravityScale = 1.0f;
				body.isKinematic = false;
			}

			//check if any collider needs to be enabled, only when fall starts
			PolygonCollider2D collider = GetComponent<PolygonCollider2D>();
			if(collider!=null && collider.enabled==false) {
			  collider.enabled = true;
			}
		}

	}

	void OnBecameVisible() {
		if (!isVisible) {
			isVisible = true;
			//isFalling = false;
			if(applyDelayOnlyVisible && fallOnlyIfVisible && !isFalling) {//start counting

			    if(visibilityCounter==0) {
					StartFallingAfterDelay();
			    }
			    else {
			      	visibilityCounter-=1;
					if(visibilityCounter==0) {
						StartFallingAfterDelay();
			    	}
			    }
				
			}
		}

	}

	public bool IsFalling() {
		return isFalling;
	}

	public bool IsVisible() {
	 return isVisible;
	}

	void OnBecameInvisible() {
		if (isVisible) {
			AudioSource audio = GetComponent<AudioSource> ();
			if (audio != null && audio.isPlaying ) {
				audio.Stop();
			}
			isVisible = false;
			if(autoDestroy) {
				Destroy(gameObject);
			}
		}
	}
}
