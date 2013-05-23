package com.emerginggames.lqserver;

public class FacebookSignedRequest {

	private String algorithm;
	private Long expires;
	private Long issued_at;
	private String oauth_token;
	private Long user_id;
	private FacebookSignedRequestUser user;
	private FacebookSignedRequestCredits credits;

	public String getAlgorithm() {
		return algorithm;
	}

	public void setAlgorithm(String algorithm) {
		this.algorithm = algorithm;
	}

	public Long getExpires() {
		return expires;
	}

	public void setExpires(Long expires) {
		this.expires = expires;
	}

	public Long getIssued_at() {
		return issued_at;
	}

	public void setIssued_at(Long issued_at) {
		this.issued_at = issued_at;
	}

	public String getOauth_token() {
		return oauth_token;
	}

	public void setOauth_token(String oauth_token) {
		this.oauth_token = oauth_token;
	}

	public Long getUser_id() {
		return user_id;
	}

	public void setUser_id(Long user_id) {
		this.user_id = user_id;
	}

	public FacebookSignedRequestUser getUser() {
		return user;
	}

	public void setUser(FacebookSignedRequestUser user) {
		this.user = user;
	}

	public FacebookSignedRequestCredits getCredits() {
		return credits;
	}

	public void setCredits(FacebookSignedRequestCredits credits) {
		this.credits = credits;
	}

	public static class FacebookSignedRequestUser {
		private String country;
		private String locale;
		private FacebookSignedRequestUserAge age;

		public String getCountry() {
			return country;
		}

		public void setCountry(String country) {
			this.country = country;
		}

		public String getLocale() {
			return locale;
		}

		public void setLocale(String locale) {
			this.locale = locale;
		}

		public FacebookSignedRequestUserAge getAge() {
			return age;
		}

		public void setAge(FacebookSignedRequestUserAge age) {
			this.age = age;
		}

		public static class FacebookSignedRequestUserAge{
			private int min;
			private int max;

			public int getMin() {
				return min;
			}

			public void setMin(int min) {
				this.min = min;
			}

			public int getMax() {
				return max;
			}

			public void setMax(int max) {
				this.max = max;
			}
		}
	}
	
	public static class FacebookSignedRequestCredits {
		private Long buyer;
		private Long receiver;
		private Long order_id;
		private String order_info;
		private String status;
		private String order_details;
		
		public Long getOrder_id() {
			return order_id;
		}

		public void setOrder_id(Long order_id) {
			this.order_id = order_id;
		}

		public Long getReceiver() {
			return receiver;
		}

		public void setReceiver(Long receiver) {
			this.receiver = receiver;
		}

		public Long getBuyer() {
			return buyer;
		}

		public void setBuyer(Long buyer) {
			this.buyer = buyer;
		}

		public String getOrder_info() {
			return order_info;
		}

		public void setOrder_info(String order_info) {
			this.order_info = order_info;
		}

		public String getStatus() {
			return status;
		}

		public void setStatus(String status) {
			this.status = status;
		}

		public String getOrder_details() {
			return order_details;
		}

		public void setOrder_details(String order_details) {
			this.order_details = order_details;
		}
	}
}
