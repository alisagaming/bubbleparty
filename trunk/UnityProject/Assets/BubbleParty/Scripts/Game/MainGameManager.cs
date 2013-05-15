using UnityEngine;
using System.Collections;

public class MainGameManager : MonoBehaviour {
	public GameObject panelInGameGo;
	public OnFireManager onFireManager;
	public GameObject input;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void PauseOn(){
		Time.timeScale = 0;
		input.SetActive(false);
	}
	
	public void PauseOff(){
		Time.timeScale = 1;
		input.SetActive(true);
	}
	
	public void Restart(){
		GameVariables.score = 0;
		GameVariables.levelBonus = 0;
		GameVariables.time = 60;
		
		onFireManager.Restart();
		panelInGameGo.SetActive(true);
		InGameScriptRefrences.scoreManager.Restart();
		InGameScriptRefrences.playingObjectGeneration.Restart();
		InGameScriptRefrences.playingObjectManager.Restart();
		InGameScriptRefrences.strikerManager.Restart();
		PauseOff();		
	}
}
