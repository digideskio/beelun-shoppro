package com.beelun.shoppro.web;

import java.util.HashMap;
import java.util.Map;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.apache.commons.lang.RandomStringUtils;
import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.mail.SimpleMailMessage;
import org.springframework.stereotype.Controller;
import org.springframework.validation.BindException;
import org.springframework.validation.FieldError;
import org.springframework.validation.Validator;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.servlet.ModelAndView;

import com.beelun.shoppro.model.User;
import com.beelun.shoppro.service.MailEngine;
import com.beelun.shoppro.service.MyGlobManager;
import com.beelun.shoppro.service.RoleManager;
import com.beelun.shoppro.service.UserManager;
import com.beelun.shoppro.utils.ServletUtils;

@Controller
@RequestMapping("/membership/create-user.html")
public class UserFormController extends BaseFormController {
	private final Log log = LogFactory.getLog(UserFormController.class);
	@Autowired
	UserManager userManager;

	@Autowired
	RoleManager roleManager;

	@Autowired
	Validator validator;

	@Autowired
	MailEngine mailEngine;

	@Autowired
	MyGlobManager myGlobManager;

	public UserFormController() {
		// NB: Autowire happens after the bean is created by certain constructor
		// So, validator/userManager etc are null at this point.
		setCommandName("user");
		setCommandClass(User.class);
		setFormView("/membership/signup");
		setSuccessView("redirect:/membership/signup-done.html");
	}

	public ModelAndView processFormSubmission(HttpServletRequest request, HttpServletResponse response, Object command, BindException errors) throws Exception {

		// Keep this for extensibility although there is no cancel button in View now
		if (request.getParameter("cancel") != null) {
			return new ModelAndView(getSuccessView());
		}

		return super.processFormSubmission(request, response, command, errors);
	}

	@Override
	protected void onBind(HttpServletRequest request, Object command) throws Exception {
		log.debug("in onBind()");
		// if the user is being deleted, turn off validation
		if (request.getParameter("delete") != null) {
			super.setValidateOnBinding(false);
		} else {
			// We should set validator here so that it will do validation during
			// binding
			// TODO: add to validator array?
			setValidator(validator);
			super.setValidateOnBinding(true);
		}
	}

	protected void onBindAndValidate(HttpServletRequest request, Object command, BindException errors) {
		log.debug("in onBindAndValidate()");
		// Check user existence
		User user = (User) command;
		if (true == userManager.exists(user.getEmail())) {
			log.error("user exists, error");

			FieldError emailFieldError = new FieldError("user", "user.email", getText("user.exists"));
			errors.addError(emailFieldError);
		}
	}

	@SuppressWarnings("unchecked")
	public ModelAndView onSubmit(HttpServletRequest request, HttpServletResponse response, Object command, BindException errors) throws Exception {
		log.debug("entering 'onSubmit' method...");

		User user = (User) command;

		if (request.getParameter("delete") != null) {
			userManager.removeUser(user.getId().toString());
			request.getSession().setAttribute("message", getText("user.deleted", user.getName()));
		} else {
			// Set account locked to wait for user to activate it via link
			user.setAccountLocked(true);

			// Create unlock token
			String unlockToken = RandomStringUtils.random(64, true, true);
			user.setUnlockToken(unlockToken);

			// Save
			userManager.saveUser(user, true);

			if (mailEngine != null) {
				// log.debug("to send out notification mail...");
				// String unlockUrl = ServletUtils.getBaseUrl(request) + "/membership/unlock.html?token=" + unlockToken;
				// BeelunMail beelunMail = new BeelunMail(new String[] { user.getEmail() }, "mail.signupConfirmMail.subject");
				// beelunMail.addObject("unlockUrl", unlockUrl);
				// beelunMail.addObject("userName", user.getName());
				// mailEngine.send(beelunMail);

				log.debug("to send out confirmation mail...");
				// Send out mail
				SimpleMailMessage msg = new SimpleMailMessage();
				Map map = new HashMap();
				String unlockUrl = ServletUtils.getBaseUrl(request) + "/membership/unlock.html?token=" + unlockToken;
				log.debug("unlockUrl: " + unlockUrl);
				map.put("unlockUrl", unlockUrl);
				map.put("userName", user.getName());

				// Setup mail
				msg.setTo(user.getEmail());
				final String mailSubject = getText("mail.signupConfirmMail.subject", myGlobManager.fetch().getShopName());
				msg.setSubject(mailSubject);

				mailEngine.SendMessageByStringSource(msg, myGlobManager.fetch().getUnlockEmailTemplate(), map);
			} else {
				log.error("no mail engine.");
				response.sendError(500);
			}
			// Not necessary?
			// request.getSession().setAttribute("message", getText("user.saved", user.getName()));
		}

		return new ModelAndView(getSuccessView());
	}

	protected Object formBackingObject(HttpServletRequest request) throws ServletException {
		String userId = request.getParameter("id");

		if ((userId != null) && !userId.equals("")) {
			return userManager.getUserById(userId);
		} else {
			// Set user as ROLE_CUSTOMER
			return new User(roleManager.getCustomerRole());
		}
	}
}
