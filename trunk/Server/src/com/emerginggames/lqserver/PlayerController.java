package com.emerginggames.lqserver;

import java.sql.SQLException;
import java.util.HashMap;

import org.apache.log4j.Logger;

import com.emerginggames.lqserver.db.Dao;

public class PlayerController {
	private final Logger log = Logger.getLogger(FacebookController.class);
	private static PlayerController instance = null;
	
	private HashMap<String, Player> playersMapDeviceID;
	private HashMap<String, Player> playersMapFBID;

	public synchronized static PlayerController getPlayerController() {
		if (instance == null) {
			instance = new PlayerController();
			//instance.playersMap = new HashMap<Long, Player>();
			instance.playersMapDeviceID = new HashMap<String, Player>();
			instance.playersMapFBID = new HashMap<String, Player>();
		}
		return instance;
	}
	
	synchronized Player getPlayer(boolean isFB, String Id) {
		//Long value = Long.valueOf(Id);
		
		HashMap<String, Player> hashMap = isFB ? playersMapFBID : playersMapDeviceID;
		
		Player p = hashMap.get(Id);
		
		if (p == null) {
			try {
				Dao dao = new Dao();
				p = dao.loadPlayerFB(isFB, Id);
				if (p != null) {
					addPlayerToHash(hashMap, p);
				}
				dao.close();
			} catch (SQLException e) {
				log.error("SQLException: " + e);
				return null;
			}
		}
		
		return p;
	}
	
	synchronized void addNewPlayer(HashMap<String, Player> hashMap, Player p) {
		try {
			Dao dao = new Dao();
			dao.insertPlayer(p);
			addPlayerToHash(hashMap, p);
			dao.close();
			
		} catch (SQLException e) {
			log.error("SQLException: " + e);
			return;
		}
	}
	
	synchronized void addPlayerToHash(HashMap<String, Player> hashMap, Player p) {
		//Long value = Long.valueOf(p.getFacebookId());
		hashMap.put(p.getFacebookId(), p);
	}
	
	public Player updatePlayer(Player p) {
		try {
			if(p.facebook_id != null){
				
			}else{
				Player hashPlayer = getPlayer(false, p.device_id);
				if(hashPlayer == null){ 
					addNewPlayer(playersMapDeviceID, p);
					return p;
				}else{
					if(hashPlayer.total_expiriens >= p.total_expiriens)
						return hashPlayer;
					else{
						Dao dao = new Dao();
						dao.updatePlayer(p);
						addPlayerToHash(playersMapDeviceID, p);
						dao.close();
						return p;
					}
				}
			}				
			
			/*Dao dao = new Dao();
			dao.updatePlayer(p);
			addPlayer(p);
			dao.close();*/
			return null;
		} catch (SQLException e) {
			log.error("SQLException: " + e);
			return null;
		}
	}

}
