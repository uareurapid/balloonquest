using UnityEngine;
using System.Collections;
using MrBalloony;

public class NextSceneScript : MonoBehaviour {


    int nextScene = 1;
    bool goToNextScene = false;
	MoveTowardsScript moveTowards;
    public Sprite chestClosed;
    public Sprite chestOpen;
	// Use this for initialization
	void Start () {

	  //read the level to load
	  nextScene = PlayerPrefs.GetInt(GameConstants.NEXT_SCENE_KEY,1);

	  CheckUnlockedLevels();
	  //start moving fake player
	  Invoke("MovingTowards",0.5f);
	}
	
	// Update is called once per frame
	void Update () {
	 if(moveTowards!=null && moveTowards.HasReachedTarget() && !goToNextScene) {
	     //already reached target? then load the scene
		 goToNextScene = true;
		 Application.LoadLevel("Level"+nextScene);
	 }

	}

	void MovingTowards() {
		GameObject fake = GameObject.FindGameObjectWithTag("Player");
		if(fake!=null) {
		   moveTowards = fake.GetComponent<MoveTowardsScript>();
		   GameObject targetObj = GameObject.FindGameObjectWithTag("Level"+nextScene);
		   if(moveTowards!=null && targetObj!=null) {
		     moveTowards.target = targetObj.transform;
			 moveTowards.StartMovingTowards(true);
		   }

		}
	}

	void CheckUnlockedLevels() {

	  for(int i=1; i<=GameConstants.NUM_WORLDS; i++) {

	   if(PlayerPrefs.HasKey(GameConstants.UNLOCKED_LEVEL_KEY+i)) {
			GameObject obj = GameObject.FindGameObjectWithTag("start"+i);
			if(obj!=null) {
			    SpriteRenderer rend = obj.GetComponent<SpriteRenderer>();
			    rend.sprite = chestOpen;
			}
	   }

	  }
	}

}
