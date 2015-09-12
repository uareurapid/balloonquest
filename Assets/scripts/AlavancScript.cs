using UnityEngine;
using System.Collections;

public class AlavancScript : MonoBehaviour {

    public float enableGravityDelay = 2f;
	// Use this for initialization
	void Start () {

	  Invoke("EnableGravityScale",enableGravityDelay);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void EnableGravityScale(){
	  Rigidbody2D rig =GetComponent<Rigidbody2D>();
	  if(rig!=null){
	    rig.gravityScale=1.0f;
	    //also disable collider so the ball can pass through it
	    StartCoroutine(DisableCollider());
	  }
	}

	IEnumerator DisableCollider() {
	  yield return new WaitForSeconds(0.5f);
	  BoxCollider2D collider = GetComponent<BoxCollider2D>();
	  if(collider!=null) {
	     collider.enabled = false;
	  }
	}
}
