package com.beelun.shoppro.web;

import org.apache.log4j.Logger;
import org.jmock.MockObjectTestCase;

public class ShoppingCartControllerTest extends MockObjectTestCase {
	private static final Logger log = Logger.getLogger(ShoppingCartControllerTest.class);
	
	ShoppingCartController c = new ShoppingCartController();
	
	/**
	 * Looks like it is not necessary to test this controller
	 * 
	 */
	public void testMe() {
		log.info("add test code here.");
	}
}
