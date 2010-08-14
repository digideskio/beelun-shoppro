package com.beelun.shoppro.web.listener;

import javax.servlet.ServletContext;
import javax.servlet.ServletContextEvent;
import javax.servlet.ServletContextListener;

import org.apache.log4j.Logger;

import com.beelun.shoppro.SuperContainer;

/**
 * This kind of listener will be running during servlet startup if enabled. 
 * Currently this listener is abondoned.
 * 
 * @author bill
 *
 */
public class StartupListener implements ServletContextListener {
	private static final Logger log = Logger.getLogger(StartupListener.class);

	public void contextDestroyed(ServletContextEvent event) {
		// do nothing.
	}

	public void contextInitialized(ServletContextEvent event) {
		log.debug("context init...");
		ServletContext context = event.getServletContext();
		final String baseDir = context.getRealPath("/WEB-INF/web.xml");
		if(baseDir == null)
		{
			log.error("Fail to get base web app dir. Probably you are running this with a war. If so, expand it manually and retry.");
		} else
		{
			SuperContainer.BaseDir = baseDir.substring(0, baseDir.length() - "/WEB-INF/web.xml".length());
			log.debug("context init done. BaseDir=" + SuperContainer.BaseDir);
		}
	}
}
