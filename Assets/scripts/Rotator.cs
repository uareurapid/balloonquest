using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {
	
	//with a negative value rotates counter clock
	public float rotateSpeed = 5f;
	public string angle="y";
	//public bool useTransformRotate = true;


	void Start() {
	 if(!angle.Equals("z") && !angle.Equals("y") && !angle.Equals("x")) {
	  //wrong angle/axis
	  angle = "z";
	 }
	}
	
	void Update () {

		Vector3 rotationAngle;

	    if(angle.Equals("y")){

		 rotationAngle = new Vector3(0, rotateSpeed*Time.deltaTime,0);
	     //if(useTransformRotate) {
				 transform.Rotate (rotationAngle);
	     //}
		 
			  
	    }
		else if(angle.Equals("x")){

		rotationAngle = new Vector3(rotateSpeed*Time.deltaTime,0,0);

		//	  if(useTransformRotate) {
				  transform.Rotate (rotationAngle);
		//	  }

	    }
		else if(angle.Equals("z")){

		rotationAngle = new Vector3(0, 0,rotateSpeed*Time.deltaTime);

		transform.Rotate(rotationAngle);//	 if(useTransformRotate) {
			//transform.rotation = Quaternion.Euler(new Vector3(0,0,30 * Time.deltaTime));
	
	    }
	}

    

}
