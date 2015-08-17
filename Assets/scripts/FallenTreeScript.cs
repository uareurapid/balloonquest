using UnityEngine;
using System.Collections;

public class FallenTreeScript : MonoBehaviour {

	//seconds to fall, delay
	public float fallDelay = 2.0f;
	private bool isVisible = false;
	public bool applyDelayOnlyVisible = true;//by default only start counting for fall, if visible
	public bool applyFallRotation = true;
	public float fallRotationSpeed = 10.0f;
	public int fallRotationDirection = 1;//either 1 (left) or -1 (right)

	private bool isFalling = false;
	// Use this for initialization
	void Start () {
		isFalling = false;
		if (applyDelayOnlyVisible == false) {//if false apply delay immediatelly
			Invoke ("StartFalling", fallDelay);
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
		if (applyFallRotation && isFalling) {
			transform.Rotate(new Vector3(0,0,fallRotationSpeed * fallRotationDirection * Time.deltaTime));
		}

	}

	//could be invoked from outside, on camera shake for instance
	public void StartFallingAfterDelay() {
	 if(!isFalling && fallDelay>0f) {
		Invoke ("StartFalling", fallDelay);
	 }
	}

	//also public to allow calls from other objects
	public void StartFalling() {
		isFalling = true;
		AudioSource audio = GetComponent<AudioSource> ();
		if (audio != null && !audio.isPlaying ) {
			audio.Play();
		}

		Rigidbody2D body = GetComponent<Rigidbody2D> ();
		if (body != null) {
			body.gravityScale = 1.0f;
		}
	}

	void OnBecameVisible() {
		if (!isVisible) {
			isVisible = true;
			if(applyDelayOnlyVisible) {//start counting
				Invoke ("StartFalling", fallDelay);
			}
		}

	}

	void OnBecameInvisible() {
		if (isVisible) {
			AudioSource audio = GetComponent<AudioSource> ();
			if (audio != null && audio.isPlaying ) {
				audio.Stop();
			}
			isVisible = false;
			Destroy(gameObject);
		}
	}
}
