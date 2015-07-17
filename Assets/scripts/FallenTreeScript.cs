using UnityEngine;
using System.Collections;

public class FallenTreeScript : MonoBehaviour {

	//seconds to fall, delay
	public float fallDelay = 2.0f;
	private bool isVisible = false;
	// Use this for initialization
	void Start () {
	
		Invoke ("FallTree", fallDelay);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FallTree() {
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
		isVisible = true;
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
