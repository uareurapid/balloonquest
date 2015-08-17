using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	public Transform camTransform;
	
	// How long the object should shake for.
	public float shakeDuration = 0f;
	
	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;

	public bool shakeEnabled = false;
	public float shakeInterval = 0f;//interval between sakes
	private float lastShakeTime = 0f;//last saved shake time
	private float startShakeTime = 0f;//when it started shaking
	public float shakeDelay = 0f;//if greater than 0, start delayed

	public bool autoStartShakeOnVisible = false;
	
	Vector3 originalPos;

	void Start() {
	  if(shakeDelay > 0) {
	    Invoke("EnableShake",shakeDelay);
	  }
	}
	
	void Awake()
	{
		if (camTransform == null)
		{
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}
	
	void OnEnable()
	{
		originalPos = camTransform.localPosition;
	}

	void Update()
	{

	  float currentTime = Time.deltaTime;

	  if(shakeEnabled){
		if (shakeDuration > 0)
		{
		//only get time if is zero 
			if(startShakeTime==0f) {
				startShakeTime = Time.deltaTime;
			}
		    
			camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
			shakeDuration -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			shakeDuration = 0f;
			camTransform.localPosition = originalPos;
			shakeEnabled = false;
			//re-schedule next shake, as soon as this one finishes
			lastShakeTime = Time.deltaTime;
			startShakeTime = 0f;
		}
	  }
	  else {
	   //not enabled
			Debug.Log("CurrenTime: " + currentTime);
				Debug.Log("lastShakeTime: " + lastShakeTime);
				Debug.Log("shakeInterval: " + shakeInterval);
		if(shakeInterval > 0f && (currentTime - lastShakeTime > shakeInterval / 1000)  ) {
				Debug.Log("AGAIN!!!!!: " );
	      EnableShake();
	   }
	  }

		
	}

	void OnBecameVisible() {
	 if(autoStartShakeOnVisible && !shakeEnabled) {
	    EnableShake();
	 }
	}

	public void EnableShake() {
	  shakeEnabled = true;
	  OnEnable();

	  if(gameObject.tag!=null && gameObject.CompareTag("MainCamera")) {
	  //now shake also the other objects with same script, at the same time we shake the camera
		 CameraShake[] otherObjects = GameObject.FindObjectsOfType<CameraShake>();
		 foreach(CameraShake obj in otherObjects) {
		   string tag = obj.gameObject.tag;
		   if(tag!=null && tag.Equals("MainCamera")) {
		     continue;//ignore the camera, since we are dealing already with it
		   }
		   else {
		    obj.EnableShake();//enable shake on th eother object as well
		    if(obj.gameObject.GetComponent<FallenTreeScript>()!=null) {
				obj.gameObject.GetComponent<FallenTreeScript>().enabled = true;
		    }
		   }
		 }
	  }
	}

	public void DisableShake() {
	  shakeEnabled = false;
	}
}
