package com.emerginggames.lqserver;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.OutputStream;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.apache.log4j.Logger;
import org.json.JSONException;
import org.json.JSONObject;

@SuppressWarnings("serial")
public class AdminServlet extends HttpServlet  {

	private static Logger log = Logger.getLogger(AdminServlet.class);

	
	private void addCoins(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
		/*StringBuffer jb = new StringBuffer();
		String line = null;
		try {
			BufferedReader reader = req.getReader();
			while ((line = reader.readLine()) != null)
				jb.append(line);
		} catch (Exception e) { 
			resp.sendError(HttpServletResponse.SC_BAD_REQUEST);
			return;
		}
		
		JSONObject jsonObject;
		String secret;
		long facebookId;
		int coinsToAdd;
		try {
			jsonObject = new JSONObject(jb.toString());
			log.debug("addcoins jsonObject = " + jsonObject);
			secret = jsonObject.getString("secret");
			facebookId = jsonObject.getLong("facebook_id");
			coinsToAdd = jsonObject.getInt("coins_to_add");
		} catch (JSONException e) {
			resp.sendError(HttpServletResponse.SC_BAD_REQUEST);
			return;
		}
		
		PlayerController pc = PlayerController.getPlayerController(); 
		//Player p = pc.getPlayer(facebookId);

		if (secret.equals(Configuration.getConfiguration().getSecret()) && p != null && coinsToAdd != 0) {
			synchronized(p) {
				//p.setCoinsTotal(p.getCoinsTotal() + coinsToAdd);
				//p.setRandomUpdateCode();
				pc.updatePlayer(p);
				OutputStream out = resp.getOutputStream();
				String output = p.toJSON().toString() + "\n";
				out.write(output.getBytes());
				out.close();
			}
		} else {
			resp.sendError(HttpServletResponse.SC_BAD_REQUEST);
			return;
		}*/
	}
	
	@Override
	protected void doPost(HttpServletRequest req, HttpServletResponse resp)
			throws ServletException, IOException {

		String[] parts = req.getRequestURI().split("/");
		
		if (parts.length != 3) {
			resp.sendError(HttpServletResponse.SC_BAD_REQUEST);
			return;
		}
		
		String action = parts[2];
		if (action.equalsIgnoreCase("addcoins")) {
			addCoins(req, resp);
		} else {
			resp.sendError(HttpServletResponse.SC_BAD_REQUEST);
			return;
		}

	}
}
