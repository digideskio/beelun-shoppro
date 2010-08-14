package com.beelun.shoppro.utils;

import java.net.MalformedURLException;
import java.net.URL;
import java.util.regex.Pattern;

import javax.servlet.http.HttpServletRequest;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;

public final class ServletUtils {
	private final static Log log = LogFactory.getLog(ServletUtils.class);
	
	/**
	 * Get the base url from request. It should work well in either the servlet deployed to ROOT or somewhere else
	 * 
	 * @param request
	 * @return
	 */
	public static String getBaseUrl(HttpServletRequest request) {		
		try {
			String baseUrl = null;
			// For well know protocol, ignore the port info
			if((request.getScheme().equalsIgnoreCase("http") && request.getServerPort() == 80) ||
					(request.getScheme().equalsIgnoreCase("https") && request.getServerPort() == 443)	) {
				URL reconstructedURL = new URL(request.getScheme(),
				        request.getServerName(),
				        request.getContextPath());	
				baseUrl = reconstructedURL.toString();
			} else {
				URL reconstructedURL = new URL(request.getScheme(),
				        request.getServerName(),
				        request.getServerPort(),
				        request.getContextPath());
				baseUrl = reconstructedURL.toString();
			}
			return baseUrl;
		} catch (MalformedURLException e) {
			log.error(e.getMessage());
			return null;
		}		
	}
	
	/**
	 * uri is in following format: 
	 *   /tab/%id%/descriptive-url-a-b-c
	 * We need get the %id% out
	 *   
	 * @param request
	 * @return the Id in the request
	 */
	public static Long getId(HttpServletRequest request) {
		Long ret = null;
		Pattern p = Pattern.compile("[/]+"); // Split by '/'
        String[] result = p.split(request.getRequestURI());
        if(result != null && result.length >= 3) {
        	try{
	        	String idString = result[2].substring(0, result[2].indexOf("-"));
	        	ret = Long.parseLong(idString);
        	} catch(Exception ex) {
        		log.error("fail to parse id: " + ex.getMessage());
        		ret = null;
        	}
        }
                
        return ret;
	}
}
