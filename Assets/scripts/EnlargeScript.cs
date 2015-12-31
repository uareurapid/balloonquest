using UnityEngine;
using System.Collections;

public class EnlargeScript : MonoBehaviour {

    public float duration = 1.5f; //in seconds
    public float scaleAmount = 0.2f;//how much enlarge each time
    public float repeatFrequency = 0.2f;//repeat effect every x seconds
    public int maxRepetitions = 5;//number of times to repeat
	public float delay = 0f;//delay before start

    private int counter = 0;
	// Use this for initialization
	void Start () {

	  counter = 0;
	  InvokeRepeating("Enlarge",delay,repeatFrequency);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Enlarge() {

	  Vector3 transformation = new Vector3(transform.localScale.x,transform.localScale.y,transform.localScale.z);
	  transformation.x = transformation.x + scaleAmount;
	  transformation.y = transformation.y + scaleAmount;
	  transform.localScale = transformation;

	  counter++;

	  if(counter>=maxRepetitions) {
		CancelInvoke("Enlarge");
	  }
	}
}
