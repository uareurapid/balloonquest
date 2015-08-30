using UnityEngine;
using System.Collections;

public class BeeScript : MonoBehaviour {

    public bool isAttacking = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	  
	}

	public void Attack(bool attack) {

		isAttacking = attack;
		/*if(isAttacking) {
		  GetComponent<Animator>();
		}
		else {
		  GetComponent<Animation>().Stop();
		}*/
	}


}
