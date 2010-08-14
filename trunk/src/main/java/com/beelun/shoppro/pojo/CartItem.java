package com.beelun.shoppro.pojo;

import com.beelun.shoppro.model.Item;

/**
 * This class stands for the item in the shopping cart
 * 
 * @author Bill Li(bill@beelun.com)
 *
 */
public class CartItem {
	// The item
	private Item item;
	
	// Number of the item in the cart
	private long count;
	
	public CartItem(Item item, long count) {
		this.item = item;
		this.count = count;
	}

	public Item getItem() {
		return item;
	}

	public long getCount() {
		return count;
	}
}
