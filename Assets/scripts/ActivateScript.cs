using UnityEngine;
using System.Collections;

public class ActivateScript : MonoBehaviour {
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Activate() {


		ParticleSystem[] systems = GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem ps in systems) {
			ps.Play(true);
		}
	}
	public void DeActivate() {
		gameObject.SetActive (false);
	}
}
