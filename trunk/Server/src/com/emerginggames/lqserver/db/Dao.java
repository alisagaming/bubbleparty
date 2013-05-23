package com.emerginggames.lqserver.db;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;

import org.apache.log4j.Logger;
import org.json.JSONException;
import org.json.JSONObject;

import com.emerginggames.lqserver.Player;

public class Dao {
	protected Connection c = null;
	Logger log = Logger.getLogger(Dao.class);

	protected Dao(boolean autoCommit) throws SQLException{
		c = Connector.getConnection();
		try{
			if (autoCommit)
				c.setAutoCommit(true);
			else
				c.setAutoCommit(false);
		}
		catch (Exception e){
			if (c == null){
				e.printStackTrace();
				log.info(e.getMessage());
				throw new SQLException(e);
			}
			log.info("dao conneciton stale, reconnecting");
			
			c = Connector.getConnection();

			if (autoCommit)
				c.setAutoCommit(true);
			else
				c.setAutoCommit(false);
		}
	}

	public Dao() throws SQLException{
		c = Connector.getConnection();
		try{
			c.setAutoCommit(true);
		}catch (Exception e){//try again if connection stale!
			if (c == null){
				e.printStackTrace();
				log.info(e.getMessage());
				throw new SQLException(e);
			}
			log.info("dao conneciton stale, reconnecting");
			c = Connector.getConnection();
			c.setAutoCommit(true);
		}
	}
	
	protected Dao(Dao d){
		this.c = d.c;
	}
	
	public void rollback(){
		try {
			c.rollback();
		} catch (SQLException e) {
			e.printStackTrace();
			log.error("Failed to rollback connection");
		}
	}
	
	public void close(){
		try {
			if (c.getAutoCommit() == false)
				close(true);
			else
				c.close();
		} catch (SQLException e) {
			e.printStackTrace();
			log.error("Failed to close connection c gracefully");
		}
	}

	public void close(boolean commit){
		try{
			if (commit)
				c.commit();
			else
				c.rollback();
		
			c.close();
		}
		catch (SQLException se){
			se.printStackTrace();
			log.info("failed to close connection");
		}
	}
	
	public Player insertPlayer(Player p) {
		log.debug("inserting player " + p.getFacebookId());
		
		//String q = "insert into players (device_id, facebook_id, first_name, last_name, region, email, registration_date, last_played_date)"
		//		+ " values (?, ?, ?, ?, ?, now(), now())";
		String q = "insert into players (registration_date, last_played_date,"
					+"device_id, facebook_id, device_model, device_type, operating_system, region, first_name, last_name, email,"
				    +"coins_total, coins_spent, diamond_total, diamond_spent, lives_total, lives_spent, bonus_star, bonus_time, bonus_fireball, bonus_plazma, total_score, total_expiriens)"
						+ " values (now(), now(), ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
		PreparedStatement stmt = null;
		
		try {
			stmt = c.prepareStatement(q, Statement.RETURN_GENERATED_KEYS);
			stmt.setString(1, p.device_id);
			stmt.setString(2, p.facebook_id);
			stmt.setString(3, p.device_model);
			stmt.setString(4, p.device_type);
			stmt.setString(5, p.operating_system);
			stmt.setString(6, p.region);
			stmt.setString(7, p.first_name);
			stmt.setString(8, p.last_name);
			stmt.setString(9, p.email);
			stmt.setInt(10, p.coins_total);
			stmt.setInt(11, p.coins_spent);
			stmt.setInt(12, p.diamond_total);
			stmt.setInt(13, p.diamond_spent);
			stmt.setInt(14, p.lives_total);
			stmt.setInt(15, p.lives_spent);
			stmt.setInt(16, p.bonus_star);
			stmt.setInt(17, p.bonus_time);
			stmt.setInt(18, p.bonus_fireball);
			stmt.setInt(19, p.bonus_plazma);
			stmt.setInt(20, p.total_score);
			stmt.setInt(21, p.total_expiriens);
			
			stmt.executeUpdate();
			ResultSet rs = stmt.getGeneratedKeys();
			if (rs.next()) {
				p.setPlayerId(rs.getInt(1));
			}
		} catch (SQLException e) {
			log.error("SQLException: " + e.getMessage());
		} finally {
			if (stmt != null)
				try {
					stmt.close();
				} catch (SQLException e) {
					log.error("SQLException: " + e.getMessage());
				}
		}

		return p;
	}
	
	public Player loadPlayerFB(boolean isFB, String Id) {
		String q = isFB ? "select * from players where facebook_id = " + Id : "select * from players where device_id = \"" + Id+"\"";
		ResultSet rst = null;
		PreparedStatement stmt = null;
		Player p = null;
		
		try {
			stmt = c.prepareStatement(q);
			rst = stmt.executeQuery();
			if (rst.next()) {
				p = new Player();
				if(isFB) p.facebook_id = Id; else p.device_id = Id;
				
				p.device_model = rst.getString("device_model");
				p.device_type = rst.getString("device_type");
				p.operating_system = rst.getString("operating_system");
				p.region = rst.getString("region");
				p.first_name = rst.getString("first_name");
				p.last_name = rst.getString("last_name");
				p.email = rst.getString("email");
				p.coins_total = rst.getInt("coins_total");
				p.coins_spent = rst.getInt("coins_spent");
				p.diamond_total = rst.getInt("diamond_total");
				p.diamond_spent = rst.getInt("diamond_spent");
				p.lives_total = rst.getInt("lives_total");
				p.lives_spent = rst.getInt("lives_spent");
				p.bonus_star = rst.getInt("bonus_star");
				p.bonus_time = rst.getInt("bonus_time");
				p.bonus_fireball = rst.getInt("bonus_fireball");
				p.bonus_plazma = rst.getInt("bonus_plazma");
				p.total_score = rst.getInt("total_score");
				p.total_expiriens = rst.getInt("total_expiriens");
				/*p.setFacebookId(facebookId);
				p.setPlayerId(rst.getInt("player_id"));
				p.setFirstName(rst.getString("first_name"));
				p.setLastName(rst.getString("last_name"));
				p.setRegion(rst.getString("region"));	*/			
			}
		} catch (SQLException e) {
			log.error("SQLException: " + e.getMessage());
		} finally {
			if (rst != null)
				try {
					rst.close();
				} catch (SQLException e) {
					log.error("SQLException: " + e.getMessage());
				}
			if (stmt != null)
				try {
					stmt.close();
				} catch (SQLException e) {
					log.error("SQLException: " + e.getMessage());
				}
		}
		
		return p;
	}
	
	
	public boolean updatePlayer(Player p) {
		/*String q = 	"update players set " +
					" score_count = ?," + 
					" coins_total = ?," + 
					" coins_spent = ?," + 
					" hints_total = ?," + 
					" hints_used  = ?," + 
					" bombs_total = ?," + 
					" bombs_used  = ?," +
					" reset_count = ?," +
					" region = ?," + 
					" user_data = ?," + 
					" last_played_date = now() " + 
					" where facebook_id = " + p.getFacebookId();
		log.debug(q);
		PreparedStatement stmt = null;
		boolean ok = false;
		try {
			stmt = c.prepareStatement(q);
			stmt.setInt(1, p.getScoreCount());
			stmt.setInt(2, p.getCoinsTotal());
			stmt.setInt(3, p.getCoinsSpent());
			stmt.setInt(4, p.getHintsTotal());
			stmt.setInt(5, p.getHintsUsed());
			stmt.setInt(6, p.getBombsTotal());
			stmt.setInt(7, p.getBombsUsed());
			stmt.setInt(8, p.getResetCount());
			stmt.setString(9, p.getRegion());
			if (p.getUserData() != null)
				stmt.setString(10, p.getUserData().toString());
			else
				stmt.setString(10, null);
			stmt.executeUpdate();
			ok = true;
		} catch (SQLException e) {
			log.error("SQLException: " + e.getMessage());
		} finally {
			if (stmt != null)
				try {
					stmt.close();
				} catch (SQLException e) {
					log.error("SQLException: " + e.getMessage());
				}
		}		
		return ok;*/
		return true;
	}
}
