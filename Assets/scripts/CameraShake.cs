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
	private bool isMobilePlatform = false;
	private static RuntimePlatform platform;
	Vector3 originalPos;

	void Start() {

	  initialShakeDuration = shakeDuration;
	  isMobilePlatform = (platform == RuntimePlatform.IPhonePlayer) || (platform == RuntimePlatform.Android);
	  
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
			Handheld.Vibrate();
		}
		else
		{
			shakeDuration = 0f;
			camTransform.localPosition = originalPos;
			shakeEnabled = false;

			//Stop shaking
			FallenTreeScript fall =	gameObject.GetComponent<FallenTreeScript>();
		    if(fall!=null) {
		        fall.prepareFallingSequence();

		    }
	
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
	  if(isMobilePlatform) {
	    Handheld.Vibrate();
	  }

	  if(gameObject.tag!=null && gameObject.CompareTag("MainCamera")) {
	  //now shake also the other objects with same script, at the same time we shake the camera
		 CameraShake[] otherShakyObjects = GameObject.FindObjectsOfType<CameraShake>();
		 foreach(CameraShake obj in otherShakyObjects) {
		   string tag = obj.gameObject.tag;
		   if(tag!=null && tag.Equals("MainCamera")) {
		     Debug.Log("This is the camera itself, ignoring it...");
		     continue;//ignore the camera, since we are dealing already with it
		   }
		   else {
		    
		    obj.EnableShake();//enable shake on the other object as well
			
		   }
		 }
	  }
	}

	public void DisableShake() {
	  shakeEnabled = false;
	}
}
