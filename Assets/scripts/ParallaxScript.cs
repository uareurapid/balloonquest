using UnityEngine;
using System.Collections;

public class ParallaxScript : MonoBehaviour {
   private float xPosition;
	private float yPosition;
   public int offset;
   public bool followCamera;
   private Vector3 newPosition;
	// Use this for initialization
	void Start () {
	  xPosition = Camera.main.transform.position.x;
	  yPosition = Camera.main.transform.position.y;
	  newPosition = new Vector3(transform.position.x,transform.position.y,transform.position.z);

	  //just in case we do something stupid on the editor!
	  if(offset==0) {
	    offset = 1;
	  }
	}
	
	// Update is called once per frame
	void Update () {

	

	  if(followCamera) {
		//newPosition.x = (Camera.main.transform.position.x - xPosition)/offset;
			newPosition.y = (Camera.main.transform.position.y + yPosition)/offset;
	  }
	  else {
		//newPosition.x = (xPosition - Camera.main.transform.position.x)/offset;
			newPosition.y = (yPosition + Camera.main.transform.position.y)/offset;
	  }
	  transform.position = newPosition;
	}
}
