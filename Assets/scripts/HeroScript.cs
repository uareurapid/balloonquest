using UnityEngine;
using System.Collections;

public class HeroScript : MonoBehaviour {

	private bool isVisible = true;
	PlayerScript player;
	// Use this for initialization
	void Start () {
	
		GameObject playerObj = GameObject.FindGameObjectWithTag ("Player");
		player = playerObj.GetComponent<PlayerScript> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnBecameVisible() {
		isVisible = true;
	}
	
	void OnBecameInvisible() {
		isVisible = false;
		if (!player.CanPlayerMove()) {
			player.KillPlayer();
		}
	}
}
