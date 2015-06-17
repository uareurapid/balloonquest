using UnityEngine;
using System.Collections;

public class LadderScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.CompareTag("Player")) {
		  Debug.Log("Player enter ladder!!!");
		  PlayerScript player = other.gameObject.GetComponent<PlayerScript>();
		  player.SetOnLadder(true);
		}
	}
	
	void OnTriggerExit2D(Collider2D other) {
		if(other.gameObject.CompareTag("Player")) {
			Debug.Log("Player leave ladder!!!");
			PlayerScript player = other.gameObject.GetComponent<PlayerScript>();
			player.SetOnLadder(false);
			
		}
	}
}
