using UnityEngine;
using System.Collections;

public class SpriteMaterialChangerScript : MonoBehaviour {

    public Material defaultSpritesMaterial;
    public Material graySpriteMaterial;
	public Material currentMaterial;
    public float startDelay = 0f;//start delayed?
    public float stopAfterSeconds = 10f; //stop after 10f
    public float swapInterval = 2f; //change every 2 secs
    private int currentMaterialIndex = 0;
    public int maxSwaps = 0;
    private int counter = 0;
    private SpriteRenderer[] rendererArray;
	public bool affectsOnlyCurrentObject = false;
	// Use this for initialization
	void Start () {

	if(!affectsOnlyCurrentObject)
	 	rendererArray = GameObject.FindObjectsOfType<SpriteRenderer>();

	 currentMaterialIndex = 0;
	 counter = 0;
	 InvokeRepeating("Swap",startDelay,swapInterval);
	   
	}
	
	// Update is called once per frame
	void Update () {

	  if(counter == maxSwaps && maxSwaps > 0) {
	    CancelInvoke("Swap");
	  }
	}

	public void Swap() {

		if(currentMaterial == graySpriteMaterial) {
			currentMaterial = defaultSpritesMaterial;
			
		}
		else {
			currentMaterial = graySpriteMaterial;
			
		}

		if (affectsOnlyCurrentObject) {
			SpriteRenderer rend = GetComponent<SpriteRenderer> ();
			if (rend != null) {
				rend.material = currentMaterial;//reset also the color
				if (maxSwaps==counter+1 && currentMaterial == defaultSpritesMaterial) {

					//check if we have a reference to the original color
					EmeraldLineColor col = GetComponent<EmeraldLineColor>();
					if(col!=null) {
						rend.color = col.originalColor;
					}
					else {
						rend.color = Color.white;
					}
				}

			}

		} 
		else {
			//is a global script instance, affects all objects of the same type
			foreach(SpriteRenderer rend in rendererArray) {

			    //i am destroying some object that uses this rendered CHECK!!!
				GameObject owner = rend.gameObject;
				//this object has itself a material changer, so we do not consider it
				SpriteMaterialChangerScript changer = owner.GetComponent<SpriteMaterialChangerScript>();
				if(changer!=null && changer.affectsOnlyCurrentObject) {
				  continue;
				}

				rend.material = currentMaterial;
				//reset also the color
				if (maxSwaps==counter+1 && currentMaterial == defaultSpritesMaterial) {
					rend.color = Color.white;
				}
				
			}
		}


	

		counter+=1;


	  
	}
}
