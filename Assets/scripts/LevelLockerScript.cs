using UnityEngine;
using System.Collections;
using BalloonQuest;

public class LevelLockerScript : MonoBehaviour {

	public bool levelLocked = true;
	public int level = 1;
	// Use this for initialization
	void Start () {

		SpriteRenderer renderer = GetComponent<SpriteRenderer> ();
		//level 1 is always unlocked, obviously
		if (level > 1) {
			bool hasKey = PlayerPrefs.HasKey (GameConstants.UNLOCKED_LEVEL_KEY + level);
			//show the lock if i dont have the key
			renderer.enabled = !hasKey;
			levelLocked = !hasKey;
		} 
		else {
			levelLocked = false;
			renderer.enabled = false;
		}

	}
	
	// Update is called once per frame
	void Update () {
	  
	}
}
