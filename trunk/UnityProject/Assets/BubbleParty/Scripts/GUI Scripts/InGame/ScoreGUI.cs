using UnityEngine;
using System.Collections;

public class ScoreGUI : MonoBehaviour {
	
	public tk2dAnimatedSprite explore;
	public UILabel text;
	
	
	public enum Type{
		EXPLOSE,
		FALLEN,
		FIREBALL
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void SetScore(int score, Type type){
		text.text = score.ToString();
		switch(type){
		case Type.EXPLOSE:
			if(InGameScriptRefrences.onFireManager.IsOnFire()){
				explore.clipId = explore.anim.GetClipIdByName("score_exp_onfire");
			}
			explore.Play();
			break;
		case Type.FALLEN:
			explore.gameObject.SetActive(false);
			break;
		}
		
		Destroy(gameObject,0.5f);
	}
}
