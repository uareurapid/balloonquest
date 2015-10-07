using UnityEngine;
using System.Collections;

public class GemScript : MonoBehaviour {

    //the player balloon will get this sprite
    public Sprite giftBaloon;
    public bool isRed = false;
	public bool isGreen = false;
	public bool isBlue = false;

	//give a gift after n pickups
	public int giftAfter = 5;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Sprite GetBalloonGift() {

	   return giftBaloon;
	}
}
