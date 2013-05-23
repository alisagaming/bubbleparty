using UnityEngine;
using System.Collections;

public class GameVariables : MonoBehaviour 
{
	public class PlayerParameters{
		public long 	player_id;
		public string 	device_id;
		public string 	facebook_id;
		public string 	access_token;
		public string 	device_model;
		public string 	device_type;
		public string 	operating_system;
		
		public string region;
		public string first_name ;
		public string last_name ;
		public string email;
		
		public int coins_total;
		public int coins_spent;
		public int diamond_total;
		public int diamond_spent;
		public int lives_total;		
		public int lives_spent;
		
		public int bonus_star;
		public int bonus_time;
		public int bonus_fireball;
		public int bonus_plazma;
		
		
		public int total_score;
		public int total_expiriens;
	}
	
	static GameVariables instance;
	
	internal static PlayerParameters playerParameters = new PlayerParameters();
	
    internal static int score = 0;
    //internal static int levelBonus = 0;
    internal static float time = 60;

    //internal static int totalScore = 0;
	//internal static int expiriens;
	
	//internal static int coins  = 2000;
	//internal static int diamond= 100;
	//internal static int lives  = 5;
	
	
	//internal static int bonus_star = 0;
	//internal static int bonus_time = 0;
	//internal static int bonus_fireball = 0;
	//internal static int bonus_plazma = 0;
	
	public int minimumNumberOfRows = 10;
    	
	BonusCoins bonusCoins;
	
	void Start () {
		instance = this;
		LoadLocal();
		//string str = SystemInfo.deviceUniqueIdentifier;
		//str = "";
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
		return 3;
	}
	
	public static int GetLevelPersent(int exp)
	{
		return 56;
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
			case ItemBoost.BoostType.TYPE_STAR: return playerParameters.bonus_star;
			case ItemBoost.BoostType.TYPE_TIME: return playerParameters.bonus_time;
			case ItemBoost.BoostType.TYPE_FIREBALL: return playerParameters.bonus_fireball;
			case ItemBoost.BoostType.TYPE_PLAZMA: return playerParameters.bonus_plazma;
		}
		return 0;
	}
	
	public static void Save(){
		instance.SaveStart();
		//StartCoroutine(ClientServer.Sync());		
	}
	
	void SaveStart(){
		StartCoroutine(ClientServer.Sync());	
	}
		
	public static void Load(){
	}
	
	static void LoadLocal(){
		playerParameters.player_id = -1;
		playerParameters.access_token = null;
		playerParameters.facebook_id = null;
		playerParameters.device_id = SystemInfo.deviceUniqueIdentifier;
		playerParameters.device_model = SystemInfo.deviceModel;
		playerParameters.operating_system = SystemInfo.operatingSystem;	
		playerParameters.device_type = SystemInfo.deviceType.ToString();
		
		playerParameters.coins_total = 2000;
		playerParameters.diamond_total = 100;
		playerParameters.lives_total = 4;	
		
		playerParameters.first_name = "Test Name";//"asd И ФЫВА Имя тест";
	}
}
