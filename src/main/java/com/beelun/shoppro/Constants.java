package com.beelun.shoppro;

/**
 * Constant values used throughout the application.
 * 
 * @author Bill Li(bill@beelun.com)
 *
 */
public class Constants {
	/**
	 * The name of shopping cart attribute in the request session
	 */
    public static final String SHOPPING_CART = "shoppingCart";
    
    /**
     * The name of the configuration hashmap stored in application scope.
     */
    public static final String CONFIG = "appConfig";
    
    /**
     * Success message
     */
    public static final String MESSAGES_KEY = "successMessages";
    
    /**
     * Error messages
     */
    public static final String ERROR_KEY = "errorMessages";
    
    /**
     * Session attribute for PayPal payment amount
     */
    public static final String PAYPAL_Payment_Amount = "Payment_Amount";
    
    /**
     * Product tab ID
     */
    public static final Long PRODUCT_TAB_ID = new Long(2);
    
    /**
     * Brand.No_brand
     */
    public static final Long NO_BRAND_ID = new Long(10);

    /**
     * For showing items from different category
     */
	public static final Long ALL_CATEGORIES = new Long(-1);
	public static final Long UNCATEGORIED = new Long(-2);

}
