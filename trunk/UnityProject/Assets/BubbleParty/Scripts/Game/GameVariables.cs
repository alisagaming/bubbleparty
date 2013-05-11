using UnityEngine;
using System.Collections;

public class GameVariables : MonoBehaviour 
{
    internal static int score = 0;
    internal static int levelBonus = 0;
    internal static float time = 60;

    //public int totalNumberOfRowsLeft = 100;
    public int minimumNumberOfRows = 3;
    //public float rowAddingInterval = 5f;
	
	internal static int totalScore = 0;
	internal static int expiriens;
	
	internal static int bonus_star = 0;
	internal static int bonus_time = 0;
	internal static int bonus_fireball = 1;
	internal static int bonus_plazma = 1;
	
	BonusCoins bonusCoins;
	
	void Start () {
		bonusCoins = BonusCoins.LoadFromFile("bonus_coins");
	}
	
	public int GetXPfromScore(){
		return 0;
	}
	
	public int GetCoinsfromScore(){
		for(int x=0;x<bonusCoins.bonusCoins.Length;x++){
			if(score<int.Parse(bonusCoins.bonusCoins[x].score)){
				if(x==0) return 0;
				else return bonusCoins.bonusCoins[x-1].amount;
			}
		}
		return 0;
	}
	
	public static int GetLevel(int exp)
	{
		return 1;
	}
	
	public static bool IsBonusUnlock(ItemBoost.BoostType type){
		switch(type){
			case ItemBoost.BoostType.TYPE_STAR: return true;
			case ItemBoost.BoostType.TYPE_TIME: return true;
			case ItemBoost.BoostType.TYPE_FIREBALL: return true;
			case ItemBoost.BoostType.TYPE_PLAZMA: return true;
		}
		return false;
	}
	
	public static int GetBonusCount(ItemBoost.BoostType type){
		switch(type){
			case ItemBoost.BoostType.TYPE_STAR: return bonus_star;
			case ItemBoost.BoostType.TYPE_TIME: return bonus_time;
			case ItemBoost.BoostType.TYPE_FIREBALL: return bonus_fireball;
			case ItemBoost.BoostType.TYPE_PLAZMA: return bonus_plazma;
		}
		return 0;
	}
	
	public void Save(){
		
	}
		
	public void Load(){
	}
}
