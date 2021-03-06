﻿using UnityEngine;

/// <summary>
/// Creating instance of particles from code with no effort
/// </summary>
public class SpecialEffectsHelper : MonoBehaviour
{
	/// <summary>
	/// Singleton
	/// </summary>
	private static SpecialEffectsHelper instance;
	public bool alwaysOnFrontOfCamera = true; 

	public ParticleSystem hitEffect;
	public ParticleSystem landEffect;
	public ParticleSystem soulEffect;
	public ParticleSystem waterSplashEffect;
	public ParticleSystem explosionEffect;
	public ParticleSystem bloodSplaterEffect;	
	public ParticleSystem lavaSplashEffect;
	public ParticleSystem dustEffect;

	public ParticleSystem auraEffect;

	public ParticleSystem electricityEffect;

	public ParticleSystem coinBurstEffect;

	public ParticleSystem touchDownEffect;
	//2
	public ParticleSystem pointsEffect2Points;
	//5
	public ParticleSystem pointsEffect5Points;
	//10
	public ParticleSystem pointsEffect10Points;


	public ParticleSystem powerupRed;
	public ParticleSystem powerupGreen;
	public ParticleSystem powerupBlue;

	
	void Awake()
	{
		// Register the singleton
		if(instance!=null) {
			Debug.Log("There is another instance special effects running");
		}
		else {
			instance = this;
		}
		
	}

	public void PlayAuraEffect(Vector3 position) {
		instantiate(auraEffect, position);
	}

	public void PlayCoinBurstEffect(Vector3 position) {
		instantiate(coinBurstEffect, position);
	}

	public void PlayHitDeadEffect(Vector3 position) {
		instantiate(hitEffect, position);
	}

	public void PlayBloodSplaterEffect(Vector3 position) {
		instantiate(bloodSplaterEffect, position);
	}

	
	public void PlayLandingEffect(Vector3 position) {
		instantiate(landEffect, position);
	}
	
	public void PlaySoulEffect(Vector3 position) {
		instantiate(soulEffect, position);
	}

	public void PlayPowerupRed(Vector3 position) {
		instantiate(powerupRed, position);
	}
	public void PlayPowerupGreen(Vector3 position) {
		instantiate(powerupGreen, position);
	}
	public void PlayPowerupBlue(Vector3 position) {
		instantiate(powerupBlue, position);
	}

	public void PlayTouchdownEffect(Vector3 position) {
		instantiate(touchDownEffect, position);
	}

	public void PlayJElectricityEffect(Vector3 position) {
		instantiate(electricityEffect, position);
	}

	public void Play5PointsEffect(Vector3 position) {
		instantiate(pointsEffect5Points, position);
	}
	public void Play10PointsEffect(Vector3 position) {
		instantiate(pointsEffect10Points, position);
	}
	public void Play2PointsEffect(Vector3 position) {
		instantiate(pointsEffect2Points, position);
	}

	public void PlayWaterSplashEffect(Vector3 position) {
		instantiate(waterSplashEffect, position);
	}

	public void PlayLavaSplashEffect(Vector3 position) {
		instantiate(lavaSplashEffect, position);
	}

	public void PlayDustEffect(Vector3 position) {
		instantiate(dustEffect, position);
	}

	public void PlayExplosionEffect(Vector3 position) {
		instantiate(explosionEffect, position);
	}

	
	
	public static SpecialEffectsHelper Instance {
		get
		{
			if (instance == null)
			{
				instance = (SpecialEffectsHelper)FindObjectOfType(typeof(SpecialEffectsHelper));
				if (instance == null)
					instance = (new GameObject("SpecialEffectsHelper")).AddComponent<SpecialEffectsHelper>();
			}
			return instance;
		}
	}

	//public void ThunderboltEffect(Vector3 position) {
	//	instantiate(thunderbolt, position);
	//}
	
	/// <summary>
	/// Instantiate a Particle system from prefab
	/// </summary>
	/// <param name="prefab"></param>
	/// <returns></returns>
	private ParticleSystem instantiate(ParticleSystem prefab, Vector3 position)
	{
	    if(alwaysOnFrontOfCamera)
	    	position.z = -5f; //make sure this is in front of all stull

		ParticleSystem newParticleSystem = Instantiate(
			prefab,
			position,
			Quaternion.identity
			) as ParticleSystem;
		
		// Make sure it will be destroyed
		Destroy(
			newParticleSystem.gameObject,
			newParticleSystem.startLifetime
			);
		
		return newParticleSystem;
	}
	
	private Transform instantiateTransform(Transform prefab, Vector3 position)
	{
		Transform newTransform = Instantiate(
			prefab,
			position,
			Quaternion.identity
			) as Transform;
		
		// Make sure it will be destroyed
		Destroy(
			newTransform.gameObject,
			3f
			);
		
		return newTransform;
	}
	
	//Note: Because we can have multiple particles in the scene at the same time, 
	//we are forced to create a new prefab each time. 
	//If we were sure that only one system was used at a time, 
	//we would have kept the reference and use the same everytime.

}
