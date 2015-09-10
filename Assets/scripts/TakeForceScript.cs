using UnityEngine;
using System.Collections;

public class TakeForceScript : MonoBehaviour {


    public bool enableGravityAfterForce = true;
	public float gravityDelayAfterForce = 1.5f; //1.5 secons after
	Rigidbody2D rig = null;

    //This game object can receive force from another one
    //it should be set as "Trigger"

	// Use this for initialization
	void Start () {
	  rig = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {


	}

	//apply force from AddForceScript gameObject
	public void TakeForce(Vector2 force) {
	 Debug.Log("take force");
		
		if(rig!=null) {
		    MoveScript move = GetComponent<MoveScript>();
			rig.AddForce(force);
		    //disable any movement script first
		    if(move!=null) {
		   	 	move.enabled = false;
		    }

		    //just set the bool to true, so i can call Invoke on Update()
		    if(enableGravityAfterForce) {
				StartCoroutine("EnableGravityScale");
		    }
			
		}
	}

	IEnumerator EnableGravityScale() {
		yield return new WaitForSeconds(gravityDelayAfterForce);
		rig.gravityScale = 1.0f;
	}
}
