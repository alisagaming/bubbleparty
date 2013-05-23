package com.emerginggames.lqserver;

import java.io.IOException;
import java.io.PrintWriter;
import java.util.ArrayList;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.apache.commons.codec.binary.Base64;
import org.apache.log4j.Logger;
import org.codehaus.jackson.JsonGenerationException;
import org.codehaus.jackson.JsonParseException;
import org.codehaus.jackson.annotate.JsonMethod;
import org.codehaus.jackson.annotate.JsonAutoDetect.Visibility;
import org.codehaus.jackson.map.DeserializationConfig;
import org.codehaus.jackson.map.JsonMappingException;
import org.codehaus.jackson.map.ObjectMapper;
import org.junit.Test;

@SuppressWarnings("serial")
public class FacebookPaymentServlet extends HttpServlet {

	@SuppressWarnings("unused")
	private static Logger log = Logger.getLogger(FacebookPaymentServlet.class);

	public static class OrderInfo {
		private String item_id;

		public String getItem_id() {
			return item_id;
		}

		public void setItem_id(String item_id) {
			this.item_id = item_id;
		}
	}
	
	public static class OrderItem {
		private String item_id;
		private String title;
		private String description;
		private String image_url;
		private int price;
		public String getItem_id() {
			return item_id;
		}
		public void setItem_id(String item_id) {
			this.item_id = item_id;
		}
		public String getTitle() {
			return title;
		}
		public void setTitle(String title) {
			this.title = title;
		}
		public String getDescription() {
			return description;
		}
		public void setDescription(String description) {
			this.description = description;
		}
		public String getImage_url() {
			return image_url;
		}
		public void setImage_url(String image_url) {
			this.image_url = image_url;
		}
		public int getPrice() {
			return price;
		}
		public void setPrice(int price) {
			this.price = price;
		}
	}
	
	public static class OrderDetails {
		private Long order_id;
		private Long buyer;
		private Long receiver;
		private String currency;
		private int amount;
		private ArrayList<OrderItem> items;
		
		public Long getOrder_id() {
			return order_id;
		}
		public void setOrder_id(Long order_id) {
			this.order_id = order_id;
		}
		public Long getBuyer() {
			return buyer;
		}
		public void setBuyer(Long buyer) {
			this.buyer = buyer;
		}
		public Long getReceiver() {
			return receiver;
		}
		public void setReceiver(Long receiver) {
			this.receiver = receiver;
		}
		public String getCurrency() {
			return currency;
		}
		public void setCurrency(String currency) {
			this.currency = currency;
		}
		public int getAmount() {
			return amount;
		}
		public void setAmount(int amount) {
			this.amount = amount;
		}
		public ArrayList<OrderItem> getItems() {
			return items;
		}
		public void setItems(ArrayList<OrderItem> items) {
			this.items = items;
		}
	}
	
	public class PaymentsGetItemsResponse {
		private String method;
		private ArrayList<PaymentsGetItemsContent> content;
		
		public class PaymentsGetItemsContent {
			private String title;
			private String description;
			private int price;
			private String image_url;
			private String item_id;
			
			public String getTitle() {
				return title;
			}
			public void setTitle(String title) {
				this.title = title;
			}
			public String getDescription() {
				return description;
			}
			public void setDescription(String description) {
				this.description = description;
			}
			public int getPrice() {
				return price;
			}
			public void setPrice(int price) {
				this.price = price;
			}
			public String getImage_url() {
				return image_url;
			}
			public void setImage_url(String image_url) {
				this.image_url = image_url;
			}
			public String getItem_id() {
				return item_id;
			}
			public void setItem_id(String item_id) {
				this.item_id = item_id;
			}
		}
		
		public PaymentsGetItemsResponse(String title, String description, int price, String image_url, String item_id) {
			this.setMethod("payments_get_items");
			PaymentsGetItemsContent content = new PaymentsGetItemsContent();
			content.setTitle(title);
			content.setDescription(description);
			content.setPrice(price);
			content.setImage_url(image_url);
			content.setItem_id(item_id);
			this.setContent(new ArrayList<PaymentsGetItemsContent>());
			this.content.add(content);
		}

		public String getMethod() {
			return method;
		}

		public void setMethod(String method) {
			this.method = method;
		}

		public ArrayList<PaymentsGetItemsContent> getContent() {
			return content;
		}

		public void setContent(ArrayList<PaymentsGetItemsContent> content) {
			this.content = content;
		}
	}
	
	public class PaymentsStatusUpdateResponse {
		private String method;
		private PaymentsStatusUpdateContent content;
		public class PaymentsStatusUpdateContent {
			private String status;
			private Long order_id;
			public String getStatus() {
				return status;
			}
			public void setStatus(String status) {
				this.status = status;
			}
			public Long getOrder_id() {
				return order_id;
			}
			public void setOrder_id(Long order_id) {
				this.order_id = order_id;
			}
		}
		public PaymentsStatusUpdateResponse(String status, Long order_id) {
			this.setMethod("payments_status_update");
			this.setContent(new PaymentsStatusUpdateContent());
			this.content.setOrder_id(order_id);
			this.content.setStatus(status);
		}
		public PaymentsStatusUpdateContent getContent() {
			return content;
		}
		public void setContent(PaymentsStatusUpdateContent content) {
			this.content = content;
		}
		public String getMethod() {
			return method;
		}
		public void setMethod(String method) {
			this.method = method;
		}
	}
	
	
	@Test
	public void test1() {
		String signedRequest = "Cxx6eJf0XqeReY6S2Y8rHJjLGF0tYVnbZwtAgN7UgQg.eyJhbGdvcml0aG0iOiJITUFDLVNIQTI1NiIsImNyZWRpdHMiOnsiYnV5ZXIiOjEwMDAwMzQ0NzkxMDc5MiwicmVjZWl2ZXIiOjEwMDAwMzQ0NzkxMDc5Miwib3JkZXJfaWQiOjE2Mzg5MDE3MDQyMTg1NSwib3JkZXJfaW5mbyI6IntcIml0ZW1faWRcIjpcIjFcIn0iLCJ0ZXN0X21vZGUiOjF9LCJleHBpcmVzIjoxMzUxNTk0ODAwLCJpc3N1ZWRfYXQiOjEzNTE1OTA0NTgsIm9hdXRoX3Rva2VuIjoiQUFBRFpCMEpVaVpCUE1CQURLTzNkdWZjRk5nWDhXUDNaQTI1eGlCdHlORkpoU1FONDhkZXU0Y1lZeXJBejRJb0RmT0hxZURXSFVNT3phVXBMa3FGOUE2U3o1VmhOcWZCNkNrcXN0TlRwZUtMNDFzYnVmeUUiLCJ1c2VyIjp7ImNvdW50cnkiOiJydSIsImxvY2FsZSI6ImVuX1VTIiwiYWdlIjp7Im1pbiI6MjF9fSwidXNlcl9pZCI6IjEwMDAwMzQ0NzkxMDc5MiJ9";
		String payLoad = signedRequest.split("[.]", 2)[1];
		payLoad = payLoad.replace("-", "+").replace("_", "/").trim();
		System.out.println(new String(Base64.decodeBase64(payLoad)));
		try {
			FacebookSignedRequest facebookSignedRequest = FacebookAuthService.getFacebookSignedRequest(signedRequest);
			//System.out.println("order_info " +facebookSignedRequest.getCredits().getOrder_info() );
			System.out.println( new ObjectMapper().writeValueAsString(facebookSignedRequest));
		} catch (Exception e) {
			// TODO Auto-generated catch block
			System.out.println("exception " + e.getMessage());
			e.printStackTrace();
		}
		
		//String string = "{"algorithm":"HMAC-SHA256","credits":{"buyer":100003447910792,"receiver":100003447910792,"order_id":297093967071621,"order_info":"{\"item_id\":\"1\"}","test_mode":1},"expires":1351594800,"issued_at":1351590243,"oauth_token":"AAADZB0JUiZBPMBADKO3dufcFNgX8WP3ZA25xiBtyNFJhSQN48deu4cYYyrAz4IoDfOHqeDWHUMOzaUpLkqF9A6Sz5VhNqfB6CkqstNTpeKL41sbufyE","user":{"country":"ru","locale":"en_US","age":{"min":21}},"user_id":"100003447910792"}";
		//FacebookSignedRequest facebookSignedRequest = new ObjectMapper().readValue(jsonString, FacebookSignedRequest.class);;
	}
	
	@Test
	public void test2() {
		String signedRequest = "AQssfZauuLyCeLSRuGAysR4mP5DpJJ3CyQwHlqlBNR8.eyJhbGdvcml0aG0iOiJITUFDLVNIQTI1NiIsImNyZWRpdHMiOnsib3JkZXJfZGV0YWlscyI6IntcIm9yZGVyX2lkXCI6MzY3Mzg2MDY2Njc4NjY0LFwiYnV5ZXJcIjoxMDAwMDM0NDc5MTA3OTIsXCJhcHBcIjoyODAxNzE4MDg3NDk4MTEsXCJyZWNlaXZlclwiOjEwMDAwMzQ0NzkxMDc5MixcImN1cnJlbmN5XCI6XCJGQlpcIixcImFtb3VudFwiOjIsXCJ0aW1lX3BsYWNlZFwiOjEzNTE1OTQyMzAsXCJwcm9wZXJ0aWVzXCI6MSxcInVwZGF0ZV90aW1lXCI6MTM1MTU5NDIzNCxcImRhdGFcIjpcIlwiLFwicmVmX29yZGVyX2lkXCI6MCxcIml0ZW1zXCI6W3tcIml0ZW1faWRcIjpcIjBcIixcInRpdGxlXCI6XCIxMDAgY29pbnNcIixcImRlc2NyaXB0aW9uXCI6XCIxMDAgZ2FtZSBjb2luc1wiLFwiaW1hZ2VfdXJsXCI6XCJodHRwczpcXFwvXFxcL2VtZXJnaW5nZ2FtZXMuczMuYW1hem9uYXdzLmNvbVxcXC9zd2ZcXFwvaW1hZ2VzXFxcL2ludGVyZmFjZVxcXC9jb2luc19pY29uXzEwMC5wbmdcIixcInByb2R1Y3RfdXJsXCI6XCJcIixcInByaWNlXCI6MixcImRhdGFcIjpcIlwifV0sXCJzdGF0dXNcIjpcInBsYWNlZFwifSIsInNvdXJjZSI6bnVsbCwic3RhdHVzIjoicGxhY2VkIiwib3JkZXJfaWQiOjM2NzM4NjA2NjY3ODY2NCwidGVzdF9tb2RlIjoxfSwiZXhwaXJlcyI6MTM1MTU5ODQwMCwiaXNzdWVkX2F0IjoxMzUxNTk0MjM0LCJvYXV0aF90b2tlbiI6IkFBQURaQjBKVWlaQlBNQkFQVm44Znhhd0R6RGRVZHZWV0o4S1pBRFZmWkJJWERaQnF2VjN4c1pBZm9ZcVJaQVJZMGxmZWNGQk54SUVVakdMT0FPOUlWQ1NRWDRqdFJybUt5UFJ6VkdCWEtaQzZONmtYd2hoUGJ2WkNKIiwidXNlciI6eyJjb3VudHJ5IjoicnUiLCJsb2NhbGUiOiJlbl9VUyIsImFnZSI6eyJtaW4iOjIxfX0sInVzZXJfaWQiOiIxMDAwMDM0NDc5MTA3OTIifQ";
		try {
			FacebookSignedRequest facebookSignedRequest = FacebookAuthService.getFacebookSignedRequest(signedRequest);
			ObjectMapper mapper = new ObjectMapper().setVisibility(JsonMethod.FIELD, Visibility.ANY);
			mapper.configure(DeserializationConfig.Feature.FAIL_ON_UNKNOWN_PROPERTIES, false);
			OrderDetails details = mapper.readValue(facebookSignedRequest.getCredits().getOrder_details(), OrderDetails.class);
			
			System.out.println( mapper.writeValueAsString(details));
		} catch (Exception e) {
			// TODO Auto-generated catch block
			System.out.println("exception " + e.getMessage());
			e.printStackTrace();
		}
	}
	
	@Test
	public void test3() {
		String info = "{\"item_id\":\"1\"}";
		try {
			OrderInfo order_info = new ObjectMapper().readValue(info, OrderInfo.class);
			System.out.println(order_info.getItem_id());
		} catch (JsonParseException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (JsonMappingException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
	
	@Override
	protected void doPost(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {
		/*String signedRequest = (String) request.getParameter("signed_request");
		String method = (String) request.getParameter("method");
		log.debug("signed_request = " + signedRequest);
		log.debug("FacebookPaymentServlet:method="+method);

		
		String payLoad = signedRequest.split("[.]", 2)[1];
		payLoad = payLoad.replace("-", "+").replace("_", "/").trim();
		String jsonString = new String(Base64.decodeBase64(payLoad));
		log.debug(jsonString);

		FacebookSignedRequest facebookSignedRequest = FacebookAuthService.getFacebookSignedRequest(signedRequest);
		PrintWriter writer = response.getWriter();
		
		if (method == null) {
			response.sendError(HttpServletResponse.SC_BAD_REQUEST);
		}
		
		
		if (method.equals("payments_get_items")) {
			//JSONObject orderInfoJson = new JSONObject();
			
			ObjectMapper mapper = new ObjectMapper();
			log.debug("order_info = " + facebookSignedRequest.getCredits().getOrder_info());
			OrderInfo orderInfo = mapper.readValue(facebookSignedRequest.getCredits().getOrder_info(), OrderInfo.class);
			String itemId = orderInfo.getItem_id();
			log.debug("itemId = " + itemId);
			if ("1".equals(itemId) || "com.emerginggames.brandomania.100coins".equals(itemId)) {
				PaymentsGetItemsResponse resp = new PaymentsGetItemsResponse("100 coins", "100 game coins", 20, "https://emerginggames.s3.amazonaws.com/swf/images/interface/coins_icon_100.png", itemId);
				mapper.writeValue(writer, resp);
			} else if ("2".equals(itemId) || "com.emerginggames.brandomania.300coins".equals(itemId)) {
				PaymentsGetItemsResponse resp = new PaymentsGetItemsResponse("300 coins", "300 game coins", 50, "https://emerginggames.s3.amazonaws.com/swf/images/interface/coins_icon_300.png", itemId);
				mapper.writeValue(writer, resp);
			} else if ("3".equals(itemId) || "com.emerginggames.brandomania.1000coins".equals(itemId)) {
				PaymentsGetItemsResponse resp = new PaymentsGetItemsResponse("1000 coins", "1000 game coins", 100, "https://emerginggames.s3.amazonaws.com/swf/images/interface/coins_icon_1000.png", itemId);
				mapper.writeValue(writer, resp);
			} else if ("4".equals(itemId) || "com.emerginggames.brandomania.3500coins".equals(itemId)) {
				PaymentsGetItemsResponse resp = new PaymentsGetItemsResponse("3500 coins", "3500 game coins", 250, "https://emerginggames.s3.amazonaws.com/swf/images/interface/coins_icon_3500.png", itemId);
				mapper.writeValue(writer, resp);
			}
			
			writer.close();
			
		} else if (method.equals("payments_status_update")) {
		
			ObjectMapper mapper = new ObjectMapper().setVisibility(JsonMethod.FIELD, Visibility.ANY);
			mapper.configure(DeserializationConfig.Feature.FAIL_ON_UNKNOWN_PROPERTIES, false);
			OrderDetails details = mapper.readValue(facebookSignedRequest.getCredits().getOrder_details(), OrderDetails.class);
			
			String status = facebookSignedRequest.getCredits().getStatus();
			
			if ("placed".equals(status) ) {
				PlayerController pc = PlayerController.getPlayerController(); 
				Player p = pc.getPlayer(details.getReceiver().longValue());
				
				if (p != null) {
					synchronized(p) {
						int coinsToAdd = 0;
						for (OrderItem item: details.getItems()) {
							if ("1".equals(item.item_id) || "com.emerginggames.brandomania.100coins".equals(item.item_id)) {
								coinsToAdd += 100;
							} else if ("2".equals(item.item_id) || "com.emerginggames.brandomania.300coins".equals(item.item_id)) {
								coinsToAdd += 300;
							} else if ("3".equals(item.item_id) || "com.emerginggames.brandomania.1000coins".equals(item.item_id)) {
								coinsToAdd += 1000;
							} else if ("4".equals(item.item_id) || "com.emerginggames.brandomania.3500coins".equals(item.item_id)) {
								coinsToAdd += 3500;
							}
						}
						//p.setCoinsTotal(p.getCoinsTotal() + coinsToAdd);
						//p.setRandomUpdateCode();
						pc.updatePlayer(p);
					}
				}

				PaymentsStatusUpdateResponse resp = new PaymentsStatusUpdateResponse("settled", details.getOrder_id());
				mapper.writeValue(writer, resp);
			} else if ("settled".equals(status)) {
				PaymentsStatusUpdateResponse resp = new PaymentsStatusUpdateResponse("settled", details.getOrder_id());
				mapper.writeValue(writer, resp);
			}
			writer.close();
			
		} else {
			log.debug("unknown method "+method);
			writer.print("{}");
			writer.close();
		}*/
	}
}
