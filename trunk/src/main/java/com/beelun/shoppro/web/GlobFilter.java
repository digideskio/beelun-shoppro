package com.beelun.shoppro.web;

import java.io.IOException;

import javax.servlet.Filter;
import javax.servlet.FilterChain;
import javax.servlet.FilterConfig;
import javax.servlet.ServletException;
import javax.servlet.ServletRequest;
import javax.servlet.ServletResponse;
import javax.servlet.http.HttpServletRequest;

import org.apache.commons.lang.StringUtils;
//import org.apache.commons.logging.Log;
//import org.apache.commons.logging.LogFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import com.beelun.shoppro.SuperContainer;
import com.beelun.shoppro.service.MyGlobManager;
import com.beelun.shoppro.utils.ServletUtils;

/**
 * The filter to fill global variables to the request. TODO: every request will
 * call this, do some perf optimization?
 * 
 * @author Bill Li(bill@beelun.com)
 * 
 */
@Component("globFilter")
public class GlobFilter implements Filter {
	// private transient final Log log = LogFactory.getLog(GlobFilter.class);

	@Autowired
	MyGlobManager myGlobManager;

	@Override
	public void destroy() {

	}

	@Override
	public void doFilter(ServletRequest req, ServletResponse res,
			FilterChain chain) throws IOException, ServletException {
		HttpServletRequest request = (HttpServletRequest) req;
		
		// Get base url
		if(null == SuperContainer.BaseUrl) {
			SuperContainer.BaseUrl = ServletUtils.getBaseUrl(request);
		}
		
		// Set 'glob'
		if (null == request.getAttribute("glob")) {
			// log.info("reset glob...");
			request.setAttribute("glob", myGlobManager.fetch());
		} else {
			// log.info("glob exists, skip...");
		}

		// Set username
		String currentUser = request.getRemoteUser();
		if (!StringUtils.isBlank(currentUser)) {
			request.setAttribute("currentUser", currentUser);
		}

		// Set browser
		if (null == request.getAttribute("currentBrowser")) {
			String currentBrowser = "MSIE";
			String tBrowser = ((HttpServletRequest) req)
					.getHeader("user-agent");
			// log.info("tBrowser: " + tBrowser);
			// If there is no 'user-agent', it might not a browser at all,
			// but we treat it as IE for now
			if (!StringUtils.isBlank(tBrowser)) {
				if (!(tBrowser.indexOf("MSIE") > -1)) {
					currentBrowser = "NON_MSIE";
				}
			}
			request.setAttribute("currentBrowser", currentBrowser);
		}

		chain.doFilter(req, res);
	}

	@Override
	public void init(FilterConfig filterConfig) throws ServletException {
	}
}
