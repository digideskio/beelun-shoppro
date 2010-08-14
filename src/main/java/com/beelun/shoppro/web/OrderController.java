package com.beelun.shoppro.web;

import java.util.List;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.context.SecurityContextHolder;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.servlet.ModelAndView;

import com.beelun.shoppro.model.Order;
import com.beelun.shoppro.model.User;
import com.beelun.shoppro.service.OrderManager;

@Controller
public class OrderController {
	private transient final Log log = LogFactory.getLog(OrderController.class);

	@Autowired
	OrderManager orderManager;
	
	@RequestMapping("/customer/order.html")
	public ModelAndView handleOrdertRequest(HttpServletRequest request,
			HttpServletResponse response) throws Exception {
		// Get the object
		String idString = request.getParameter("id");
		//String detailsString = request.getParameter("details");	// true/false
		log.debug("order id: " + idString);
		Order order = orderManager.get(Long.parseLong(idString));
		if(null == order) {
			response.sendError(404);
			return null;			
		}
		log.debug("order serail number: " + order.getSerialNumber());

		// Make sure the order belongs to currently logged in user.
		// Otherwise, return a 404 address
		User loggedInUser = (User) SecurityContextHolder.getContext()
				.getAuthentication().getPrincipal();

		if (order.getUser().getId().equals(loggedInUser.getId())) {
			// Add objects and return
			ModelAndView mv = new ModelAndView("/customer/order");
			mv.addObject("order", order);			
			return mv;
		} else {
			// This user don've have permission
			response.sendError(404);
			return null;
		}
	}
	
	@RequestMapping("/customer/order-list.html")
	public ModelAndView handleOrderListRequest(HttpServletRequest request,
			HttpServletResponse response) throws Exception {		
		// Get current user's order
		User user = (User) SecurityContextHolder.getContext().getAuthentication().getPrincipal();
		List<Order> orderList = orderManager.getOrderListByUser(user);
		
		// Add objects and return
		ModelAndView mv = new ModelAndView("/customer/order-list");
		mv.addObject("orderList", orderList);
		return mv; 
	}
	
	@RequestMapping("/customer/pay.html")
	public ModelAndView handlePayRequest(HttpServletRequest request,
			HttpServletResponse response) throws Exception {
		String orderId = request.getParameter("id");
		Order order = orderManager.get(Long.parseLong(orderId));
		
		// Add objects and return. Do redirect here.
		ModelAndView mv = new ModelAndView("redirect:" + order.getPaymentTool().getName());
		return mv; 
	}
}
