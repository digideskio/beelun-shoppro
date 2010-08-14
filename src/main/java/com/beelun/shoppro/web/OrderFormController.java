package com.beelun.shoppro.web;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.apache.commons.lang.StringUtils;
import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.security.context.SecurityContextHolder;
import org.springframework.stereotype.Controller;
import org.springframework.validation.BindException;
import org.springframework.validation.Validator;
import org.springframework.web.bind.ServletRequestDataBinder;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.servlet.ModelAndView;

import com.beelun.shoppro.Constants;
import com.beelun.shoppro.model.ExpressCorp;
import com.beelun.shoppro.model.Order;
import com.beelun.shoppro.model.PaymentTool;
import com.beelun.shoppro.model.User;
import com.beelun.shoppro.model.type.ShipDateEnum;
import com.beelun.shoppro.model.type.ShipTimeEnum;
import com.beelun.shoppro.service.ExpressCorpManager;
import com.beelun.shoppro.service.OrderManager;
import com.beelun.shoppro.service.PaymentToolManager;
import com.beelun.shoppro.service.ShoppingCart;
import com.beelun.shoppro.web.fieldBinding.ExpressCorpPropertyEditor;
import com.beelun.shoppro.web.fieldBinding.PaymentToolPropertyEditor;
import com.beelun.shoppro.web.fieldBinding.ShipDatePropertyEditor;
import com.beelun.shoppro.web.fieldBinding.ShipTimePropertyEditor;

/**
 * Controller for submitting info to back end
 * For spring form work flow,
 * refer to: http://static.springsource.org/spring/docs/2.5.x/api/org/springframework/web/portlet/mvc/AbstractFormController.html
 * 
 * @author Bill Li(bill@beelun.com)
 * 
 */
@Controller
@RequestMapping("/customer/create-order.html")
public class OrderFormController extends BaseFormController {
	private transient final Log log = LogFactory.getLog(OrderFormController.class);

	@Autowired
	OrderManager orderManager;

	@Autowired
	SessionFactory sessionFactory;

	@Autowired
	PaymentToolManager paymentToolManager;

	@Autowired
	ExpressCorpManager expressCorpManager;

	@Autowired(required = false)
	@Qualifier("beanValidator")
	Validator validator;

	public OrderFormController() {
		setCommandName("order"); // name of command used in ftl
		setCommandClass(Order.class); // which bean to map from form input fields
		setFormView("/customer/create-order"); // what UI form to show to user to fill in
		setSuccessView("redirect:/customer/order.html"); // which view to show after successful submission, Add '?id=xxx', also add 'redirect:'?
		if (validator != null) {
			setValidator(validator);
		}
	}

	public ModelAndView processFormSubmission(HttpServletRequest request, HttpServletResponse response, Object command, BindException errors) throws Exception {
		log.debug("in processFormSubmission()...");
		if (request.getParameter("cancel") != null) {
			return new ModelAndView(getSuccessView());
		}

		return super.processFormSubmission(request, response, command, errors);
	}

	// TODO: Transaction support from controller level?
	@SuppressWarnings("unchecked")
	public ModelAndView onSubmit(HttpServletRequest request, HttpServletResponse response, Object command, BindException errors) throws Exception {
		log.debug("onSubmit...");
		Order order = (Order) command;
		log.debug("order.shipDate:" + order.getShipDate());
		log.debug("order.shipTime: " + order.getShipTime());
		log.debug("order.expressCorp: " + order.getExpressCorp().getShortName());
		log.debug("order.paymentTool: " + order.getPaymentTool().getName());
		log.debug("order.notes: " + order.getNotes());

		// Get current user's order and save it to the order
		User user = (User) SecurityContextHolder.getContext().getAuthentication().getPrincipal();
		order.setUser(user);

		// Set address
		order.setShippingAddress(user.getShippingAddress());

		// Set orderItemSet
		ShoppingCart shoppingCart = (ShoppingCart) request.getSession().getAttribute(Constants.SHOPPING_CART);
		if (null != shoppingCart && !shoppingCart.isEmpty()) {
			order.setOrderItemSet(shoppingCart.getOrderItemSet(order));
		} else {
			// TODO: do something such as throwing exception
		}

		// Save this order
		Order savedOrder = orderManager.save(order);
		log.debug("newly created order ID: " + savedOrder.getId());

		// Clean cart
		shoppingCart.clearAll();

		// MV
		ModelAndView mv = new ModelAndView(getSuccessView() + "?id=" + savedOrder.getId());

		// Return
		return mv;
	}

	protected Object formBackingObject(HttpServletRequest request) throws ServletException {
		String orderId = request.getParameter("id");

		if (!StringUtils.isBlank(orderId)) {
			log.debug("getting object from db...");
			return orderManager.get(Long.parseLong(orderId));
		} else {
			log.debug("new order()...");
			return new Order();
		}
	}

	protected void initBinder(HttpServletRequest request, ServletRequestDataBinder binder) {
		log.debug("calling into initBinder()...");
		super.initBinder(request, binder);
		binder.registerCustomEditor(ExpressCorp.class, new ExpressCorpPropertyEditor(sessionFactory));
		binder.registerCustomEditor(PaymentTool.class, new PaymentToolPropertyEditor(sessionFactory));
		binder.registerCustomEditor(ShipDateEnum.class, new ShipDatePropertyEditor());
		binder.registerCustomEditor(ShipTimeEnum.class, new ShipTimePropertyEditor());
	}

	protected ModelAndView showForm(HttpServletRequest request, HttpServletResponse response, BindException errors) throws Exception {
		log.debug("in showForm() to add paymentList and expressCorpList");
		ModelAndView mv = super.showForm(request, response, errors);

		ShoppingCart shoppingCart = (ShoppingCart) request.getSession().getAttribute(Constants.SHOPPING_CART);

		if (null != shoppingCart && !shoppingCart.isEmpty()) {
			mv.addObject("itemList", shoppingCart.getCartItemList());
			User user = (User) SecurityContextHolder.getContext().getAuthentication().getPrincipal();
			log.debug(String.format("username:%s, userId:%d", user.getUsername(), user.getId()));
			if (user.getShippingAddress() != null) {
				mv.addObject("shippingAddress", user.getShippingAddress());
				log.debug("address: " + user.getShippingAddress().getAddress());
			} else {
				log.debug("no address yet.");
			}
		}

		mv.addObject("paymentList", paymentToolManager.getAllAvailable());
		mv.addObject("expressCorpList", expressCorpManager.getAllAvailable());
		return mv;
	}

}
