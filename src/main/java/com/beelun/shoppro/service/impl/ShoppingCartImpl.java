package com.beelun.shoppro.service.impl;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;

import com.beelun.shoppro.model.Item;
import com.beelun.shoppro.model.Order;
import com.beelun.shoppro.model.OrderItem;
import com.beelun.shoppro.pojo.CartItem;
import com.beelun.shoppro.service.ItemManager;
import com.beelun.shoppro.service.ShoppingCart;

/**
 * Implementation of ShoppingCart class
 * This class will be in single-threaded environment only, so it is not necessary to add locks.
 * This implementation will not save cart to DB and will be put to request session instead.
 * 
 * TODO: Alternatively, we may consider put logged in user's shopping cart to DB in upcoming releases
 *  
 * @author Bill Li(bill@beelun.com)
 *
 */
public class ShoppingCartImpl implements ShoppingCart {
	private transient final Log log = LogFactory.getLog(ShoppingCartImpl.class);
	
	private ItemManager itemManager;
	
	// Format <ItemId, how many this item in the shopping cart>
	private HashMap<Long, Long> cartHashTable = new HashMap<Long, Long>();
	
	public ShoppingCartImpl(ItemManager itemManager) {
		this.setItemManager(itemManager);
	}
	
	@Override
	public void add(Long itemId, Long n) {
		if(itemManager.exists(itemId)) { // Add the item only when it exists in the db
			if(cartHashTable.containsKey(itemId)) {
				Long value = cartHashTable.get(itemId);
				cartHashTable.put(itemId, new Long(value.longValue() + n.longValue()));
			} else {
				cartHashTable.put(itemId, n);
			}
		}
		log.debug(String.format("After add(), item:count=%d:%d", itemId, cartHashTable.get(itemId)));
	}

	@Override
	public void clearAll() {
		cartHashTable.clear();
		log.debug("cart is cleared.");
	}

	@Override
	public void remove(Long itemId, Long n) {
		if(cartHashTable.containsKey(itemId) && n >= 1){
			Long value = cartHashTable.get(itemId);
			if(value.longValue() <= 1) {
				cartHashTable.remove(itemId);
				log.debug(itemId + "is removed");
			} else {
				// value = value - n;
				long newCount = value.longValue() - n.longValue();
				if(newCount >= 1) { 
					cartHashTable.put(itemId, new Long(newCount));
					log.debug(String.format("After remove(), item:count=%d:%d", itemId, newCount));
				} else {
					cartHashTable.remove(itemId);
					log.debug(String.format("After remove(), item:%d doesnot exist any more.", itemId));
				}
			}
		} else {
			log.debug("there is no such itemId in the cart. itemId:" + itemId);
		}
	}

	@Override
	public void clearOne(Long itemId) {
		if(cartHashTable.containsKey(itemId)) {
			cartHashTable.remove(itemId);
		}
	}

	/**
	 * We use <ItemId, Long> to store shoppingCart in the session,
	 * This method will return <Item, Long> so that UI layer can get item's other properties such as thumbnail, name, etc.
	 * 
	 */
	@Override
	public List<CartItem> getCartItemList() {
		List<CartItem> cartItemList = new ArrayList<CartItem>();		

		List<Long> keyList = new ArrayList<Long>();
		for(Long theKey:  cartHashTable.keySet()) {
			keyList.add(theKey);
		}
		
		List<Item> itemList = itemManager.getByIdList(keyList);
		
		if(null != itemList) {
			for(Item i : itemList) {
				cartItemList.add(new CartItem(i, cartHashTable.get(i.getId()).longValue()));
			}
		}
		return cartItemList;
	}

	public void setItemManager(ItemManager itemManager) {
		this.itemManager = itemManager;
	}

	@Override
	public long getItemNumber() {
		long retItemNumber = 0;
		for(Long itemNumber : cartHashTable.values()) {
			retItemNumber += itemNumber.longValue();
		}
		return retItemNumber;
	}

	@Override
	public float getItemTotalValue() {
		// Sum(item_i * count_i)
		List<CartItem> cartItemList = this.getCartItemList();
		float itemTotalValue = 0f;
		for(CartItem cartItem : cartItemList) {
			itemTotalValue += cartItem.getItem().getSellPrice().multiply(new BigDecimal(cartItem.getCount())).floatValue();
		}
		return itemTotalValue;
	}

	@Override
	public boolean isEmpty() {
		return (this.getItemNumber()==0);
	}

	@Override
	public void setItems(Long itemId, Long n) {
		this.clearOne(itemId);
		this.cartHashTable.put(itemId, n);		
	}

	@SuppressWarnings("unchecked")
	@Override
	public Set getOrderItemSet(Order order) {
		Set<OrderItem> orderItemSet = new HashSet<OrderItem>();
		List<CartItem> cartItemList = this.getCartItemList();
		for(CartItem cartItem : cartItemList) {
			orderItemSet.add(new OrderItem(order, cartItem.getItem(), cartItem.getItem().getSellPrice(), new Long(cartItem.getCount())));
		}
		return orderItemSet;
	}
}
