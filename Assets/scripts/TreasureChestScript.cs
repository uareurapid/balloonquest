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
		if (player != null && player.IsPlayerAlive() && !opened ) {

			if(player.HasChestKey()) {
				//Open the chest
				Animator anim = GetComponent<Animator>();
				anim.enabled = true;
				ActivateScript part = GetComponent<ActivateScript>();
				if(part!=null) {
					part.Activate();
				}
				opened = true;
				player.OpenChest(opened);

				ShowPointer();

				//play sound
				AudioSource au = GetComponent<AudioSource>();
				if(au!=null) {
					au.Play();
				}

				ChangeColorFlash();
				ChangeRainbowColor();
				Invoke("UnleashColors",2f);
			}
			else {
			 //POSSIBLE TO GO UP AGAIN??
			}



		}

		
	}

	//shows a pointer blinking
	void ShowPointer() {
		GameObject obj = GameObject.FindGameObjectWithTag("pointer");
		if(obj!=null) {
		  BlinkSpriteScript blink = obj.GetComponent<BlinkSpriteScript>();
		  if(blink!=null) {
		    blink.EnableBlink(0,1.2f);
		  }
		}
	}

	//fire the chest/ground big flash, with the same colour as the level
	void ChangeColorFlash() {
		GameObject obj = GameObject.FindGameObjectWithTag("ColorChangerFlash");
		if(obj!=null) {
			ActivateScript part = obj.GetComponentInChildren<ActivateScript>();
			part.Activate();
		}
	}

	void UnleashColors() {
		SpriteMaterialChangerScript change = GetComponent<SpriteMaterialChangerScript> ();
		if(change!=null)
		  change.enabled = true;
	}

	//changes the rainbow color
	void ChangeRainbowColor() {

	  GameObject obj = GameObject.FindGameObjectWithTag("Scripts");
	  if(obj!=null) {
	    GameControllerScript controller = obj.GetComponent<GameControllerScript>();

	      int level = controller.currentLevel;

	      string tagToFind = "Level"+level;
	      GameObject emeraldObj = GameObject.FindGameObjectWithTag(tagToFind);
	      if(emeraldObj!=null) {

	        //Activate the smaller white flash

			ActivateScript activateFlash = emeraldObj.GetComponent<ActivateScript>();
			if(activateFlash!=null) {
			  activateFlash.Activate();
			}
	        //change the colour
			EmeraldLineColor color = emeraldObj.GetComponent<EmeraldLineColor>();
			UnityEngine.UI.Image image = emeraldObj.GetComponent<UnityEngine.UI.Image>();
			if(color!=null && image!=null) {
			  image.color = color.originalColor;
			}
	        
	      
	      }
	  }
	}
}
