using UnityEngine;
using System.Collections;
using BalloonQuest;

public class LevelLockerScript : MonoBehaviour {

	public bool levelLocked = true;
	public int level = 1;
	// Use this for initialization
	void Start () {
		//level 1 is always unlocked, obviously
		if (level > 1) {
			SpriteRenderer renderer = GetComponent<SpriteRenderer> ();
			bool hasKey = PlayerPrefs.HasKey(GameConstants.UNLOCKED_LEVEL_KEY + level);
			renderer.enabled = hasKey;
			levelLocked = !hasKey;
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
