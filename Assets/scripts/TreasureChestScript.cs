using UnityEngine;
using System.Collections;

public class TreasureChestScript : MonoBehaviour {

	private bool opened = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	

	}

	void OnTriggerEnter2D(Collider2D otherCollider)
	{

		GameObject collisionObject = otherCollider.gameObject;
		HeroScript player = collisionObject.GetComponent<HeroScript> ();
		if (player != null) {
			//Open the chest
			Animator anim = GetComponent<Animator>();
			anim.enabled = true;
			ActivateScript part = GetComponent<ActivateScript>();
			if(part!=null) {
				part.Activate();
			}
			opened = true;
			player.OpenChest(opened);

			//play sound
			AudioSource au = GetComponent<AudioSource>();
			if(au!=null) {
				au.Play();
			}

			Invoke("UnleashColors",2f);
		}
		
	}

	void UnleashColors() {
		SpriteMaterialChangerScript change = GetComponent<SpriteMaterialChangerScript> ();
		if(change!=null)
		  change.enabled = true;
	}
}
