using UnityEngine;
using System.Collections;

public class MainGameManager : MonoBehaviour {
	public GameObject panelInGameGo;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void PauseOn(){
		Time.timeScale = 0;
	}
	
	public void PauseOff(){
		Time.timeScale = 1;
	}
	
	public void Restart(){
		GameVariables.score = 0;
		GameVariables.levelBonus = 0;
		GameVariables.time = 60;
		
		InGameScriptRefrences.playingObjectGeneration.Restart();
		InGameScriptRefrences.playingObjectManager.Restart();
		PauseOff();
		panelInGameGo.SetActive(true);
	}
}
