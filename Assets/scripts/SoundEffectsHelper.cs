using UnityEngine;
using System.Collections;

/// <summary>
/// Creating instance of sounds from code with no effort
/// </summary>
public class SoundEffectsHelper : MonoBehaviour
{
	
	/// <summary>
	/// Singleton
	/// </summary>
	private static SoundEffectsHelper instance;

	public AudioClip movementWooshSound;
	public AudioClip pickupCoinSound;
	public AudioClip hitDeadSound;
	public AudioClip waterSplashSound;
	public AudioClip redAlertSound;
	public AudioClip electricitySound;
	public AudioClip replaySound;
	public AudioClip settingsSound;
	public AudioClip countdownSound;
	public AudioClip powerupSound;
	public AudioClip landingSound;
	public AudioClip flipTimeSound;
	public AudioClip successSound;
	public AudioClip earthQuakeSound;
	public AudioClip teleportSound;

		//todo credit Freesound.org - "Energy Weapon 001.wav" by DJ Chronos
		//Freesound.org - "Medium Explosion.wav" by ryansnook
		//Freesound.org - "Distant explosion.wav" by juskiddink
	//https://www.freesound.org/people/fins/sounds/146729/
	
	void Awake()
	{
		// Register the singleton
		//if (Instance != null)
		//{
		//	Debug.LogError("Multiple instances of SoundEffectsHelper!");
		//}
		//Instance = this;
		if(instance!=null) {
			Debug.Log("There is another instance sound effects running");
		}
		else {
			instance = this;
		}
		
		
	}

	public void PlayPickupCoinSound()
	{
		MakeSound(pickupCoinSound);
	}

	public void PlayEarthQuakeSound()
	{
		MakeSound(earthQuakeSound);
	}

	public void PlaySuccessSound()
	{
		MakeSound(successSound);
	}

	public void PlayTeleportSound()
	{
		MakeSound(teleportSound);
	}

	public void PlayCountdownSound()
	{
		MakeSound(countdownSound);
	}

	public void PlayPowerupSound()
	{
		MakeSound(powerupSound);
	}

	public void PlayHitDeadSound()
	{
		MakeSound(hitDeadSound);
	}

	public void PlayWooshSound() {
		MakeSound(movementWooshSound);
	}

	public void PlayLandingSound() {
		MakeSound(landingSound);
	}

	public void PlayWaterSplashSound()
	{
		MakeSound(waterSplashSound);
	}

	public void PlayReplaySound()
	{
		MakeSound(replaySound);
	}

	public void PlaySettingsSound()
	{
		MakeSound(settingsSound);
	}

	public void PlayRedAlertSound()
	{
		//play at 25%
		AudioSource.PlayClipAtPoint(redAlertSound, transform.position,0.25f);
	}
	
	public void PlayElectricitySound()
	{
		MakeSound(electricitySound);
	}

	public void PlayFlipTimeSound()
	{
		MakeSound(flipTimeSound);
	}

	public static SoundEffectsHelper Instance {
		get
		{
			if (instance == null)
			{
				instance = (SoundEffectsHelper)FindObjectOfType(typeof(SoundEffectsHelper));
				if (instance == null)
					instance = (new GameObject("SoundEffectsHelper")).AddComponent<SoundEffectsHelper>();
			}
			return instance;
		}
	}
	
	/// <summary>
	/// Play a given sound
	/// </summary>
	/// <param name="originalClip"></param>
	private void MakeSound(AudioClip originalClip)
	{
		//play at the camera position, otherwise you cannot hear it
		AudioSource.PlayClipAtPoint(originalClip, Camera.main.transform.position,1f);
	}

	//initialization
	public void Start() {
				
	}
	//called at every run loop
	public void Update() {

	}

	//TODO need a sound for the pickups (could be from Fishy Bird Flapy)
}
