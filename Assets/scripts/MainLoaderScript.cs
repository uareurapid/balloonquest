using UnityEngine;
using System.Collections;

public class MainLoaderScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		if (GUI.Button(new Rect(10,10,50,50),"Level1")) {
			Application.LoadLevel("Level1");
		}
		if (GUI.Button(new Rect(10,70,50,30),"Level2")) {
			Application.LoadLevel("Level2");
		}
		if (GUI.Button(new Rect(10,130,50,30),"Level3")) {
			Application.LoadLevel("Level3");
		}
	}
}
