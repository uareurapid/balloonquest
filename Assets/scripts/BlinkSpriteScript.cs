using UnityEngine;
using System.Collections;

public class BlinkSpriteScript : MonoBehaviour {

    public float delay = 0f;
    public float blinkInterval = 0f;
    public bool canBlink = false;
    public int numberOfBlinks = 5;

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
		if(canBlink) {
		 Blink();
		}
	}

	public void EnableBlink(float del, float interval) {
	  canBlink = true;
	  if(canBlink) {

	    for(int i = 0; i<numberOfBlinks; i++) {
			ShowSprite();
	 		StartCoroutine(MyWaitMethod(interval));
			HideSprite();
	    }
	  }
	}

	public void DisableBlink() {
	 canBlink = false;
	 CancelInvoke("Blink");
	}

	public void Blink() {
	  rend.enabled = !rend.enabled;
	}

	void HideSprite() {

		rend.enabled = false; 


	}

	void ShowSprite() {

		rend.enabled = true; 
	
	}

	IEnumerator MyWaitMethod(float seconds) {
		yield return new WaitForSeconds(seconds);
	}

}
