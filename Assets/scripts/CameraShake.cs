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
	public float shakeDelay = 0f;//if greater than 0, start delayed

	public bool autoStartShakeOnVisible = false;
	private float initialShakeDuration = 0f;
	
	Vector3 originalPos;

	void Start() {

	  initialShakeDuration = shakeDuration;
	  
	  if (shakeDelay > 0f) {
		 if (shakeInterval <= 0f) {
			//do not repeat
			Invoke ("EnableShake", shakeDelay);
		 } 
		 else {
			InvokeRepeating ("EnableShake", shakeDelay, shakeInterval);
		 }
	 } 
	 else if (shakeInterval > 0f) {
		InvokeRepeating ("EnableShake", 0f, shakeInterval);
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

	  //float currentTime = Time.deltaTime;

	  if(shakeEnabled){
		if (shakeDuration > 0)
		{
			camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
			shakeDuration -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			shakeDuration = 0f;
			camTransform.localPosition = originalPos;
			shakeEnabled = false;
	
		}
	  }
	  

		
	}

	void OnBecameVisible() {
	 if(autoStartShakeOnVisible && !shakeEnabled) {
	    EnableShake();
	 }
	}

	private void PlayEarthQuakeSound() {
		GameObject music = GameObject.FindGameObjectWithTag("GameMusic");
		AudioSource source = music.GetComponent<AudioSource> ();
		source.volume = 0.3f;
		Invoke("RestoreVolume",3.0f);
		//private SoundEffectsHelper GetSoundEffects() {
		GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
		SoundEffectsHelper fx = scripts.GetComponentInChildren<SoundEffectsHelper> ();
	    fx.PlayEarthQuakeSound();
		//}
	}

	void RestoreVolume() {

		GameObject music = GameObject.FindGameObjectWithTag("GameMusic");
		AudioSource source = music.GetComponent<AudioSource> ();
		source.volume = 0.7f;
	}

	public void EnableShake() {
	  shakeEnabled = true;
	  lastShakeTime = 0f;
	  shakeDuration = initialShakeDuration;
	  OnEnable();
	  PlayEarthQuakeSound ();

	  if(gameObject.tag!=null && gameObject.CompareTag("MainCamera")) {
	  //now shake also the other objects with same script, at the same time we shake the camera
		 CameraShake[] otherObjects = GameObject.FindObjectsOfType<CameraShake>();
		 Debug.Log("ENCONTREI NUM: " + otherObjects.Length);
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
