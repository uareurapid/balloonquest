using UnityEngine;
using System.Collections;

public class HealthBarScript : MonoBehaviour {

    public SpriteRenderer red;
	public SpriteRenderer green;

	private float maxHealth = 0f;
	private float currentHealth = 0f;

	//BalloonScript balloon;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	 
	  var healthPercent = 2 / (float) 5;
	 green.transform.localScale = new Vector3(healthPercent,1,1);
	    
		
	}



}
