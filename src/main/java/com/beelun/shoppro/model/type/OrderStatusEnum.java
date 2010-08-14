package com.beelun.shoppro.model.type;

/**
 * The status of Order
 * !!! DO NOT RE-ORDER THEM otherwise you will lose the correct mapping with the values in your DB !!!
 * 
 * @author bali
 *
 */
public enum OrderStatusEnum {
	PAID,		// Money is paid, we need ship the product now
	NOTPAID,	// Placed order, but not paid yet
	SHIPPING,	// The product is on its way
	CLOSED		// Product is confirmed as received. 
}
