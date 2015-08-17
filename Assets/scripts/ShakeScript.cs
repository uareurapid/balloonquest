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
		Vector3 position = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		position.y = Mathf.Sin(Time.time * shakeSpeed);
		transform.position = position;
	}
}
