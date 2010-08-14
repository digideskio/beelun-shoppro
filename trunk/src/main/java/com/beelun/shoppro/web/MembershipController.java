package com.beelun.shoppro.web;

import java.util.HashMap;
import java.util.Map;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.apache.commons.lang.RandomStringUtils;
import org.apache.commons.lang.StringUtils;
import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.mail.SimpleMailMessage;
import org.springframework.security.context.SecurityContextHolder;
import org.springframework.security.providers.UsernamePasswordAuthenticationToken;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.servlet.ModelAndView;
import org.springframework.web.servlet.support.RequestContextUtils;

import com.beelun.shoppro.model.User;
import com.beelun.shoppro.service.MailEngine;
import com.beelun.shoppro.service.MyGlobManager;
import com.beelun.shoppro.service.UserManager;
import com.beelun.shoppro.utils.ServletUtils;

@org.springframework.stereotype.Controller
public class MembershipController {
	private final Log log = LogFactory.getLog(MembershipController.class);

	@Autowired
	UserManager userManager;

	@Autowired
	MailEngine mailEngine;

	@Autowired
	MyGlobManager myGlobManager;

	@RequestMapping("/membership/login.*")
	public ModelAndView handleLoginRequest(HttpServletRequest request, HttpServletResponse response) throws Exception {
		log.debug("in handleLoginRequest...");

		return new ModelAndView("/membership/login");
	}

	// @RequestMapping("/membership/admin-login.*")
	// public ModelAndView handleAdminLoginRequest(HttpServletRequest request,
	// HttpServletResponse response) throws Exception {
	// log.debug("in handleAdminLoginRequest...");
	//
	// return new ModelAndView("/membership/admin-login");
	// }

	@RequestMapping("/membership/admin-rest-login.*")
	public ModelAndView handleAdminRestLoginRequest(HttpServletRequest request, HttpServletResponse response) throws Exception {
		log.debug("in handleAdminRestLoginRequest...");
		String u = request.getParameter("u");
		String p = request.getParameter("p");
		response.setContentType("text/xml");
		response.setCharacterEncoding("utf-8");
		final User user = userManager.getAdmin(u, p);
		if (user != null) {
			UsernamePasswordAuthenticationToken token = new UsernamePasswordAuthenticationToken(user, null, user.getAuthorities());
			SecurityContextHolder.getContext().setAuthentication(token);
			// TODO: Change session
			return new ModelAndView("ajaxResponse", "result", "pass");
		} else {
			return new ModelAndView("ajaxResponse", "result", "fail");
		}
	}

	@RequestMapping("/membership/unlock.*")
	public ModelAndView handleUnlockRequest(HttpServletRequest request, HttpServletResponse response) throws Exception {
		log.debug("in handleUnlockRequest...");

		String token = request.getParameter("token");
		boolean isSuccessful = userManager.unlock(token);
		return new ModelAndView("/membership/unlock", "isSuccessful", isSuccessful);
	}

	@SuppressWarnings("unchecked")
	@RequestMapping("/membership/forget-password.*")
	public ModelAndView handleForgetPasswordRequest(HttpServletRequest request, HttpServletResponse response) throws Exception {
		log.debug("in handleForgetPasswordRequest...");
		ModelAndView mv = new ModelAndView("/membership/forget-password");
		String email = request.getParameter("email");
		if (!StringUtils.isBlank(email)) {
			User user = userManager.getUserByEmail(email);
			if (null != user) {
				// Generate token and update the user
				String pswdRestToken = RandomStringUtils.random(64, true, true);
				user.setResetPswdToken(pswdRestToken);
				userManager.saveUser(user, false);

				// Send out email
				log.debug("to send out confirmation mail...");
				SimpleMailMessage msg = new SimpleMailMessage();
				Map map = new HashMap();
				String resetPasswordUrl = ServletUtils.getBaseUrl(request) + "/membership/reset-password.html?token=" + pswdRestToken;
				log.debug("resetPasswordUrl: " + resetPasswordUrl);
				map.put("resetPasswordUrl", resetPasswordUrl);
				map.put("userName", user.getName());

				// Setup email
				msg.setTo(email);
				String subject = RequestContextUtils.getWebApplicationContext(request).getMessage("mail.resetPassword.subject", null, request.getLocale());
				msg.setSubject(subject);

				// Send out mail
				mailEngine.SendMessageByStringSource(msg, myGlobManager.fetch().getResetPasswordMailTemplate(), map);
				log.debug("reset password email is out.");

				// Inject page model
				mv.addObject("status", "resetPasswordMailIsOut");
			} else {
				// Send no-this-email error
				mv.addObject("status", "noSuchEmail");
				log.debug("no such email");
			}
		} else {
			log.debug("email is empty.");
		}

		log.debug("view name: " + mv.getViewName());
		return mv;
	}

	@RequestMapping("/membership/reset-password.*")
	public ModelAndView handleRestPasswordRequest(HttpServletRequest request, HttpServletResponse response) throws Exception {
		log.debug("in handleRestPasswordRequest...");

		// Try to reset the password and then notify the user in UI
		ModelAndView mv = new ModelAndView("/membership/reset-password");
		String token = request.getParameter("token");
		mv.addObject("token", token);
		String newPassword = request.getParameter("password_name");
		if (!StringUtils.isBlank(newPassword)) {
			boolean resetSuccess = userManager.resetPassword(token, newPassword);
			mv.addObject("resetSuccess", resetSuccess);
			log.debug("Result for reset attempt: " + resetSuccess);
		}
		return mv;
	}

	@RequestMapping("/membership/signup-done.*")
	public ModelAndView handleSignupDoneRequest(HttpServletRequest request, HttpServletResponse response) throws Exception {
		return new ModelAndView("/membership/signup-done");
	}

}
