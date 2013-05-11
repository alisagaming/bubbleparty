using UnityEngine;
using System.Collections;

public class PanelStatistic : MonoBehaviour {
	
	public UILabel txt_stat_score;
	public UILabel txt_stat_star;
	public UILabel txt_stat_coins;
	public UILabel txt_stat_caption;
	
	int coins;
	int exp;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnEnable() {
		coins = InGameScriptRefrences.gameVariables.GetCoinsfromScore();
		exp = InGameScriptRefrences.gameVariables.GetXPfromScore();
	}
}
