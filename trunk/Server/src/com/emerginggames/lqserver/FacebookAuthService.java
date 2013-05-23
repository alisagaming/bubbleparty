package com.emerginggames.lqserver;

import java.io.IOException;
import java.io.PrintWriter;
import java.io.UnsupportedEncodingException;
import java.net.URLEncoder;
import java.util.ArrayList;
import java.util.List;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import org.apache.commons.codec.binary.Base64;

import org.apache.http.NameValuePair;
import org.apache.http.client.HttpClient;
import org.apache.http.client.ResponseHandler;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.impl.client.BasicResponseHandler;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.message.BasicNameValuePair;
import org.apache.log4j.Logger;
import org.json.JSONObject;
import org.junit.Test;

import org.codehaus.jackson.JsonParseException;
import org.codehaus.jackson.annotate.JsonAutoDetect.Visibility;
import org.codehaus.jackson.annotate.JsonMethod;
import org.codehaus.jackson.map.DeserializationConfig;
import org.codehaus.jackson.map.JsonMappingException;
import org.codehaus.jackson.map.ObjectMapper;

@SuppressWarnings("serial")
public class FacebookAuthService extends HttpServlet  {


	@SuppressWarnings("unused")
	private static Logger log = Logger.getLogger(FacebookAuthService.class);

	private static final String apiKey = "280171808749811";

	private static final String appSecret = "b9925854c439a1d09cc78c7bc50c2744";

	private static final String appId = "280171808749811";
	
	private static final String swfPath = "https://emerginggames.s3.amazonaws.com/swf/Brandomania.swf";
	private static final String dataUrl = "https://emerginggames.s3.amazonaws.com/swf/";



	private static final String redirect_uri = "https://apps.facebook.com/brandomania/";



	private static final String[] perms = new String[] {"publish_actions", "email"};



	public static String getAPIKey() {

		return apiKey;

	}



	public static String getSecret() {

		return appSecret;

	}


	public static String joinStrings(String[] array) {
		String result = "";
		for (int i = 0; i < array.length; i++) {
			result += array[i] + ((i == array.length - 1) ? "" : ",");
		}
		
		return result;
	} 

	public static String getLoginRedirectURL() {

		return "https://graph.facebook.com/oauth/authorize?client_id=" + appId

				+ "&display=page&redirect_uri=" + redirect_uri + "&scope="

            + joinStrings(perms);

	}
	
	@Test
	public void test_ExtendAccessToken() {
		System.out.println(getExtendedAccessToken("AAADZB0JUiZBPMBADkoIBqFsWtZBbvu0nZBlFpymn79MYYt0Sg2rzPDajGWMQi21snb2mcAyQhLeiZBMJZBZBwluFOrmDSGtsVYOyglXCQiBPgZDZD"));
	} 
	
	public static String getExtendedAccessToken(String accessToken) {
		String newAccessToken = null;
		String request = "https://graph.facebook.com/oauth/access_token";
		
		HttpClient httpClient = new DefaultHttpClient();
		List<NameValuePair> pairs = new ArrayList<NameValuePair>();
		pairs.add(new BasicNameValuePair("grant_type", "fb_exchange_token"));
		pairs.add(new BasicNameValuePair("fb_exchange_token", accessToken));
		pairs.add(new BasicNameValuePair("client_id", appId));
		pairs.add(new BasicNameValuePair("client_secret", appSecret));
		HttpPost httpPost = new HttpPost(request);
		try {
			httpPost.setEntity(new UrlEncodedFormEntity(pairs));
		} catch (UnsupportedEncodingException e1) {
			return accessToken;
		}

		try {
			// Create a response handler
			ResponseHandler<String> responseHandler = new BasicResponseHandler();
			String response = httpClient.execute(httpPost, responseHandler);
			String[] params = response.split("&");
			for (String param: params) {
				String key = param.split("=")[0];
				String value = param.split("=")[1];
				if (key.equals("access_token"))
					newAccessToken = value;
			}
		} catch (Exception e) {
			return accessToken;
		}
		finally {
			httpClient.getConnectionManager().shutdown();
		}


		return newAccessToken;

	}



	public static String getAuthURL(String authCode) {

		return "https://graph.facebook.com/oauth/access_token?client_id="

            + appId + "&redirect_uri=" + redirect_uri + "&client_secret="

            + appSecret + "&code=" + authCode;

	}



	public static String getAuthURL() {
		try {
			return "https://www.facebook.com/dialog/oauth?client_id="
			    + appId + "&redirect_uri=" + URLEncoder.encode(redirect_uri, "UTF-8") + "&scope="
			    + joinStrings(perms);
		} catch (UnsupportedEncodingException e) {
			return null;
		}
	}
	
	@Test
	public void test1() throws JsonParseException, JsonMappingException, IOException {
		String signedRequest = "qkBGnc5NoVa-ItUDjJY0eOhh6QIeq7iuGYrP-v9ebUs.eyJhbGdvcml0aG0iOiJITUFDLVNIQTI1NiIsImV4cGlyZXMiOjEzNTE0NDM2MDAsImlzc3VlZF9hdCI6MTM1MTQzODIyNCwib2F1dGhfdG9rZW4iOiJBQUFEWkIwSlVpWkJQTUJBQWJmdlVFeWFmbkswMWd3eWZFQTRrQkdPRHhZdXlzd0JKWkJ3UlhtQWNpQkRaQ21TUTBKbkFtc0RYQU4zWkFWUXJzYnl2WkNNek94dHFiOGNOZVpDOEdYVWUxUVhGbHk4WFJ0Vlc3aFpCIiwidXNlciI6eyJjb3VudHJ5IjoicnUiLCJsb2NhbGUiOiJlbl9VUyIsImFnZSI6eyJtaW4iOjIxfX0sInVzZXJfaWQiOiIxMDAwMDM0NDc5MTA3OTIifQ";
		FacebookSignedRequest fb = getFacebookSignedRequest(signedRequest);
		System.out.println(fb.getUser_id());
	}

	public static FacebookSignedRequest getFacebookSignedRequest(String signedRequest) throws JsonParseException, JsonMappingException, IOException {
		String payLoad = signedRequest.split("[.]", 2)[1];
		payLoad = payLoad.replace("-", "+").replace("_", "/").trim();
		String jsonString = new String(Base64.decodeBase64(payLoad));
		ObjectMapper mapper = new ObjectMapper().setVisibility(JsonMethod.FIELD, Visibility.ANY);
		mapper.configure(DeserializationConfig.Feature.FAIL_ON_UNKNOWN_PROPERTIES, false);
		return mapper.readValue(jsonString, FacebookSignedRequest.class);

	}
	
	@Override
	protected void doPost(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {
		String signedRequest = (String) request.getParameter("signed_request");
		log.debug("signed_request = " + signedRequest);
		FacebookSignedRequest facebookSignedRequest = FacebookAuthService.getFacebookSignedRequest(signedRequest);
		PrintWriter writer = response.getWriter();
		if (facebookSignedRequest.getOauth_token() == null) {

			response.setContentType("text/html");

			writer.print("<script> top.location.href='"   + FacebookAuthService.getAuthURL() + "'</script>");

			writer.close();

		} else {

			response.setContentType("text/html");
			
			// extend the access token
			facebookSignedRequest.setOauth_token(getExtendedAccessToken(facebookSignedRequest.getOauth_token()));

			//request.setAttribute("accessToken", facebookSignedRequest.getOauth_token());
			String query = request.getQueryString();
			if (query == null)
				query = "";
			
			query += (query.isEmpty() ? "" : "&") + "post=1&user_id=" +  facebookSignedRequest.getUser_id() + 
					"&access_token=" + facebookSignedRequest.getOauth_token() +
					"&data_url=" + dataUrl;
			log.debug("query = " + query);
			
			writer.print(genPage(query, facebookSignedRequest.getOauth_token()));
			//writer.print(testPage());
			writer.close();
			
			//RequestDispatcher requestDispatcher = getServletContext().getRequestDispatcher("/YOUR_NEXT_PATH");

			//requestDispatcher.forward(request, response);

		}

	}
	
	@SuppressWarnings("unused")
	private static String testPage() {
		return 
				"<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">\n"+
				"<html xmlns=\"http://www.w3.org/1999/xhtml\" lang=\"en\" xml:lang=\"en\">\n" +
				"	<head>\n" +
				"		<title>Brandomania</title>\n" +
				"		<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\n" +
				"		<style type=\"text/css\" media=\"screen\">\n" +
				"		html, body { height:100%; background-color: #ffffff;}\n" +
				"		body { margin:0; padding:0; overflow:hidden; }\n" +
				"		#flashContent { width:100%; height:100%; }\n" +
				"		</style>\n" +
				"	</head>\n" +
				"<body>\n" +
				"		<object type=\"application/x-shockwave-flash\" data=\"" + swfPath + "\" id=\"Brandomania\" allowFullScreen=\"false\" width=\"760\" height=\"950\">\n" +
				"</body>\n" +
				"</html>\n";
	}
	
	private static String genPage(String query, String accessToken) {
		return
				"<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">\n"+
				"<html xmlns=\"http://www.w3.org/1999/xhtml\" lang=\"en\" xml:lang=\"en\">\n" +
				"	<head>\n" +
				"		<title>Brandomania</title>\n" +
				"		<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\n" +
				"		<style type=\"text/css\" media=\"screen\">\n" +
				"		html, body { height:100%; background-color: #ffffff;}\n" +
				"		body { margin:0; padding:0; overflow:hidden; }\n" +
				"		#flashContent { width:100%; height:100%; }\n" +
				"		</style>\n" +
				"	</head>\n" +
				"	<body>\n" +
				"		<div id=\"fb-root\"></div>\n" +
				"		<script src=\"https://connect.facebook.net/en_US/all.js\"></script>\n" +
				"		<script language=\"JavaScript\">\n" +
				"			FB.init({appId:'" + appId + "',status:true,frictionlessRequests:true,cookie:true,oauth:true});\n" +
				"		</script>\n" +
				"	<div id=\"flashContent\" style=\"background-size: 100%;\">\n" +
				"		<img id=\"ie_fix\" width=\"760\" height=\"950\" style=\"display:none\"/>\n" +
				"		<object classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\" width=\"760\" height=\"950\" id=\"Brandomania_M\" align=\"middle\">\n" +
				"			<param name=\"movie\" value=\"" + swfPath + "\" />\n" +
				"			<param name=\"quality\" value=\"high\" />\n" +
				"			<param name=\"bgcolor\" value=\"#ffffff\" />\n"+
				"			<param name=\"play\" value=\"true\" />\n" +
				"			<param name=\"loop\" value=\"true\" />\n" +
				"			<param name=\"wmode\" value=\"opaque\" />\n" +
				"			<param name=\"scale\" value=\"showall\" />\n" +
				"			<param name=\"menu\" value=\"true\" />\n" +
				"			<param name=\"allowFullScreen\" value=\"false\" />\n" +
				"			<param name=\"devicefont\" value=\"false\" />\n" +
				"			<param name=\"salign\" value=\"\" />\n" +
				"			<param name=\"allowScriptAccess\" value=\"always\" />\n" +
				"			<param name=\"FlashVars\" value=\"" + query + "\"/>\n" +
				"			<!--[if !IE]>-->\n" +
				"			<object type=\"application/x-shockwave-flash\" data=\"" + swfPath + "\" FlashVars=\"" + query + "\" id=\"Brandomania\" allowFullScreen=\"false\" width=\"760\" height=\"950\">\n" +
				"				<param name=\"movie\" value=\"" + swfPath + "\" />\n" +
				"				<param name=\"quality\" value=\"high\" />\n" +
				"				<param name=\"bgcolor\" value=\"#ffffff\" />\n" +
				"				<param name=\"play\" value=\"true\" />\n" +
				"				<param name=\"loop\" value=\"true\" />\n" +
				"				<param name=\"wmode\" value=\"opaque\" />\n" +
				"				<param name=\"scale\" value=\"showall\" />\n" +
				"				<param name=\"menu\" value=\"true\" />\n" +
				"				<param name=\"devicefont\" value=\"false\" />\n" +
				"				<param name=\"salign\" value=\"\" />\n" +
				"				<param name=\"allowFullScreen\" value=\"false\" />\n" +
				"				<param name=\"allowScriptAccess\" value=\"always\" />\n" +
				"			<!--<![endif]-->\n" +
				"				<a href=\"http://www.adobe.com/go/getflash\">\n" +
				"					<img src=\"http://www.adobe.com/images/shared/download_buttons/get_flash_player.gif\" alt=\"Get Adobe Flash player\" />\n" +
				"				</a>\n" +
				"			<!--[if !IE]>-->\n" +
				"			</object>\n" +
				"			<!--<![endif]-->\n" +
				"		</object>\n" +
				"	</div>\n" +
				"	<script language=\"JavaScript\">\n" +		
				"		function getJS(){\n" +
				"			if (navigator.appName.indexOf(\"Microsoft\")!=-1)\n" +
				"				return window[\"Brandomania_M\"];\n" +
				"			else\n" +
				"			if (window[\"Brandomania\"]!=undefined)\n" +
				"				return window[\"Brandomania\"];\n"+
				"			else\n" +
				"				return document[\"Brandomania\"];\n" +
				"		}\n" +
				"\n" +		
				"		function fbAPI(func,call,params){\n" +
				"			FB.api(func,call,params,function(response){ getJS().JSCallback(func,'call',response); } );\n" +
				"		}\n" +
				"\n" +		
				"		function fbUI(params){\n" +
				"			FB.ui(params, function(response){ getJS().JSCallback('ui','call',response); } );\n"+
				"		}\n" +
				"\n" +
				"		function open_url(url){\n" +
				"			window.open(url, '_blank');\n"+
				"			window.focus();\n"+
				"		}\n"+
				"\n" +		
				"		function get_data(){\n" +
				"			FB.getLoginStatus(function(response){\n"+
				"				alert(response);\n"+
				"				if (response.status==='connected'){\n"+
				"					getJS().JSCallback('fb','init',response.authResponse);\n"+
				"				}\n" + 
				"				else\n" +
				"				if (response.status==='not_authorized'){\n" +
				"					FB.login(function(response){\n" +
				"						if (response.authResponse){\n" +
				"							getJS().JSCallback('fb','init',response.authResponse);\n" + 
				"						}\n" + 
				"						else{\n" +
				"						}\n" +
				"					});\n"+
				"				}\n"+
				"				else{\n" + 
				"					var auth_url=\"" + getAuthURL() + "\";\n" +
				"					top.location.href=auth_url;\n" +
				"				}\n" +
				"			},true);\n" +
				"		}\n" +
				"\n" +
				"</script>\n" +
				"</body>\n" +
				"</html>\n";
	}


}
