package com.beelun.shoppro.web;

import java.util.Date;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.GrantedAuthority;
import org.springframework.security.context.SecurityContextHolder;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.servlet.ModelAndView;
import org.springframework.web.servlet.mvc.Controller;

import com.beelun.shoppro.model.User;
import com.beelun.shoppro.service.UserManager;

/**
 * This is routing controller handling proper destination right after a successful login
 * 
 * @author bali
 *
 */
@org.springframework.stereotype.Controller
public class LoginSuccessController implements Controller {
	private transient final Log log = LogFactory.getLog(LoginSuccessController.class);
	private transient final String roleAdmin = "ROLE_ADMIN";
	
	@Autowired
	UserManager userManager;
	
	@Override
	@RequestMapping("/login-success.html")
	public ModelAndView handleRequest(HttpServletRequest request,
			HttpServletResponse response) throws Exception {
		
		// Get current user's role
		if(SecurityContextHolder.getContext() != null && SecurityContextHolder.getContext().getAuthentication() != null) {
			Object p = SecurityContextHolder.getContext().getAuthentication().getPrincipal();
			if(!(p instanceof String)) {
				User user = (User) p;
				if(user != null) {
					for(GrantedAuthority a: user.getAuthorities()) {
						// Set last login
						user.setLastLogin(new Date());
						userManager.saveUser(user, false);
						
						if(a.getAuthority().equals(roleAdmin)) {
							// Admin login
							log.debug("admin login, go to admin page.");
							response.sendRedirect("admin/index.html");
							return null;
						} else {
							// Customer login
						}
					}
				}
			}
		}
		
		// Logged in customer or un-logged in visitors will go here 
		log.debug("customer/visitor login, go to home page.");
		response.sendRedirect("index.html");
		
		return null;
	}

}
