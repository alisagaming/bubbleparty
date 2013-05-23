package com.emerginggames.lqserver;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.OutputStream;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.apache.http.protocol.HTTP;
import org.apache.log4j.Logger;
import org.json.JSONException;
import org.json.JSONObject;

@SuppressWarnings("serial")
public class PlayerServlet extends HttpServlet {
	
	private static Logger log = Logger.getLogger(PlayerServlet.class);

	/*
	@Override
	protected void doGet(HttpServletRequest req, HttpServletResponse resp) 
			throws ServletException, IOException {
		
		log.debug("GET request " + req.getQueryString());
		
		String[] parts = req.getRequestURI().split("/");
		
		if (parts.length != 3) {
			resp.sendError(HttpServletResponse.SC_BAD_REQUEST);
			return;
		}
		
		long facebookId;
		try {
			facebookId = Long.parseLong(parts[parts.length - 1]);
		} catch (NumberFormatException e) {
			resp.sendError(HttpServletResponse.SC_BAD_REQUEST);
			return;
		}
		
		String accessToken = req.getParameter("access_token");
		if (accessToken == null) {
			resp.sendError(HttpServletResponse.SC_UNAUTHORIZED);
			return;
		}
		
		String updateCode = req.getParameter("update_code");
		log.debug("update_code = " + updateCode);

		PlayerController pc = PlayerController.getPlayerController(); 
		Player p = pc.getPlayer(facebookId);
		
		boolean isNewPlayer = false;
		
		if (p == null) {

			p = Player.parseFromJSON(FacebookController.getFacebookController().getMe(accessToken));
			
			synchronized(p) {
				if (p != null && p.getFacebookId() == facebookId) {
					pc.addNewPlayer(p);
					isNewPlayer = true;
					p.setPostRequired(true);
					p.setAccessToken(accessToken);
				}
			}
		} else if (p.getAccessToken() == null) {
			Player fbPlayer = Player.parseFromJSON(FacebookController.getFacebookController().getMe(accessToken));
			if (fbPlayer != null && fbPlayer.getFacebookId() == p.getFacebookId())
				p.setAccessToken(accessToken);
			else {
				resp.sendError(HttpServletResponse.SC_UNAUTHORIZED);
				return;
			}
		}
		
		
		if (p != null && p.getFacebookId() == facebookId) {
			OutputStream out = resp.getOutputStream();
			String output;
			if (isNewPlayer ||
					(updateCode != null && updateCode.equals(p.getUpdateCode())))
				output = p.toJSONShort().toString() + "\n";
			else
				output = p.toJSON().toString() + "\n";
			log.debug("GET response: " + output);
			out.write(output.getBytes());
			out.close();
		} else {
			resp.sendError(HttpServletResponse.SC_FORBIDDEN);
			return;
		}
	}*/
	
	private String unescape(String s) {
	    int i=0, len=s.length();
	    char c;
	    StringBuffer sb = new StringBuffer(len);
	    while (i < len) {
	        c = s.charAt(i++);
	        if (c == '\\') {
	            if (i < len) {
	                c = s.charAt(i++);
	                if (c == 'u') {
	                    // TODO: check that 4 more chars exist and are all hex digits
	                    c = (char) Integer.parseInt(s.substring(i, i+4), 16);
	                    i += 4;
	                } // add other cases here as desired...
	            }
	        } // fall through: \ escapes itself, quotes any character but u
	        sb.append(c);
	    }
	    return sb.toString();
	}
	
	// convert from UTF-8 -> internal Java String format
    public static String convertFromUTF8(String s) {
        String out = null;
        try {
            out = new String(s.getBytes("ISO-8859-1"), "UTF-8");
        } catch (java.io.UnsupportedEncodingException e) {
            return null;
        }
        return out;
    }
 
    // convert from internal Java String format -> UTF-8
    public static String convertToUTF8(String s) {
        String out = null;
        try {
            out = new String(s.getBytes("UTF-8"), "ISO-8859-1");
        } catch (java.io.UnsupportedEncodingException e) {
            return null;
        }
        return out;
    }
	
	@Override
	protected void doPost(HttpServletRequest req, HttpServletResponse resp)
			throws ServletException, IOException {

		StringBuffer jb = new StringBuffer();
		String line = null;
		try {
			BufferedReader reader = req.getReader();
			while ((line = reader.readLine()) != null)
				jb.append(line);
		} catch (Exception e) { 
				log.debug("exception " + e);
			resp.sendError(HttpServletResponse.SC_BAD_REQUEST);
			return;
		}
		
		JSONObject jsonObject;
		try {
			String str = jb.toString();
			str = unescape(str);	
			//str = convertFromUTF8(str);
			str = new String(str.getBytes(), "UTF-8");
			jsonObject = new JSONObject(str);
			log.debug("POST body = " + jsonObject);
		} catch (JSONException e) {
			log.debug("exception " + e);
			resp.sendError(HttpServletResponse.SC_BAD_REQUEST);
			return;
		}
		
		Player player = Player.parseFromJSON(jsonObject);

		if (player == null) {
			log.debug("player is null");
			resp.sendError(HttpServletResponse.SC_BAD_REQUEST);
			return;
		}

		/*String[] parts = req.getRequestURI().split("/");
		
		if (parts.length != 3) {
			resp.sendError(HttpServletResponse.SC_BAD_REQUEST);
			return;
		}
		
		long facebookId;
		try {
			facebookId = Long.parseLong(parts[parts.length - 1]);
		} catch (NumberFormatException e) {
			resp.sendError(HttpServletResponse.SC_BAD_REQUEST);
			return;
		}
		
		if (facebookId != player.getFacebookId()) {
			resp.sendError(HttpServletResponse.SC_UNAUTHORIZED);
			return;
		}
		
		String accessToken = req.getParameter("access_token");
		if (accessToken == null)
			accessToken = player.getAccessToken();
		
		if (accessToken == null) {
			resp.sendError(HttpServletResponse.SC_UNAUTHORIZED);
			return;
		}*/
		
		PlayerController pc = PlayerController.getPlayerController();
		Player playerOut = pc.updatePlayer(player);
		if(playerOut == null) {
			resp.sendError(HttpServletResponse.SC_UNAUTHORIZED);
			return;
		}else{
			OutputStream out = resp.getOutputStream();
			String output = playerOut.toJSON().toString() + "\n";
			
			log.debug("POST response: " + output);
			out.write(output.getBytes());
			out.close();
		}
		/*Player p = pc.getPlayer(facebookId);
		
		if (p == null) {

			p = Player.parseFromJSON(FacebookController.getFacebookController().getMe(accessToken));
			
			if (p != null && p.getFacebookId() == facebookId) {
				pc.addNewPlayer(p);
				p.setAccessToken(accessToken);
			}
		} else if (p.getAccessToken() == null) {
			Player fbPlayer = Player.parseFromJSON(FacebookController.getFacebookController().getMe(accessToken));
			if (fbPlayer != null && fbPlayer.getFacebookId() == p.getFacebookId())
				p.setAccessToken(accessToken);
			else {
				resp.sendError(HttpServletResponse.SC_UNAUTHORIZED);
				return;
			}
		}*/
		
		/*if (p != null && p.getFacebookId() == player.getFacebookId()) {
			p.setPostRequired(false);
			
			p.mergeWithPlayer(player);
			
			pc.updatePlayer(p);
			
			OutputStream out = resp.getOutputStream();
			String output;
			if (player.isOutOfDate()) {
				log.debug("*** player data is out of date ***");
				output = p.toJSON().toString() + "\n";
			} else
				output = p.toJSONShort().toString() + "\n";
			log.debug("POST response: " + output);
			out.write(output.getBytes());
			out.close();

		} else {
			resp.sendError(HttpServletResponse.SC_FORBIDDEN);
			return;
		}*/

	}
	
}
