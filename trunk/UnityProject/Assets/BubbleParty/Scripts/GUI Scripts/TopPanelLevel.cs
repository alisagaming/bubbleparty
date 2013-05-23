using UnityEngine;
using System.Collections;

public class TopPanelLevel : MonoBehaviour {
	
	public UISprite bar_level;
	public UILabel txt_level_caption;
	public UILabel txt_level;
	
	public void OnEnable(){
		int levelPercent = GameVariables.GetLevelPersent(GameVariables.playerParameters.total_expiriens);
		txt_level_caption.text = levelPercent + "%";
		bar_level.transform.localPosition = new Vector3(-1.8f*(100-levelPercent),0,0);
		txt_level.text = GameVariables.GetLevel(GameVariables.playerParameters.total_expiriens).ToString();
	}
}
