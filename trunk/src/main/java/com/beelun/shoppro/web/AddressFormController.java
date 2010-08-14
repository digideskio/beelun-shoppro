package com.beelun.shoppro.web;

import java.util.ArrayList;
import java.util.EnumSet;
import java.util.List;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.context.SecurityContextHolder;
import org.springframework.stereotype.Controller;
import org.springframework.validation.BindException;
import org.springframework.validation.ObjectError;
import org.springframework.validation.Validator;
import org.springframework.web.bind.ServletRequestDataBinder;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.servlet.ModelAndView;

import com.beelun.shoppro.Constants;
import com.beelun.shoppro.model.Order;
import com.beelun.shoppro.model.User;
import com.beelun.shoppro.model.type.USStateEnum;
import com.beelun.shoppro.pojo.AddressWrapper;
import com.beelun.shoppro.service.AddressManager;
import com.beelun.shoppro.service.OrderManager;
import com.beelun.shoppro.service.ShoppingCart;
import com.beelun.shoppro.service.UserManager;
import com.beelun.shoppro.web.fieldBinding.USStatePropertyEditor;

/**
 * For user input/update address
 * !! THIS IS FOR US MARKET ONLY
 * 
 * @author bali
 * 
 */
@Controller
@RequestMapping("/customer/input-address.html")
public class AddressFormController extends BaseFormController {
	private final Log log = LogFactory.getLog(AddressFormController.class);

	@Autowired
	UserManager userManager;

	@Autowired
	Validator validator;

	@Autowired
	AddressManager addressManager;

	@Autowired
	OrderManager orderManager;

	public AddressFormController() {
		// NB: Autowire happens after the bean is created by certain constructor
		// So, validator/userManager etc are null at this point.
		setCommandName("addressWrapper");
		setCommandClass(AddressWrapper.class);
		setFormView("/customer/input-address2");
		setSuccessView("redirect:/customer/paypal-express-checkout.html");
	}

	@SuppressWarnings("unchecked")
	public ModelAndView onSubmit(HttpServletRequest request, HttpServletResponse response, Object command, BindException errors) throws Exception {
		log.debug("onSubmit...");
		AddressWrapper addressWrapper = (AddressWrapper) command;
		log.debug("same adress? " + addressWrapper.isSameAddress());
		log.debug("billing address state:" + addressWrapper.getBillingAddress().getState().name());
		log.debug("billing address phone:" + addressWrapper.getBillingAddress().getPhoneNumber());
		log.debug("shipping address state:" + addressWrapper.getShippingAddress().getState().name());
		log.debug("shipping address phone:" + addressWrapper.getShippingAddress().getPhoneNumber());

		// Get current user
		User user = (User) SecurityContextHolder.getContext().getAuthentication().getPrincipal();
		addressWrapper.putToUser(user);

		Order order = new Order(user);
		addressWrapper.putToOrder(order);

		// Set orderItemSet
		ShoppingCart shoppingCart = (ShoppingCart) request.getSession().getAttribute(Constants.SHOPPING_CART);
		if (null != shoppingCart && !shoppingCart.isEmpty()) {
			order.setOrderItemSet(shoppingCart.getOrderItemSet(order));

			// Save this order
			Order savedOrder = orderManager.save(order);
			log.debug("newly created order ID: " + savedOrder.getId());

			// Update this user
			userManager.saveUser(user, false);

			// Clean cart
			if (null != shoppingCart) {
				shoppingCart.clearAll();
			}

			return new ModelAndView(getSuccessView() + "?orderId=" + savedOrder.getId());
		} else {
			// This should not happen since onBindAndValidate will catch this.
			log.error("empty cart");
			return null;
		}
	}

	protected void onBindAndValidate(HttpServletRequest request, Object command, BindException errors) {
		log.debug("in onBindAndValidate()");

		ShoppingCart shoppingCart = (ShoppingCart) request.getSession().getAttribute(Constants.SHOPPING_CART);
		if (null == shoppingCart || shoppingCart.isEmpty()) {
			log.error("empty cart");
			errors.addError(new ObjectError("emptyCart", getText("errors.emptyCart")));
		}
	}

	protected Object formBackingObject(HttpServletRequest request) throws ServletException {
		// Get current logged in user and then construct a AddressWrapper object
		User user = (User) SecurityContextHolder.getContext().getAuthentication().getPrincipal();
		return new AddressWrapper(user);
	}

	protected void initBinder(HttpServletRequest request, ServletRequestDataBinder binder) {
		log.debug("calling into initBinder()...");
		super.initBinder(request, binder);
		binder.registerCustomEditor(USStateEnum.class, new USStatePropertyEditor());
	}

	/**
	 * Put us state in the list so that ftl can refer to
	 */
	protected ModelAndView showForm(HttpServletRequest request, HttpServletResponse response, BindException errors) throws Exception {
		log.debug("in showForm() to add us state collection.");

		// State list
		List<String> stateList = new ArrayList<String>();
		for (USStateEnum state : EnumSet.allOf(USStateEnum.class)) {
			stateList.add(state.name());
		}

		// Put to mv
		ModelAndView mv = super.showForm(request, response, errors);
		mv.addObject("stateList", stateList);
		return mv;
	}
}
