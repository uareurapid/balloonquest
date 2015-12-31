using UnityEngine;
using System.Collections;

public class GoldenKeyScript : MonoBehaviour {
	SoundEffectsHelper sfx ;
	// Use this for initialization
	void Start () {


	   GameObject scriptsObj = GameObject.FindGameObjectWithTag("Scripts");
		if(scriptsObj!=null) {
			sfx = scriptsObj.GetComponentInChildren<SoundEffectsHelper> ();
			if (sfx != null) {
				sfx.PlayHitDeadSound();
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		GameObject collisionObject = otherCollider.gameObject;
		PlayerScript player = collisionObject.GetComponent<PlayerScript>();
		if(player!=null && sfx!=null) {
		  sfx.PlayPowerupSound();
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		GameObject collisionObject = collision.gameObject;
		PlayerScript player = collisionObject.GetComponent<PlayerScript>();
		if(player!=null && sfx!=null) {
		  sfx.PlayPowerupSound();
		}
	}
}
