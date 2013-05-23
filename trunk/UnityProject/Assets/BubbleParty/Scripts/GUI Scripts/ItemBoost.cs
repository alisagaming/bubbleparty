using UnityEngine;
using System.Collections;

public class ItemBoost : MonoBehaviour {
	
	public enum BoostType{
		TYPE_STAR,
		TYPE_TIME,
		TYPE_FIREBALL,
		TYPE_PLAZMA
	}
	
	public BoostType boostType;
	public UISprite boostIcon;
	public UISprite bonus_bg_1;
	public UISprite bonus_bg_2;
	public UISprite bonus_count_bg;
	public UISprite bonus_cost_type;
	public UILabel txt_boost_count;
	public UILabel txt_cost;
	public UILabel txt_description;
	
	// Use this for initialization
	void Start () {
		
		txt_boost_count.text = GameVariables.GetBonusCount(boostType).ToString();
		
		switch(boostType){
			case BoostType.TYPE_STAR:
				txt_description.text = "Score Boost";
				break;
			case BoostType.TYPE_TIME:
				txt_description.text = "Time Boost";
				break;
			case BoostType.TYPE_FIREBALL:
				txt_description.text = "Fireball Boost";
				break;
			case BoostType.TYPE_PLAZMA:
				txt_description.text = "Plasma Boost";
				break;
		}
		
		if(!GameVariables.IsBonusUnlock(boostType)){
			bonus_bg_1.spriteName = "boostInactiveListBG@2x";
			bonus_bg_2.spriteName = "btn_boostInactiveBG@2x";
			bonus_count_bg.gameObject.SetActive(false);
			txt_boost_count.gameObject.SetActive(false);
			switch(boostType){
			case BoostType.TYPE_STAR:
				boostIcon.spriteName = "boostGoldStarInactive@2x";
				break;
			case BoostType.TYPE_TIME:
				boostIcon.spriteName = "boostGoldTimeInactive@2x";
				break;
			case BoostType.TYPE_FIREBALL:
				boostIcon.spriteName = "boostFireballInactive@2x";
				break;
			case BoostType.TYPE_PLAZMA:
				boostIcon.spriteName = "boostPlasmaInactive@2x";
				break;
			}
			return;
		}
		
		bonus_count_bg.gameObject.SetActive(true);
		bonus_count_bg.spriteName = GameVariables.GetBonusCount(boostType) == 0 ? "boostNumberRed@2x" : "boostNumberGreen@2x";
		txt_boost_count.gameObject.SetActive(true);
		
		
		switch(boostType){
		case BoostType.TYPE_STAR:
			boostIcon.spriteName = "boostGoldStarActive@2x";			
				
			//if(GameVariables.bonus_star>0)
			{
				bonus_bg_1.spriteName = "boostActiveListBG@2x";
				bonus_bg_2.spriteName = "btn_boostActiveBG@2x";
				//bonus_count_bg.spriteName = "boostNumberGreen@2x";
				bonus_cost_type.spriteName = "coinBoost@2x";
				//txt_boost_count.text = "12";
				//txt_cost.text = "250";
				//txt_description.text = "Score Boost";
			}/*else{
				
			}*/
			break;
		case BoostType.TYPE_TIME:
			boostIcon.spriteName = "boostGoldTimeActive@2x";
			bonus_bg_1.spriteName = "boostActiveListBG@2x";
			bonus_bg_2.spriteName = "btn_boostActiveBG@2x";
			//bonus_count_bg.spriteName = "boostNumberGreen@2x";
			bonus_cost_type.spriteName = "coinBoost@2x";
			//txt_boost_count.text = "3";
			//txt_cost.text = "150";
			//txt_description.text = "Time Boost";
			break;
		case BoostType.TYPE_FIREBALL:
			boostIcon.spriteName = "boostFireballActive@2x";
			bonus_bg_1.spriteName = "boostFireballListBG@2x";
			bonus_bg_2.spriteName = "btn_boostFireballBG@2x";
			//bonus_count_bg.spriteName = "boostNumberGreen@2x";
			bonus_cost_type.spriteName = "diamondBoostShop@2x";			
			//txt_boost_count.text = "5";
			//txt_cost.text = "9";
			//txt_description.text = "Fireball Boost";
			break;	
		case BoostType.TYPE_PLAZMA:
			boostIcon.spriteName = "boostPlasmaActive@2x";
			bonus_bg_1.spriteName = "boostFireballListBG@2x";
			bonus_bg_2.spriteName = "btn_boostFireballBG@2x";
			//bonus_count_bg.spriteName = "boostNumberGreen@2x";
			bonus_cost_type.spriteName = "diamondBoostShop@2x";			
			//txt_boost_count.text = "5";
			//txt_cost.text = "12";
			//txt_description.text = "Plasma Boost";
			break;
		}
		txt_cost.text = GetBonusCost().ToString();
	}
	
	int GetBonusCost(){
		switch(boostType){
		case BoostType.TYPE_STAR: return 250;
		case BoostType.TYPE_TIME: return 150;
		case BoostType.TYPE_FIREBALL: return 9;
		case BoostType.TYPE_PLAZMA: return 12;
		}
		return 0;
	}
	
	public void OnButtonAdd(){
		int cost = GetBonusCost();
		switch(boostType){
		case BoostType.TYPE_STAR:
		case BoostType.TYPE_TIME:
			if(cost <= GameVariables.playerParameters.coins_total){
				GameVariables.playerParameters.coins_total -= cost;
				GameVariables.playerParameters.coins_spent += cost;
				if(boostType == BoostType.TYPE_STAR)
					GameVariables.playerParameters.bonus_star++;
				else
					GameVariables.playerParameters.bonus_time++;
				GameVariables.Save();
				Start();
			}else InGameScriptRefrences.panelManager2D.StartBuyCoins();
			break;
		case BoostType.TYPE_FIREBALL:
		case BoostType.TYPE_PLAZMA:
			if(cost <= GameVariables.playerParameters.diamond_total){
				GameVariables.playerParameters.diamond_total -= cost;
				GameVariables.playerParameters.diamond_spent += cost;
				if(boostType == BoostType.TYPE_FIREBALL)
					GameVariables.playerParameters.bonus_fireball++;
				else
					GameVariables.playerParameters.bonus_plazma++;
				GameVariables.Save();
				Start();
			}else InGameScriptRefrences.panelManager2D.StartBuyDiamond();
			break;
		}
	}
	
	public void OnButtonInfo(){
		switch(boostType){
		case BoostType.TYPE_STAR:
			InGameScriptRefrences.panelManager2D.StartHelpStar();
			break;
		case BoostType.TYPE_TIME:
			InGameScriptRefrences.panelManager2D.StartHelpTime();
			break;
		case BoostType.TYPE_FIREBALL:
			InGameScriptRefrences.panelManager2D.StartHelpFireball();
			break;
		case BoostType.TYPE_PLAZMA:
			InGameScriptRefrences.panelManager2D.StartHelpPlazma();
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
