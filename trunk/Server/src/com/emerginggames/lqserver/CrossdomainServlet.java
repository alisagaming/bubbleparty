package com.emerginggames.lqserver;

import java.io.IOException;
import java.io.OutputStream;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

@SuppressWarnings("serial")
public class CrossdomainServlet extends HttpServlet {

	@Override
	protected void doGet(HttpServletRequest req, HttpServletResponse resp) 
			throws ServletException, IOException {
		resp.setContentType("application/xml");
		OutputStream out = resp.getOutputStream();
		String output = "<?xml version=\"1.0\"?> <!DOCTYPE cross-domain-policy SYSTEM \"/xml/dtds/cross-domain-policy.dtd\"><cross-domain-policy><site-control permitted-cross-domain-policies=\"all\"/><allow-http-request-headers-from domain=\"*\" headers=\"*\" secure=\"false\"/><allow-access-from domain=\"*\" to-ports=\"*\" secure=\"false\" /></cross-domain-policy>";
		out.write(output.getBytes());
		out.close();	}
}
