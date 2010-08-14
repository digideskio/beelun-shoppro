package com.beelun.shoppro.web;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

//import org.apache.commons.logging.Log;
//import org.apache.commons.logging.LogFactory;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.servlet.ModelAndView;

/**
 * Handle search request. Right now we use google custom search as search engine provider.
 * TODO: build our own search service.
 *  
 * @author bali
 *
 */
@org.springframework.stereotype.Controller
public class SearchController {
	// private final Log log = LogFactory.getLog(SearchController.class);
	
	/**
	 * Handle google CSE request
	 * 
	 * @param request
	 * @param response
	 * @return
	 * @throws Exception
	 */
	@RequestMapping("/google-cse-results.html")
	public ModelAndView handleCESRequest(HttpServletRequest request,
			HttpServletResponse response) throws Exception {
		return new ModelAndView("google-cse-results");
	}
}
