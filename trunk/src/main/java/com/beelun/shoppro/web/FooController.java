package com.beelun.shoppro.web;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.servlet.ModelAndView;

@org.springframework.stereotype.Controller
public class FooController {
	private transient final Log log = LogFactory.getLog(FooController.class);
	
	@RequestMapping("/foo/*.html")
	public ModelAndView handleRequest(HttpServletRequest request, HttpServletResponse response) throws Exception {

		log.debug("url: " + request.getRequestURI());
		String viewName = request.getRequestURI().substring(0, request.getRequestURI().length()-5);
		log.debug("viewName: " + viewName);
		
		return new ModelAndView(viewName);
	}	
}
