using UnityEngine;
using System.Collections;

public class ShakeScript : MonoBehaviour {


	public float shakeSpeed = 1.0f; //how fast it shakes
	public float shakeAmount = 1.0f; //how much it shakes


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		/*Vector3 position = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		position.x = Mathf.Sin(Time.time * shakeSpeed);
		transform.position = position;*/

		float AngleAmount = (Mathf.Cos(Time.time * shakeSpeed ) * 180) / Mathf.PI * 0.5f;
		Debug.Log("Rotation " + AngleAmount);
		AngleAmount = Mathf.Clamp(AngleAmount, -5 , 5 );
		transform.localRotation = Quaternion.Euler( 0,0, AngleAmount);
	}
}
