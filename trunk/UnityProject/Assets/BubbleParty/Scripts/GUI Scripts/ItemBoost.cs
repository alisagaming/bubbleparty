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
		switch(boostType){
		case BoostType.TYPE_STAR:
			boostIcon.spriteName = "boostGoldStarActive@2x";
			bonus_bg_1.spriteName = "boostActiveListBG@2x";
			bonus_bg_2.spriteName = "btn_boostActiveBG@2x";
			bonus_count_bg.spriteName = "boostNumberGreen@2x";
			bonus_cost_type.spriteName = "coinBoost@2x";
			txt_boost_count.text = "12";
			txt_cost.text = "250";
			txt_description.text = "Score Boost";
			break;
		case BoostType.TYPE_TIME:
			boostIcon.spriteName = "boostGoldTimeActive@2x";
			bonus_bg_1.spriteName = "boostActiveListBG@2x";
			bonus_bg_2.spriteName = "btn_boostActiveBG@2x";
			bonus_count_bg.spriteName = "boostNumberGreen@2x";
			bonus_cost_type.spriteName = "coinBoost@2x";
			txt_boost_count.text = "3";
			txt_cost.text = "150";
			txt_description.text = "Time Boost";
			break;
		case BoostType.TYPE_FIREBALL:
			boostIcon.spriteName = "boostFireballActive@2x";
			bonus_bg_1.spriteName = "boostFireballListBG@2x";
			bonus_bg_2.spriteName = "btn_boostFireballBG@2x";
			bonus_count_bg.spriteName = "boostNumberGreen@2x";
			bonus_cost_type.spriteName = "diamondBoostShop@2x";			
			txt_boost_count.text = "5";
			txt_cost.text = "9";
			txt_description.text = "Fireball Boost";
			break;	
		case BoostType.TYPE_PLAZMA:
			boostIcon.spriteName = "boostPlasmaActive@2x";
			bonus_bg_1.spriteName = "boostFireballListBG@2x";
			bonus_bg_2.spriteName = "btn_boostFireballBG@2x";
			bonus_count_bg.spriteName = "boostNumberGreen@2x";
			bonus_cost_type.spriteName = "diamondBoostShop@2x";			
			txt_boost_count.text = "5";
			txt_cost.text = "12";
			txt_description.text = "Plasma Boost";
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
