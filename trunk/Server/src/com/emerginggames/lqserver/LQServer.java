package com.emerginggames.lqserver;

import java.net.InetAddress;
import java.net.InetSocketAddress;

import org.apache.log4j.Logger;
import org.apache.log4j.PropertyConfigurator;
import org.eclipse.jetty.server.Connector;
import org.eclipse.jetty.server.Server;
import org.eclipse.jetty.servlet.ServletContextHandler;
import org.eclipse.jetty.servlet.ServletHolder;
import org.eclipse.jetty.server.ssl.SslSelectChannelConnector;
import org.eclipse.jetty.util.ssl.SslContextFactory;
import org.eclipse.jetty.util.thread.QueuedThreadPool;

public class LQServer {
	private static Logger log = Logger.getLogger(LQServer.class);
	
    public static void main(String[] args) throws Exception
    {
    	PropertyConfigurator.configure("config/log4j.properties");
    	
    	//InetSocketAddress addr= new InetSocketAddress("192.168.1.129", 8080);
    	//Server server = new Server(addr);
    	
        Server server = new Server(8080);
    	
        log.debug("server configuration " + Configuration.getConfiguration().getJson().toString());
        
        /*SslSelectChannelConnector ssl_connector = new SslSelectChannelConnector();
        ssl_connector.setPort(8080);
        ssl_connector.setThreadPool(new QueuedThreadPool(20));
        SslContextFactory cf = ssl_connector.getSslContextFactory();
        cf.setKeyStorePath("ssl/lqserver.brandomania.tv.jks");
        cf.setKeyStorePassword("epsilonmu");
        cf.setKeyManagerPassword("epsilonmu");
        server.setConnectors(new Connector[]
        		          { ssl_connector });*/
        
        ServletContextHandler context = new ServletContextHandler(ServletContextHandler.SESSIONS);
        context.setContextPath("/");
        server.setHandler(context);
        
        context.addServlet(new ServletHolder(new PlayerServlet()), "/players/*");
        /*context.addServlet(new ServletHolder(new AdminServlet()), "/admin/*");
        context.addServlet(new ServletHolder(new FacebookAuthService()), "/fb_request/");
        context.addServlet(new ServletHolder(new FacebookPaymentServlet()), "/fb_payment/");
        context.addServlet(new ServletHolder(new CrossdomainServlet()), "/crossdomain.xml");*/
        
        server.start();
        server.join();
    }

	
}
