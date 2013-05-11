using UnityEngine;
using System.Collections;

public class ItenInGameBoost : MonoBehaviour {
	
	public ItemBoost.BoostType boostType;
	public UISprite bonus_bg;
	public UISprite bonus_icon;
	public PlayingObjectGeneration playingObjectGeneration;
	
	
	// Use this for initialization
	void Start () {
		switch(boostType){
			case ItemBoost.BoostType.TYPE_STAR: 
				bonus_bg.spriteName = GameVariables.GetBonusCount(boostType) > 0 ?  "btn_boostActiveBG@2x" : "btn_boostInactiveBG@2x";	
				bonus_icon.spriteName = GameVariables.GetBonusCount(boostType) > 0 ?  "boostGoldStarActive@2x" : "boostGoldStarInactive@2x";
				playingObjectGeneration.enableStar = GameVariables.GetBonusCount(boostType) > 0;
				break;
			case ItemBoost.BoostType.TYPE_TIME: 
				bonus_bg.spriteName = GameVariables.GetBonusCount(boostType) > 0 ?  "btn_boostActiveBG@2x" : "btn_boostInactiveBG@2x";
				bonus_icon.spriteName = GameVariables.GetBonusCount(boostType) > 0 ?  "boostGoldTimeActive@2x" : "boostGoldTimeInactive@2x";
				playingObjectGeneration.enableTime = GameVariables.GetBonusCount(boostType) > 0;
				break;
			case ItemBoost.BoostType.TYPE_FIREBALL: 
				bonus_bg.spriteName = GameVariables.GetBonusCount(boostType) > 0 ?  "btn_boostFireballBG@2x" : "btn_boostInactiveBG@2x";		
				bonus_icon.spriteName = GameVariables.GetBonusCount(boostType) > 0 ?  "boostFireballActive@2x" : "boostFireballInactive@2x";
				playingObjectGeneration.enableFireball = GameVariables.GetBonusCount(boostType) > 0;
				break;
			case ItemBoost.BoostType.TYPE_PLAZMA:  
				bonus_bg.spriteName = GameVariables.GetBonusCount(boostType) > 0 ?  "btn_boostFireballBG@2x" : "btn_boostInactiveBG@2x";		
				bonus_icon.spriteName = GameVariables.GetBonusCount(boostType) > 0 ?  "boostPlasmaActive@2x" : "boostPlasmaInactive@2x";
				playingObjectGeneration.enablePlazma = GameVariables.GetBonusCount(boostType) > 0;
				break;			
		}
		//if(GameVariables.GetBonusCount(boostType)>0)
		//	iTween.MoveBy(bonus_icon.gameObject, new Vector3(0, 0.1f, 0), 1f);
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
