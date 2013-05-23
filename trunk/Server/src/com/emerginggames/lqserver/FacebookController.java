package com.emerginggames.lqserver;

import org.apache.log4j.Logger;
import org.json.JSONObject;
import org.apache.http.client.ResponseHandler;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.impl.client.BasicResponseHandler;
import org.apache.http.impl.client.DefaultHttpClient;


public class FacebookController {
	private final Logger log = Logger.getLogger(FacebookController.class);
	private static FacebookController instance = null;

	public synchronized static FacebookController getFacebookController() {
		if (instance == null)
			instance = new FacebookController();
		return instance;
	}

	public JSONObject getMe(String accessToken) {
		String request = "https://graph.facebook.com/me?fields=id,first_name,last_name,email&access_token=" + accessToken;
		HttpClient httpClient = new DefaultHttpClient();
		HttpGet httpget = new HttpGet(request);

		JSONObject res = null;
		try {
			// Create a response handler
			ResponseHandler<String> responseHandler = new BasicResponseHandler();
			String responseBody = httpClient.execute(httpget, responseHandler);
			res = new JSONObject(responseBody);
			if (!res.has("id"))
				res = null;
		} catch (Exception e) {
			// do nothing
			log.error("failed to get facebook profile for token " + accessToken);
		}
		finally {
			// When HttpClient instance is no longer needed,
			// shut down the connection manager to ensure
			// immediate deallocation of all system resources
			httpClient.getConnectionManager().shutdown();
		}


		return res;
	}
}
