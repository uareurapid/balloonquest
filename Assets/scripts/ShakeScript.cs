using UnityEngine;
using System.Collections;

public class ShakeScript : MonoBehaviour {


	//public float shakeSpeed = 1.0f; //how fast it shakes
	public float shakeAmount = 0.055f; //how much it shakes
	public bool shake = false;
	public bool fallAfterShake = false;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	  if(shake) {

		// Sets the position to be somewhere inside a circle
		// with radius 5 and the center at zero.

		Vector3 position = new Vector3(transform.position.x,transform.position.y,transform.position.z);
		Vector3 newPosition = Random.insideUnitCircle * 0.055f;
		transform.position = position + newPosition;

	  }
		
	}
	//starts shaking, but will stop in stopDelay seconds
	public void StartShaking(float stopDelay) {

	  shake = true;
	  Invoke("StopShaking",stopDelay);
	}

	public void StartShaking() {

	  shake = true;
	}

	public void StopShaking() {

      shake = false;
      if(fallAfterShake) {

			FallenTreeScript fall = GetComponent<FallenTreeScript>();
			fall.enabled = true;
			fall.StartFalling();
      }
	}
}
