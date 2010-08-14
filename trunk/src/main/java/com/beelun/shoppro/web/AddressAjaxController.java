package com.beelun.shoppro.web;

import java.util.HashMap;
import java.util.Map;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.apache.commons.lang.StringUtils;
import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.context.SecurityContextHolder;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.servlet.ModelAndView;

import com.beelun.shoppro.model.Address;
import com.beelun.shoppro.model.User;
import com.beelun.shoppro.service.AddressManager;
import com.beelun.shoppro.service.UserManager;

/**
 * For submitting/updating address
 *
 * @author <a href="mailto:bill@beelun.com">Bill Li</a>
 *
 */
@Controller
public class AddressAjaxController  {
	private transient final Log log = LogFactory.getLog(AddressAjaxController.class);
	
	@Autowired
	AddressManager addressManager;
	
	@Autowired
	UserManager userManager;
	
	@SuppressWarnings("unchecked")
	@RequestMapping("/customer/create-update-address.html")
	public ModelAndView handleAjaxRequest(HttpServletRequest request,
			HttpServletResponse response) throws Exception {
		
		String code = "200";
		String message = "";
		String addressId = "";
		
		// Get param
		// We don't need provide addressId here, because the id will be read from User.address.id or generated after save()
		String name = request.getParameter("name");
		String addressString = request.getParameter("address");
		String zip = request.getParameter("zip");
		String recipientName = request.getParameter("recipientName");
		String phoneNumber = request.getParameter("phoneNumber");
		String mobileNumber = request.getParameter("mobileNumber");
		
		// Do validation...
		// TODO: (1) add more validation, (2) streamline error code and error message
		if(StringUtils.isBlank(phoneNumber) && StringUtils.isBlank(mobileNumber)) {
			code = "201";
			message = "It is not allowed that both phone number and mobile number are empty.";
		} else if (!StringUtils.isNumeric(zip)) {
			code = "202";
			message = "Zip code should contain number only.";
		} else {		
			// Attach this address to current user. This controller runs only when user is logged in
			User user = (User) SecurityContextHolder.getContext().getAuthentication().getPrincipal();
			
			Address address = null;
			if(user.getShippingAddress() == null) {
				address = new Address();
			} else {
				address = user.getShippingAddress();
			}
			
			// Save address to db	
			address.setAddress(addressString);
			address.setMobileNumber(mobileNumber);
			address.setName(name);
			address.setPhoneNumber(phoneNumber);
			address.setRecipientName(recipientName);
			address.setZip(zip);
			
			try{
				// TODO: I've changed cascade to 'all', so saving address in before hand might NOT be necessary.
				Address savedAddress = addressManager.save(address);
				addressId = savedAddress.getId().toString();
				
				user.setShippingAddress(savedAddress);
				userManager.saveUser(user, false);
			}catch(Exception ex) {
				log.error(ex.getMessage());
				code = "500";
				message = ex.getMessage();
			}
		}

		// This controller will return xml format, so we set it here.
		response.setContentType("text/xml");
		response.setCharacterEncoding("utf-8");
		
		Map resultMap = new HashMap();
		resultMap.put("code", code);
		resultMap.put("message", message);
		if(!StringUtils.isBlank(addressId)) {
			resultMap.put("result", String.format("<addressId>" + addressId + "</addressId>"));
		}
		log.debug("code: " + code);

		return new ModelAndView("ajaxResponse", resultMap);
	}

}
