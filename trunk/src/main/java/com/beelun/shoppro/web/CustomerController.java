package com.beelun.shoppro.web;

import java.util.HashMap;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.context.SecurityContextHolder;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.servlet.ModelAndView;

import com.beelun.shoppro.Constants;
import com.beelun.shoppro.model.Order;
import com.beelun.shoppro.model.PaypalAccessInfo;
import com.beelun.shoppro.model.User;
import com.beelun.shoppro.model.type.OrderStatusEnum;
import com.beelun.shoppro.pay.PaypalAdaptor;
import com.beelun.shoppro.service.ExpressCorpManager;
import com.beelun.shoppro.service.ItemManager;
import com.beelun.shoppro.service.OrderManager;
import com.beelun.shoppro.service.PaymentToolManager;
import com.beelun.shoppro.service.PaypalAccessInfoManager;
import com.beelun.shoppro.service.ShoppingCart;
import com.beelun.shoppro.utils.ServletUtils;

/**
 * Controllers for resources requiring 'ROLE_CUTOMER' role to access
 * 
 * @author Bill Li(bill@beelun.com)
 * 
 */
@org.springframework.stereotype.Controller
public class CustomerController {
	@Autowired
	ItemManager itemManager;

	@Autowired
	PaymentToolManager paymentToolManager;

	@Autowired
	ExpressCorpManager expressCorpManager;

	@Autowired
	OrderManager orderManager;

	@Autowired
	PaypalAccessInfoManager paypalAccessInfoManager;

	private transient final Log log = LogFactory.getLog(CustomerController.class);

	@RequestMapping("/customer/checkout.html")
	public ModelAndView handleCheckoutRequest(HttpServletRequest request, HttpServletResponse response) throws Exception {

		log.debug("url: " + request.getRequestURI());

		// Get shoppingCart from session
		ModelAndView mv = new ModelAndView("/customer/checkout");
		ShoppingCart shoppingCart = (ShoppingCart) request.getSession().getAttribute(Constants.SHOPPING_CART);

		// Only when there are item(s) in the cart, we start the checkout
		// process
		if (null != shoppingCart && !shoppingCart.isEmpty()) {
			log.debug("items in cart, start checkout...");

			// itemList
			mv.addObject("itemList", shoppingCart.getCartItemList());

			// address
			User user = (User) SecurityContextHolder.getContext().getAuthentication().getPrincipal();
			log.debug(String.format("username:%s, userId:%d", user.getUsername(), user.getId()));
			if (user.getShippingAddress() != null) {
				mv.addObject("shippingAddress", user.getShippingAddress());
				log.debug("address: " + user.getShippingAddress().getAddress());
			} else {
				log.debug("no address yet.");
			}

			// payment
			mv.addObject("paymentList", paymentToolManager.getAllAvailable());

			// express
			mv.addObject("expressCorpList", expressCorpManager.getAllAvailable());
		}

		// Return
		return mv;
	}

	@SuppressWarnings("unchecked")
	@RequestMapping("/customer/paypal-express-checkout.html")
	public ModelAndView handleStartPaypalPaymentRequest(HttpServletRequest request, HttpServletResponse response) throws Exception {
		String orderIdString = request.getParameter("orderId");
		Order order = orderManager.get(Long.parseLong(orderIdString));
		String paymentAmountString = order.getAmount().toPlainString();
		log.debug("paymentAccount: " + paymentAmountString);

		String returnURL = ServletUtils.getBaseUrl(request) + "/customer/confirm-order.html" + "?orderId=" + orderIdString;
		String cancelURL = ServletUtils.getBaseUrl(request) + "/index.html";

		/*
		 * '------------------------------------ ' Calls the SetExpressCheckout
		 * API call ' ' The CallShortcutExpressCheckout function is defined in
		 * the file PayPalFunctions.asp, ' it is included at the top of this
		 * file. '-------------------------------------------------
		 */
		PaypalAccessInfo paypalAccessInfo = this.paypalAccessInfoManager.fetch();
		PaypalAdaptor ppf = new PaypalAdaptor(paypalAccessInfo.getApiUserName(), paypalAccessInfo.getApiPassword(), paypalAccessInfo.getApiSignature(), paypalAccessInfo.isUseSandbox());
		HashMap nvp = ppf.callSetExpressCheckout(paymentAmountString, returnURL, cancelURL);
		if (this.isPaypalResponseSuccess(nvp)) {
			log.debug("payment with PayPal is started...");
			log.debug("TOKEN: " + nvp.get("TOKEN").toString());
			// ' Redirect to paypal.com now
			ppf.RedirectURL(response, nvp.get("TOKEN").toString());
		} else {
			logPaypayError(nvp);
			response.sendRedirect(ServletUtils.getBaseUrl(request) + "/customer/checkout-paypal-error.html");
		}
		return null;
	}

	@RequestMapping("/customer/confirm-order.html")
	public ModelAndView handleConfirmOrderRequest(HttpServletRequest request, HttpServletResponse response) throws Exception {
		log.info("in handleConfirmOrderRequest()...");
		String orderIdString = request.getParameter("orderId");
		Order order = orderManager.get(Long.parseLong(orderIdString));

		// Make sure the order belongs to currently logged in user.
		// Otherwise, return a 404 address
		User loggedInUser = (User) SecurityContextHolder.getContext().getAuthentication().getPrincipal();

		if (order.getUser().getId().equals(loggedInUser.getId())) {
			ModelAndView mv = new ModelAndView("/customer/confirm-order");
			mv.addObject("order", order);
			return mv;
		} else {
			// Return a 404. Should be a security error?
			response.sendError(404);
			return null;
		}
	}

	@SuppressWarnings("unchecked")
	@RequestMapping("/customer/paypal-checkout-get-details.html")
	public ModelAndView handleCheckoutGetDetailsRequest(HttpServletRequest request, HttpServletResponse response) throws Exception {
		log.info("in handleCheckoutGetDetailsRequest...");
		String token = request.getParameter("TOKEN");
		String paymentAmount = request.getParameter("AMT");
		String orderIdString = request.getParameter("orderId");

		PaypalAccessInfo paypalAccessInfo = this.paypalAccessInfoManager.fetch();
		PaypalAdaptor ppf = new PaypalAdaptor(paypalAccessInfo.getApiUserName(), paypalAccessInfo.getApiPassword(), paypalAccessInfo.getApiSignature(), paypalAccessInfo.isUseSandbox());
		HashMap nvpDetails = ppf.callGetExpressCheckoutDetails(token);
		if (this.isPaypalResponseSuccess(nvpDetails)) {

			// TODO: Save ship-to address to my DB

			String payerId = nvpDetails.get("PAYERID").toString();

			// Do real payment now
			HashMap nvpDoPayment = ppf.ConfirmPayment(token, payerId, paymentAmount, "");

			// If success, change payment status
			if (this.isPaypalResponseSuccess(nvpDoPayment)) {
				// Change order status to PAID
				log.info("order is paid via PayPal: " + orderIdString);
				Order order = orderManager.get(Long.parseLong(orderIdString));
				order.setStatus(OrderStatusEnum.PAID);
				orderManager.save(order);
			} else {
				logPaypayError(nvpDetails);
				response.sendRedirect(ServletUtils.getBaseUrl(request) + "/customer/checkout-paypal-error.html");
				return null;
			}

			response.sendRedirect(ServletUtils.getBaseUrl(request) + "/customer/checkout-thankyou.html" + "?orderId=" + orderIdString);
			return null;
		} else {
			logPaypayError(nvpDetails);
			response.sendRedirect(ServletUtils.getBaseUrl(request) + "/customer/checkout-paypal-error.html");
			return null;
		}
	}

	@RequestMapping("/customer/checkout-thankyou.html")
	public ModelAndView handleCheckoutThankyouRequest(HttpServletRequest request, HttpServletResponse response) throws Exception {
		log.info("in handleCheckoutThankyouRequest()...");
		return new ModelAndView("/customer/checkout-thankyou");
	}

	@RequestMapping("/customer/checkout-paypal-error.html")
	public ModelAndView handleCheckoutErrorRequest(HttpServletRequest request, HttpServletResponse response) throws Exception {
		return new ModelAndView("/customer/checkout-paypal-error");
	}

	/**
	 * Log the error during interaction with paypal
	 * 
	 * @param nvp
	 */
	@SuppressWarnings("unchecked")
	private void logPaypayError(HashMap nvp) {
		// Display a user friendly Error on the page using any of the following
		// error information returned by PayPal
		if (nvp != null) {
			log.error("fail to pay:");
			HashMap<String, Object> theNvp = (HashMap<String, Object>) nvp;
			for (String theKey : theNvp.keySet()) {
				log.error("key: " + theKey + "value: " + nvp.get(theKey).toString());
			}
		} else {
			log.error("nvp is null.");
		}
	}

	/**
	 * Check whether the response of Paypal is successful or not
	 * 
	 * @param nvp
	 * @return
	 */
	@SuppressWarnings("unchecked")
	private boolean isPaypalResponseSuccess(HashMap nvp) {
		boolean successStatus = false;
		;
		if (nvp != null) {
			String strAck = nvp.get("ACK").toString();
			if (strAck != null && strAck.equalsIgnoreCase("Success")) {
				successStatus = true;
			}
		}
		return successStatus;
	}
}
