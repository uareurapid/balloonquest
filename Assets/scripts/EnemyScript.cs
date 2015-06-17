using UnityEngine;
using System.Collections;


/// <summary>
/// Enemy generic behavior
/// </summary>
public class EnemyScript : MonoBehaviour
{

  public bool autoDestroy = false;
	
	private bool isVisible = false;
	public bool takeAllLifes = false;

	private bool isBloody = false;

	public bool isBoss = false;

	private GameObject scripts;

	
	void Awake()
	{
		// Retrieve the weapon only once, which is inside the enemy-->weaponObject->weaponScript
		//weapons = GetComponentsInChildren<WeaponScript>();


	}

	// 1 - Disable everything
	void Start()
	{

	  //isBloody = gameObject.tag!=null && gameObject.tag.Equals("Bloody");
	  //scripts = GameObject.FindGameObjectWithTag("Scripts");

	}

	void Update()
	{
		
			// Auto-fire
			/*foreach (WeaponScript weapon in weapons)
			{
				if (weapon != null && weapon.enabled && weapon.CanAttack(isVisible))
				{
					weapon.Attack(true);

					//sound of shot
					SoundEffectsHelper.Instance.MakeEnemyShotSound();
				}
			}*/
			
			// 4 - Out of the camera ? Destroy the game object.
			//if (renderer.IsVisibleFrom(Camera.main) == false)
			//{
			//	Destroy(gameObject);
			//}
		
	}

	
	
	void OnBecameVisible() {

		isVisible = true;
		//if(GameControllerScript.Instance.IsGameOver()) {
		//  isVisible = false;
		//}
	}
	
	void OnBecameInvisible() {
		
		if(isVisible && autoDestroy) {
		  Destroy(gameObject);
		}
		isVisible = false;
		
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
	  PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
	  if(player!=null) {
	    player.KillPlayer();
	  }
	}

	void OnCollisionEnter2D(Collision2D collision)
	{

	  PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
	  if(player!=null) {
	    player.KillPlayer();
	  }
	}
	//alows spawning again
	void UnlockSpawning() {

	}

	IEnumerator Fade (float start,float end, float length) {

	 GUITexture redTexture = GetComponentInChildren<GUITexture>();
	 Color aux = redTexture.color;
		  //define Fade parmeters
		if (aux.a == start){

		  for (float i = 0.0f; i < 1.0f; i += Time.deltaTime*(1/length)) { 
		   //for the length of time
		   aux.a = Mathf.Lerp(start, end, i); 
		   //lerp the value of the transparency from the start value to the end value in equal increments
		   yield return null;
		   aux.a = end;
		  // ensure the fade is completely finished (because lerp doesn't always end on an exact value)
          redTexture.color = aux;
          } //end for
 
		} //end if
	
 
	} //end Fade


	void FlashRedWhenHit (){


		StartCoroutine(Fade (0f, 0.1f, 0.5f));
		StartCoroutine(MyWaitMethod());
		StartCoroutine(Fade (0.1f, 0f, 0.5f));
	
    	
    }

	IEnumerator MyWaitMethod() {
		yield return new WaitForSeconds(.01f);
	}
		

	void OnDestroy() {



	}

	//is lava enemy?
	private bool IsLava() {
	 return tag!=null && CompareTag("Lava");
	}
	
}
