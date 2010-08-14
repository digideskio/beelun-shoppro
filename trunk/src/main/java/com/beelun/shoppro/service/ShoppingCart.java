package com.beelun.shoppro.service;

import java.util.List;
import java.util.Set;
 
import com.beelun.shoppro.model.Order;
import com.beelun.shoppro.pojo.CartItem;

/**
 * The class contains what items are in current visitors's shopping cart
 * 
 * @author Bill Li(bill@beelun.com)
 *
 */
public interface ShoppingCart {
	/**
	 * Add items to shopping cart  
	 * @param itemId 
	 * 			the item to be added
	 * @param n 
	 * 			number of item to add
	 */
	public void add(Long itemId, Long n);
	
	/**
	 * Remove items from shopping cart 
	 * @param itemId
	 * 			the item to be removed
	 * @param n
	 * 			number of item to remove
	 */
	public void remove(Long itemId, Long n);
	
	/**
	 * Clear this shopping cart
	 */
	public void clearAll();
	
	/**
	 * Clear one specified item from shopping cart
	 * @param itemId
	 * 			the item to be cleared
	 */
	public void clearOne(Long itemId);
	
	/**
	 * Get item lists in the shopping cart
	 * @return
	 */
	public List<CartItem> getCartItemList();

	/**
	 * Get number of items in the cart now
	 * @return
	 */
	public long getItemNumber();
	
	/**
	 * Get total value of items in the cart now
	 * @return
	 */
	public float getItemTotalValue();
	
	/**
	 * whether the cart is empty or not
	 * @return
	 */
	public boolean isEmpty();
	
	/**
	 * Set items to specific number in the cart
	 * @param itemId
	 * @param n
	 */
	public void setItems(Long itemId, Long n);
	
	/**
	 * Get cart item for saving to db purpose
	 * @param order
	 * @return
	 */
	@SuppressWarnings("unchecked")
	public Set getOrderItemSet(Order order);
}
