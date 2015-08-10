using UnityEngine;
using System.Collections;

public class AutoDestroyScript : MonoBehaviour {

    public bool destroyIfInvisible = false;
    public bool destroyAfterDelay = false;
    public float destroyDelay = 5.0f;

    private bool isVisible = false;
	// Use this for initialization
	void Start () {

	  if(destroyAfterDelay && destroyDelay >0.0f) {
	    Invoke("DestroyObject",destroyDelay);
	  }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void DestroyObject() {
		Destroy(gameObject);
	}

	void OnBecameInvisible() {
	  if(isVisible && destroyIfInvisible) {
	    Destroy(gameObject);
	  }
	}
	void OnBecameVisible() {

	  isVisible = true;

	}
}
