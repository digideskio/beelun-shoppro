package com.beelun.shoppro.web;

import java.io.IOException;

import javax.servlet.Filter;
import javax.servlet.FilterChain;
import javax.servlet.FilterConfig;
import javax.servlet.ServletException;
import javax.servlet.ServletRequest;
import javax.servlet.ServletResponse;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.stereotype.Component;

import com.octo.captcha.module.servlet.image.SimpleImageCaptchaServlet;

/**
 * Filter for captach: To verify that the user inputs the correct words
 * 
 * @author bali
 * 
 */
@Component("captchaFilter")
public class CaptchaFilter implements Filter {
	private final Log log = LogFactory.getLog(CaptchaFilter.class);
	final String JCAPTCHA_STRING = "jcaptcha";
	final String URL_ON_FAILURE = "urlOnCaptchaFailure";

	@Override
	public void destroy() {
	}

	@Override
	public void doFilter(ServletRequest request, ServletResponse response, FilterChain chain) throws IOException, ServletException {
		String userCaptchaResponse = request.getParameter(JCAPTCHA_STRING);
		String urlOnCaptchaFailure = request.getParameter(URL_ON_FAILURE);

		// If null, which means there is no such parameter and then we will skip
		// the verification
		if (null != urlOnCaptchaFailure) {

			boolean captchaPassed = SimpleImageCaptchaServlet.validateResponse((HttpServletRequest) request, userCaptchaResponse);

			if (captchaPassed) {
				log.debug("captcha is verified ok.");
				chain.doFilter(request, response);
			} else {
				// Set response as error and return
				log.warn("fail to captcha");
				((HttpServletResponse) response).sendRedirect(urlOnCaptchaFailure);
			}
		} else {
			chain.doFilter(request, response);
		}
	}

	@Override
	public void init(FilterConfig filterConfig) throws ServletException {
	}

}
