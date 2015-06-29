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
		if (GUI.Button(new Rect(10,160,50,30),"Level4")) {
			Application.LoadLevel("Level4");
		}
		if (GUI.Button(new Rect(10,190,50,30),"Level5")) {
			Application.LoadLevel("Level5");
		}
		if (GUI.Button(new Rect(10,220,50,30),"Level6")) {
			Application.LoadLevel("Level6");
		}
		if (GUI.Button(new Rect(10,250,50,30),"Level7")) {
			Application.LoadLevel("Level7");
		}
		if (GUI.Button(new Rect(10,280,50,30),"Level8")) {
			Application.LoadLevel("Level8");
		}
	}
}
