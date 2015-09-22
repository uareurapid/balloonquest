using UnityEngine;
using System.Collections;

public class AudioPlayerScript : MonoBehaviour {

  
  public float playInterval = 20f; 
  public float startDelay = 5f;
	// Use this for initialization
	void Start () {

	  if(GetComponent<AudioSource>()!=null) {

		if(playInterval>0f) {
			InvokeRepeating("PlayClip",startDelay,playInterval);
		}
		else {  //play just once
				Invoke("PlayClip",startDelay);
			}
	    
	  }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void PlayClip() {
	  GetComponent<AudioSource>().Play();
	}
}
