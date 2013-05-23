using UnityEngine;
using System.Collections;

public class TopPanelResource : MonoBehaviour {
	public enum Type{
		TYPE_COINS,
		TYPE_DIAMOND
	}
	
	public Type type;
	public UILabel txt_resource_count;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		txt_resource_count.text = type == Type.TYPE_COINS ? GameVariables.playerParameters.coins_total.ToString() : GameVariables.playerParameters.diamond_total.ToString();
	}
	
	void OnEnable(){
		txt_resource_count.text = type == Type.TYPE_COINS ? GameVariables.playerParameters.coins_total.ToString() : GameVariables.playerParameters.diamond_total.ToString();
	}
	
	public void OnButtonDown(){
		if(type == Type.TYPE_COINS)
			InGameScriptRefrences.panelManager2D.StartBuyCoins();
		else
			InGameScriptRefrences.panelManager2D.StartBuyDiamond();
	}
}
