using UnityEngine;
using System.Collections;

public class RopeSwingScript : MonoBehaviour {


		
		public float angleX = 20; // swing angle = 2 * angle
		public float speedX = 0.5f; // speed (6.28 means about 1 second)
		
		public float angleY = 30; // swing angle = 2 * angle
		public float speedY = 0.5f; // speed (6.28 means about 1 second)
		
		public float angleZ = 10; // swing angle = 2 * angle
		public float speedZ = 0.5f; // speed (6.28 means about 1 second)
		
		// Update is called once per frame
		void LateUpdate () {
			transform.localEulerAngles = new Vector3 (angleX * Mathf.Cos(speedX * Time.time), angleY * Mathf.Sin(speedY * Time.time) , angleZ * Mathf.Cos(speedZ * Time.time));
		}
}
