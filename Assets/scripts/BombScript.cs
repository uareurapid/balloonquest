using UnityEngine;
using System.Collections;

public class BombScript : MonoBehaviour
{
	public float bombRadius = 10f;			// Radius within which enemies are killed.
	public float bombForce = 100f;			// Force that enemies are thrown from the blast.
	public AudioClip boom;					// Audioclip of explosion.
	public AudioClip fuse;					// Audioclip of fuse.
	public float fuseTime = 1.5f;
	//public GameObject explosion;			// Prefab of explosion effect.
	public bool isSpikesBomb = false;
	//release spikes??

	public float detonationDelay = 5f;//5 seconds

	void Awake ()
	{
		// Setting up references.
	}

	void Start ()
	{
		
		Invoke("StartDetonation",detonationDelay);
	}

	void StartDetonation() {
		StartCoroutine(BombDetonation());
	}

	IEnumerator BombDetonation()
	{
		// Play the fuse audioclip.
		AudioSource.PlayClipAtPoint(fuse, transform.position);

		// Wait for 2 seconds.
		yield return new WaitForSeconds(fuseTime);

		// Explode the bomb.
		Explode();
	}


	public void Explode()
	{
		//play explosion sound
		AudioSource.PlayClipAtPoint(boom, transform.position);

		// Find all the colliders on the PlayerLayer layer within the bombRadius.
		Collider2D[] elements = Physics2D.OverlapCircleAll(transform.position, bombRadius, 1 << LayerMask.NameToLayer("PlayerLayer"));

		// For each collider...
		foreach(Collider2D en in elements)
		{
			// Check if it has a rigidbody (since there is only one per enemy, on the parent).
			Rigidbody2D rb = en.GetComponent<Rigidbody2D>();
			
			if(rb != null )
			{
			
				//TODO apply force only left /right
				//put them in the same y
				Vector3 bombPosition = transform.position;
				bombPosition.y = rb.transform.position.y;

				// Find a vector from the bomb to the player.
				Vector3 deltaPos = rb.transform.position - bombPosition;

				// Apply a force in this direction with a magnitude of bombForce.
				Vector3 force = deltaPos.normalized * bombForce;
				rb.AddForce(force);

			}
		}

		// Set the explosion effect's position to the bomb's position and play the particle system.

		GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
		if(scripts!=null) {
		    SpecialEffectsHelper effects = scripts.GetComponentInChildren<SpecialEffectsHelper>();
		    if(effects!=null) {
				effects.PlayExplosionEffect(transform.position);
		    }
			
		}

		if(isSpikesBomb) {
		    Rigidbody []bodies = GetComponentsInChildren<Rigidbody>();
		    foreach(Rigidbody body in bodies) {
		      if(body!=null) {
				GameObject obj = body.gameObject;
				if(obj!=null) {
				  Debug.Log("add force to " + bodies.Length);
				  Vector3 rotation = new Vector3(obj.transform.eulerAngles.x,obj.transform.eulerAngles.y,obj.transform.eulerAngles.z);
				  body.AddForce(rotation * 20.5f);
				}
		      }
		    }

			Rigidbody2D []bodies2 = GetComponentsInChildren<Rigidbody2D>();
			foreach(Rigidbody2D body in bodies2) {
		      if(body!=null) {

				GameObject obj = body.gameObject;
				if(obj!=null) {
				  Debug.Log("add force to " + bodies2.Length);
				  body.isKinematic = false;

				  Vector3 dir = Quaternion.AngleAxis(obj.transform.eulerAngles.z, Vector3.forward) * Vector3.right;

				  //Vector3 rotation = new Vector3(obj.transform.eulerAngles.x,obj.transform.eulerAngles.y,obj.transform.eulerAngles.z);
				  body.AddForce(dir * 100f);
				}
		      }
		    }

			//rigidbody.AddForce(transform.forward * amount);
		}


		// Destroy the bomb itself.
		//Destroy (gameObject);
	}
}
