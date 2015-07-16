using UnityEngine;
using System.Collections;

//show or hide an object/sprite
public class HiddenSpriteScript : MonoBehaviour {

	public float delayBeforeShowHide = 3.0f;
	public bool startHidden = true;
	// Use this for initialization
	void Start () {
	
		if (startHidden) {
			Invoke ("Show", delayBeforeShowHide);
		} 
		else {
			Invoke ("Hidde", delayBeforeShowHide);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Show() {
		SpriteRenderer sprite = GetComponent<SpriteRenderer> ();
		if (sprite != null) {
			sprite.enabled = true;
		}
	}

	void Hidde() {
		SpriteRenderer sprite = GetComponent<SpriteRenderer> ();
		if (sprite != null) {
			sprite.enabled = false;
		}
	}
}
