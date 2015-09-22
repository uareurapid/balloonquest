using UnityEngine;
using System.Collections;

public class SpriteMaterialChangerScript : MonoBehaviour {

    public Material defaultSpritesMaterial;
    public Material graySpriteMaterial;
    public float startDelay = 0f;//start delayed?
    public float stopAfterSeconds = 10f; //stop after 10f
    public float swapInterval = 2f; //change every 2 secs
    private int currentMaterialIndex = 0;
    public int maxSwaps = 0;
    private int counter = 0;
    private SpriteRenderer[] rendererArray;
	// Use this for initialization
	void Start () {
	 rendererArray = GameObject.FindObjectsOfType<SpriteRenderer>();
	 currentMaterialIndex = 0;
	 counter = 0;
	 InvokeRepeating("Swap",startDelay,swapInterval);
	   
	}
	
	// Update is called once per frame
	void Update () {

	  if(counter == maxSwaps && maxSwaps > 0) {
	    Debug.Log("CANCEL INVOKE");
	    CancelInvoke("Swap");
	  }
	}

	void Swap() {

		foreach(SpriteRenderer rend in rendererArray) {
			if(currentMaterialIndex == 0) {
	    		rend.material = defaultSpritesMaterial;

	  		}
	  		else {
	    		rend.material = graySpriteMaterial;

	  		}
		}

		if(currentMaterialIndex == 0) {
			currentMaterialIndex = 1;
		}
		else {
		   currentMaterialIndex = 0;
		}

		counter+=1;

	  
	}
}
