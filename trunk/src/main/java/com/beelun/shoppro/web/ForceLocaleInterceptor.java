package com.beelun.shoppro.web;

import java.util.Locale;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;
import org.springframework.web.servlet.LocaleResolver;
import org.springframework.web.servlet.handler.HandlerInterceptorAdapter;
import org.springframework.web.servlet.support.RequestContextUtils;

import com.beelun.shoppro.model.type.SiteTypeEnum;
import com.beelun.shoppro.service.MyGlobManager;

/**
 * Set locale according to my DB settings Refer to:
 * http://static.springsource.org
 * /spring/docs/2.5.x/reference/mvc.html#mvc-localeresolver-interceptor
 * 
 * @author bali
 * 
 */
@Component("forceLocaleInterceptor")
public class ForceLocaleInterceptor extends HandlerInterceptorAdapter {
	@Autowired
	MyGlobManager myGlobManager;

	/**
	 * Test: set locale to en_US always.
	 */
	@Override
	public boolean preHandle(HttpServletRequest request, HttpServletResponse response, Object handler) {
		LocaleResolver localeResolver = RequestContextUtils
				.getLocaleResolver(request);
		if (null != localeResolver) {
			// localeResolver.setLocale(request, response, new Locale("en",
			// "US"));
			SiteTypeEnum currentSiteType = myGlobManager.fetch().getSiteType();
			if (currentSiteType == SiteTypeEnum.US) {
				localeResolver.setLocale(request, response, new Locale("en", "US"));
			} else {
				localeResolver.setLocale(request, response, new Locale("zh", "CN"));
			}
		}
		return true;
	}
}
