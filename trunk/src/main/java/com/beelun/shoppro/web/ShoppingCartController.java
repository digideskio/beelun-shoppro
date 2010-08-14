package com.beelun.shoppro.web;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.apache.commons.lang.StringUtils;
import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.dom4j.Document;
import org.dom4j.DocumentHelper;
import org.dom4j.Element;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.servlet.ModelAndView;

import com.beelun.shoppro.Constants;
import com.beelun.shoppro.service.ItemManager;
import com.beelun.shoppro.service.ShoppingCart;
import com.beelun.shoppro.service.impl.ShoppingCartImpl;
import com.beelun.shoppro.utils.GeneralUtils;
import com.beelun.shoppro.utils.ShoppingCartActionEnum;

/**
 * Controls how visitors interacts with shopping cart such as add(new item),
 * remove, clear etc.
 * 
 * @author Bill Li(bill@beelun.com)
 * 
 */
@org.springframework.stereotype.Controller
public class ShoppingCartController {

	private transient final Log log = LogFactory
			.getLog(ShoppingCartController.class);

	@Autowired
	ItemManager itemManager;

	/**
	 * Response to cart ajax call with xml format of response Url to be handled
	 * by this controller is as following format:
	 * 
	 * /cart/cartAjax.html?itemId=aaa&action=bbb&n=ccc aaa - itemId of the
	 * question bbb - add/remove/clean ccc - number of itemId to add/remove. If
	 * empty, take 1.
	 * 
	 * Return page sample: <?xml version="1.0" encoding="UTF-8"?> <root>
	 * <Status> <Code>200</Code> <Message></Message> </Status> <Result>
	 * <cartSummary> <itemNumber>3</itemNumber> <itemValue>89.99</itemValue>
	 * </cartSummary> </Result> </root>
	 * 
	 * 
	 * @param request
	 * @param response
	 * @return
	 * @throws Exception
	 */
	@RequestMapping("/cart/cartAjax.html")
	public ModelAndView handleAjaxRequest(HttpServletRequest request,
			HttpServletResponse response) throws Exception {
		// Get parameters
		String itemIdString = request.getParameter("itemId");
		String countString = request.getParameter("n");
		String actionString = request.getParameter("action");
		log.debug(String.format("itemId:%s, count:%s, action:%s", itemIdString,
				countString, actionString));
		Long itemId = Long.parseLong(StringUtils.trim(itemIdString));
		
		// If 'n' is null, we take 1 by default(by design)
		Long count = new Long(1);
		if (!StringUtils.isBlank(countString)) {
			count = Long.parseLong(countString);
		}

		// Get shoppingCart from session
		ShoppingCart shoppingCart = null;
		Object shoppingCartObject = request.getSession().getAttribute(
				Constants.SHOPPING_CART);
		if (null == shoppingCartObject) {
			shoppingCart = new ShoppingCartImpl(this.itemManager);
			request.getSession().setAttribute(Constants.SHOPPING_CART,
					shoppingCart);
		} else {
			shoppingCart = (ShoppingCart) shoppingCartObject;
		}

		// Switch according to action
		ShoppingCartActionEnum action = ShoppingCartActionEnum
				.valueOf(actionString.toLowerCase());
		switch (action) {
		case add:
			shoppingCart.add(itemId, count);
			break;
		case remove:
			shoppingCart.remove(itemId, count);
			break;
		case clear_all:
			shoppingCart.clearAll();
			break;
		case clear_one:
			shoppingCart.clearOne(itemId);
			break;
		case set_items:
			shoppingCart.setItems(itemId, count);
		default:
			break;
		}

		// This controller will return xml format, so we set it here.
		response.setContentType("text/xml");
		response.setCharacterEncoding("utf-8");

		// Prepare for items in the cart
		long itemNumber = shoppingCart.getItemNumber();
		float itemTotalValue = shoppingCart.getItemTotalValue();

		final Document document = DocumentHelper.createDocument();
		final Element rootElement = document.addElement("cartSummary");
		rootElement.addElement("itemNumber").addText(itemNumber + "");
		rootElement.addElement("itemValue").addText(itemTotalValue + "");

		String result = GeneralUtils.toString(document, true);

		// TODO: We always return '200'(success message here) now. add exception
		// handling here.
		return new ModelAndView("ajaxResponse", "result", result);
	}

	@RequestMapping("/cart/itemList.html")
	public ModelAndView handleListItem(HttpServletRequest request,
			HttpServletResponse response) {

		// Get shoppingCart from session
		ShoppingCart shoppingCart = null;
		Object shoppingCartObject = request.getSession().getAttribute(
				Constants.SHOPPING_CART);
		if (null == shoppingCartObject) {
			shoppingCart = new ShoppingCartImpl(this.itemManager);
			request.getSession().setAttribute(Constants.SHOPPING_CART,
					shoppingCart);
		} else {
			shoppingCart = (ShoppingCart) shoppingCartObject;
		}

		return new ModelAndView("itemList", "cartItemList", shoppingCart
				.getCartItemList());
	}

}
