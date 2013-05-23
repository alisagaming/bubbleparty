package com.emerginggames.lqserver;

import java.util.HashMap;
import java.util.Random;

import javax.xml.bind.DatatypeConverter;

import org.apache.log4j.Logger;
import org.json.JSONException;
import org.json.JSONObject;
import org.junit.Test;

public class Player {
	private static Logger log = Logger.getLogger(Player.class);
	
	public long 	player_id;
	public String 	device_id;
	public String 	facebook_id;
	public String 	access_token;
	public String 	device_model;
	public String 	device_type;
	public String 	operating_system;
	
	public String region;
	public String first_name ;
	public String last_name ;
	public String email;
	
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
	
	//private JSONObject userData;	
	
	
	/*//private long playerId;
	//private long facebookId;
	//private String accessToken;
	private int scoreCount;
	private int coinsTotal;
	private int coinsSpent;
	private int bombsTotal;
	private int bombsUsed;
	private int hintsTotal;
	private int hintsUsed;
	private int resetCount;
	//private String region;
	//private String firstName;
	//private String lastName;
	//private String email;
	private HashMap<String, byte[]> levelData = new HashMap<String, byte[]>();
	private final int FreeLevelsCount = 7;
	private final int ProLevelsCount = 3;
	private boolean isOutOfDate = false;
	private String updateCode;
	private boolean postRequired = false;*/
	
	private static String getString(JSONObject json, String fieldName) throws JSONException{
		if (!json.has(fieldName)) return null;
		String str = json.getString(fieldName);
		return "null".equals(str) ? null : str;
	}
	
	private static long getLong(JSONObject json, String fieldName, long defaultValue) throws JSONException{
		if (!json.has(fieldName)) return defaultValue;
		return json.getLong(fieldName);
	}
	
	private static int getInt(JSONObject json, String fieldName, int defaultValue) throws JSONException{
		if (!json.has(fieldName)) return defaultValue;
		return json.getInt(fieldName);
	}
	
	public static Player parseFromJSON(JSONObject json) {
		if (json == null)
			return null;
		
		Player player = new Player();
		try{
			//player.player_id ;
			player.device_id	 = getString(json, "device_id");
			player.facebook_id	 = getString(json, "facebook_id");
			player.access_token	 = getString(json, "access_token");
			player.device_model  = getString(json, "device_model");
			player.device_type   = getString(json, "device_type");
			player.operating_system = getString(json, "operating_system");
		 	
			player.region 		= getString(json, "region");
			player.first_name 	= getString(json, "first_name");
			player.last_name 	= getString(json, "last_name");
			player.email		= getString(json, "email");
			
			player.coins_total	= getInt(json, "coins_total", 0);
			player.coins_spent	= getInt(json, "coins_spent", 0);
			player.diamond_total= getInt(json, "diamond_total", 0);
			player.diamond_spent= getInt(json, "diamond_spent", 0);
			player.lives_total	= getInt(json, "lives_total", 0);		
			player.lives_spent	= getInt(json, "lives_spent", 0);
			
			player.bonus_star	 = getInt(json, "bonus_star", 0);
			player.bonus_time	 = getInt(json, "bonus_time", 0);
			player.bonus_fireball= getInt(json, "bonus_fireball", 0);
			player.bonus_plazma	 = getInt(json, "bonus_plazma", 0);
			
			player.total_score		= getInt(json, "total_score", 0);
			player.total_expiriens	= getInt(json, "total_expiriens", 0);
			
		} catch (JSONException e) {
			return null;
		}
		/*try {
			if (json.has("id"))
				player.facebookId = json.getLong("id");
			else if (json.has("facebook_id"))
				player.facebookId = json.getLong("facebook_id");
			else
				return null;
			
			if (json.has("first_name"))
				player.firstName = json.getString("first_name");
			
			if (json.has("last_name"))
				player.lastName = json.getString("last_name");
			
			if (json.has("email"))
				player.email = json.getString("email");
			
			if (json.has("access_token"))
				player.accessToken = json.getString("access_token");
			
			if (json.has("score_count"))
				player.setScoreCount(json.getInt("score_count"));

			if (json.has("coins_total"))
				player.setCoinsTotal(json.getInt("coins_total"));

			if (json.has("coins_spent"))
				player.setCoinsSpent(json.getInt("coins_spent"));

			if (json.has("bombs_total"))
				player.setBombsTotal(json.getInt("bombs_total"));

			if (json.has("bombs_used"))
				player.setBombsUsed(json.getInt("bombs_used"));

			if (json.has("hints_total"))
				player.setHintsTotal(json.getInt("hints_total"));

			if (json.has("hints_used"))
				player.setHintsUsed(json.getInt("hints_used"));
			
			if (json.has("reset_count"))
				player.setResetCount(json.getInt("reset_count"));

			if (json.has("region"))
				player.setRegion(json.getString("region"));

			if (json.has("user_data")) {
				player.setUserData(json.getJSONObject("user_data"));
			} else {
				player.setUserData(new JSONObject());
			}

		} catch (JSONException e) {
			return null;
		}*/
		return player;
	}
	
	public JSONObject toJSONShort() {
		JSONObject json = new JSONObject();
		/*try {
			json.put("facebook_id", getFacebookId());

			json.put("update_code", getUpdateCode());
			
			if (isPostRequired())
				json.put("post_required", true);

		} catch (JSONException e) {
			log.error("JSONException " + e);
		}*/
		return json;
	}
	
	public JSONObject toJSON() {
		JSONObject json = new JSONObject();
		try {
			
			/*public String 	device_id;
			public String 	facebook_id;
			public String 	device_model;
			public String 	device_type;
			public String 	operating_system;
			
			public String region;
			public String first_name ;
			public String last_name ;
			public String email;
			
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
			public int total_expiriens;*/
			
			json.put("device_id", device_id);
			json.put("facebook_id", facebook_id);
			json.put("device_model", device_model);
			json.put("device_type", device_type);
			json.put("operating_system", operating_system);
			json.put("region", region);
			json.put("first_name", first_name);
			json.put("last_name", last_name);
			json.put("email", email);
			json.put("coins_total", coins_total);
			json.put("coins_spent", coins_spent);
			json.put("diamond_total", diamond_total);
			json.put("diamond_spent", diamond_spent);
			json.put("lives_total", lives_total);
			json.put("lives_spent", lives_spent);
			json.put("bonus_star", bonus_star);
			json.put("bonus_time", bonus_time);
			json.put("bonus_fireball", bonus_fireball);
			json.put("bonus_plazma", bonus_plazma);
			json.put("total_score", total_score);
			json.put("total_expiriens", total_expiriens);
			//json.put("score_count", getScoreCount());
			//json.put("coins_total", getCoinsTotal());
			//json.put("coins_spent", getCoinsSpent());
			//json.put("bombs_total", getBombsTotal());
			//json.put("bombs_used", getBombsUsed());
			//json.put("hints_total", getHintsTotal());
			//json.put("hints_used", getHintsUsed());
			//json.put("reset_count", getResetCount());
			//json.put("user_data", getUserData());
			//json.put("update_code", getUpdateCode());
			
		} catch (JSONException e) {
			log.error("JSONException " + e);
		}
		return json;
	}
	
	/*private void updateFromPlayer(Player other) {
		setScoreCount(other.getScoreCount());
		setCoinsTotal(other.getCoinsTotal());
		setCoinsSpent(other.getCoinsSpent());
		setHintsTotal(other.getHintsTotal());
		setHintsUsed(other.getHintsUsed());
		setBombsTotal(other.getBombsTotal());
		setBombsUsed(other.getBombsUsed());
		setResetCount(other.getResetCount());
		setRegion(other.getRegion());
		setUserData(other.getUserData());
	}*/
	
	public void mergeWithPlayer(Player other) {
		
		/*if (resetCount < other.resetCount) {
			// progress has been reset on the user's side once more
			// so we just replace all our data from other
			updateFromPlayer(other);
			setRandomUpdateCode();
		} else if (resetCount > other.resetCount) {
			// ignore other data
			// tell the user to pull data from us
			log.debug("resetCount > other.resetCount");
			other.setOutOfDate(true);
		} else {
			
			int scoreTotal1 = scoreCount;
			int scoreTotal2 = other.scoreCount;
			int coinsTotal1 = coinsTotal;
			int coinsTotal2 = other.coinsTotal;
			
			if (scoreCount > other.scoreCount) {
				log.debug("scoreCount > other.scoreCount");
				other.setOutOfDate(true);
			} else {
				scoreCount = other.scoreCount;
				setRandomUpdateCode();
			}
			
			if (coinsTotal > other.coinsTotal) {
				log.debug("coinsTotal > other.coinsTotal");
				other.setOutOfDate(true);
			} else {
				coinsTotal = other.coinsTotal;
				setRandomUpdateCode();
			}

			if (coinsSpent > other.coinsSpent) {
				log.debug("coinsSpent > other.coinsSpent");
				other.setOutOfDate(true);
			} else {
				coinsSpent = other.coinsSpent;
				setRandomUpdateCode();
			}

			if (bombsTotal > other.bombsTotal) {
				log.debug("bombsTotal > other.bombsTotal");
				other.setOutOfDate(true);
			} else {
				bombsTotal = other.bombsTotal;
				setRandomUpdateCode();
			}
			
			if (hintsTotal > other.hintsTotal) {
				log.debug("hintsTotal > other.hintsTotal");
				other.setOutOfDate(true);
			} else {
				hintsTotal = other.hintsTotal;
				setRandomUpdateCode();
			}

			if (hintsUsed > other.hintsUsed) {
				log.debug("hintsUsed > other.hintsUsed");
				other.setOutOfDate(true);
			} else {
				hintsUsed = other.hintsUsed;
				setRandomUpdateCode();
			}
			
			if (bombsUsed > other.bombsUsed) {
				log.debug("bombsUsed > other.bombsUsed");
				other.setOutOfDate(true);
			} else {
				bombsUsed = other.bombsUsed;
				setRandomUpdateCode();
			}
			
			if (region == null || !region.equals(other.region)) {
				region = other.region;
				setRandomUpdateCode();
			}


			for (int i = 0; i < FreeLevelsCount + ProLevelsCount; i++) {
				String level;
				if (i < FreeLevelsCount)
					level = "L" + (i+1);
				else
					level = "P" + (i+1 - FreeLevelsCount);
				
				byte[] c1 = levelData.get(level);
				byte[] c2 = other.levelData.get(level);
				
				if (c1 != null && c2 != null) {
					String s1 = getLevelFromUserData(level);
					String s2 = other.getLevelFromUserData(level);
					log.debug("s1 = '" + s1 + "'	s2 = '" + s2 + "'");
					if (!s1.equals(s2)) {
						LevelCodeOrder order = Player.compareLevelCodes(c1, c2);
						log.debug("order " + order);
						if (order == LevelCodeOrder.Less) {
							
							log.debug("New level data");
							
							modifyUserData(level, other.getLevelFromUserData(level));
							setRandomUpdateCode();
						
						} else if (order == LevelCodeOrder.Greater) {
							log.debug("order == LevelCodeOrder.Greater for level " + level);
							other.setOutOfDate(true);
							int score1 = Player.scoreForLevelCode(c1); 
							int score2 = Player.scoreForLevelCode(c2);
							
							for (int j = 0; j < c1.length; j++)
								c1[j] |= c2[j];
							
							int score_merge = Player.scoreForLevelCode(c1);
							
							log.debug("total1="+scoreTotal1+" total2="+scoreTotal2);
							log.debug("score1="+score1+" score2="+score2);
							log.debug("score_merge="+score_merge);
							setScoreCount(score_merge + Math.max(scoreTotal1-score1, scoreTotal2-score2));
							setCoinsTotal(score_merge + Math.max(coinsTotal1-score1, coinsTotal2-score2));

						} else if (order == LevelCodeOrder.Unordered) {
							other.setOutOfDate(true);
							log.debug("Level " + level + " is out of sync. Syncing now...");
							// merge all level bits
							int score1 = Player.scoreForLevelCode(c1); 
							int score2 = Player.scoreForLevelCode(c2);
							
							for (int j = 0; j < c1.length; j++)
								c1[j] |= c2[j];
							
							int score_merge = Player.scoreForLevelCode(c1);
							
							log.debug("total1="+scoreTotal1+" total2="+scoreTotal2);
							log.debug("score1="+score1+" score2="+score2);
							log.debug("score_merge="+score_merge);
							setScoreCount(score_merge + Math.max(scoreTotal1-score1, scoreTotal2-score2));
							setCoinsTotal(score_merge + Math.max(coinsTotal1-score1, coinsTotal2-score2));
							
							modifyUserData(level, DatatypeConverter.printBase64Binary(c1));
							setRandomUpdateCode();
						}
					}
				} else if (c1 != null) {

					if (level.charAt(0) == 'L') {
						log.debug("no data for level " + level);
						other.setOutOfDate(true);
					}
					
				} else if (c2 != null) {
					log.debug("c1 is null");
					modifyUserData(level, other.getLevelFromUserData(level));
					setRandomUpdateCode();
				}
			}
		}*/
	}
	
	@Test
	public void testBase64() {
		byte[] code = DatatypeConverter.parseBase64Binary("//////PDw/8z/Pz////Pvw==");
		System.out.println("Code: " + DatatypeConverter.printHexBinary(code) + " (" + code.length + " bytes)");
		
	}
	
	@Test
	public void testScoreForLevelCode() {
		byte[] code = DatatypeConverter.parseBase64Binary("AAAAAAAAAAADAAAMAAAAAA==");
		int score = Player.scoreForLevelCode(code);
		System.out.println("score="+score);
	}
	
	private static int scoreForLevelCode(byte[] code) {
		int score = 0;
		for (int i = 0; i < code.length; i++) {
			score += ((code[i] & (3 << 0)) >> 0);
			score += ((code[i] & (3 << 2)) >> 2);
			score += ((code[i] & (3 << 4)) >> 4);
			score += ((code[i] & (3 << 6)) >> 6);
		}
		return score;
	}
	
	/*private void modifyUserData(String key, String value) {
		try {
			userData.put(key, value);
			byte[] levelCode = DatatypeConverter.parseBase64Binary(value);
			levelData.put(key, levelCode);
		} catch (JSONException e) {
			log.error("JSONException " + e);
		}
	}*/
	
	/*private String getLevelFromUserData(String level) {
		try {
			return userData.getString(level);
		} catch (JSONException e) {
			return null;
		}
	}
	
	private enum LevelCodeOrder {
		Same,
		Less,
		Greater,
		Unordered
	};
	
	private static LevelCodeOrder compareLevelCodes(byte[] c1, byte[] c2) {
		LevelCodeOrder result = LevelCodeOrder.Same;
		log.debug("compareLevelCodes c1=" + c1 + "; c2=" + c2);
		for (int i = 0; i < Math.max(c1.length, c2.length); i++) {
			byte b1 = (i < c1.length) ? c1[i] : 0;
			byte b2 = (i < c2.length) ? c2[i] : 0;

			if (b1 == b2)
				continue;
			if ( (b1|b2) == b2 ) {
				if (result != LevelCodeOrder.Greater)
					result = LevelCodeOrder.Less;
				else
					return LevelCodeOrder.Unordered;
			} else if ( (b1|b2) == b1 ) {
				if (result != LevelCodeOrder.Less)
					result = LevelCodeOrder.Greater;
				else
					return LevelCodeOrder.Unordered;
			} else
				return LevelCodeOrder.Unordered;
		}
		return result;
	}*/
	
	public long getPlayerId() {
		return player_id;
	}
	public void setPlayerId(long playerId) {
		this.player_id = playerId;
	}
	public String getAccessToken() {
		return access_token;
	}
	public void setAccessToken(String accessToken) {
		this.access_token = accessToken;
	}
	/*public int getScoreCount() {
		return scoreCount;
	}
	public void setScoreCount(int scoreCount) {
		this.scoreCount = scoreCount;
	}
	public int getCoinsTotal() {
		return coinsTotal;
	}
	public void setCoinsTotal(int coinsTotal) {
		this.coinsTotal = coinsTotal;
	}
	public int getBombsTotal() {
		return bombsTotal;
	}
	public void setBombsTotal(int bombsTotal) {
		this.bombsTotal = bombsTotal;
	}*/
	public String getRegion() {
		return region;
	}
	public void setRegion(String region) {
		this.region = region;
	}
	public String getFirstName() {
		return first_name;
	}
	public void setFirstName(String firstName) {
		this.first_name = firstName;
	}
	public String getLastName() {
		return last_name;
	}
	public void setLastName(String lastName) {
		this.last_name = lastName;
	}

	/*public int getHintsTotal() {
		return hintsTotal;
	}

	public void setHintsTotal(int hintsTotal) {
		this.hintsTotal = hintsTotal;
	}*/

	public String getFacebookId() {
		return facebook_id;
	}

	public void setFacebookId(String facebookId) {
		this.facebook_id = facebookId;
	}

	/*public JSONObject getUserData() {
		return userData;
	}

	/*public void setUserData(JSONObject userData) {
		this.userData = userData;
		String[] keys = JSONObject.getNames(userData);
		if (keys == null)
			return;
		
		levelData.clear();
		
		for (String name: keys) {
			try {
				byte[] levelCode = DatatypeConverter.parseBase64Binary(userData.getString(name));
				levelData.put(name, levelCode);
			} catch (JSONException e) {
			}
		}

	}*/

	/*public int getCoinsSpent() {
		return coinsSpent;
	}

	public void setCoinsSpent(int coinsSpent) {
		this.coinsSpent = coinsSpent;
	}

	public int getBombsUsed() {
		return bombsUsed;
	}

	public void setBombsUsed(int bombsUsed) {
		this.bombsUsed = bombsUsed;
	}

	public int getHintsUsed() {
		return hintsUsed;
	}

	public void setHintsUsed(int hintsUsed) {
		this.hintsUsed = hintsUsed;
	}

	public int getResetCount() {
		return resetCount;
	}

	public void setResetCount(int resetCount) {
		this.resetCount = resetCount;
	}*/

	public String getEmail() {
		return email;
	}

	public void setEmail(String email) {
		this.email = email;
	}

	/*public boolean isOutOfDate() {
		return isOutOfDate;
	}

	public void setOutOfDate(boolean isOutOfDate) {
		this.isOutOfDate = isOutOfDate;
	}

	public String getUpdateCode() {
		return updateCode;
	}

	public void setRandomUpdateCode() {
		byte [] array = new byte[10];
		Random random = new Random();
		random.nextBytes(array);
		this.updateCode = DatatypeConverter.printBase64Binary(array);
	}

	public boolean isPostRequired() {
		return postRequired;
	}

	public void setPostRequired(boolean postRequired) {
		this.postRequired = postRequired;
	}*/
}
