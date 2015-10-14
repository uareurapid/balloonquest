using UnityEngine;
using System.Collections;

public class BlinkSpriteScript : MonoBehaviour {

    public float delay = 0f;
    public float blinkInterval = 0f;
    public bool canBlink = false;
    public int numberOfBlinks = 5;

    private bool hasStarted = false;
    private SpriteRenderer rend;
	// Use this for initialization
	void Start () {

	   rend = GetComponent<SpriteRenderer>();
	   if(canBlink) {
			InvokeRepeating("Blink",delay,blinkInterval);
	   }

	   
	}
	
	// Update is called once per frame
	void Update () {
		if(canBlink && !hasStarted) {
			InvokeRepeating("Blink",delay,blinkInterval);
		}
	}

	public void EnableBlink(float del, float interval) {
	  canBlink = true;
	  delay = del;
	  blinkInterval = interval; 
	  hasStarted = false;
	}

	public void DisableBlink() {
	 canBlink = false;
	 CancelInvoke("Blink");
	}

	public void Blink() {
	  rend.enabled = !rend.enabled;

	  if(!hasStarted) {
	   hasStarted = true;
	  }

	  numberOfBlinks-=1;
	  if(numberOfBlinks < 0) {
	    numberOfBlinks = 0;
	    canBlink = false;
	  }
	}


}
