package com.beelun.shoppro.pojo;

import com.beelun.shoppro.model.Address;
import com.beelun.shoppro.model.Order;
import com.beelun.shoppro.model.User;

/**
 * Wrapper class for customer/input-address.html page
 * We save shippingAddress/billingAddress to User/Order respectively because user might change address after place certain order
 * 
 * @author bali
 *
 */
public class AddressWrapper {
	private Address shippingAddress = new Address();
	private Address billingAddress = new Address();
	private boolean sameAddress = true;
	
	/**
	 * Construct a object from user
	 * Also make sure that 
	 * (1) billingAddress and shippingAddress is not null
	 * (2) they are not refer to the same object
	 * @param user
	 */
	public AddressWrapper(User user) {
		if(user.getShippingAddress() == null) {
			this.shippingAddress = new Address();
		} else {
			this.shippingAddress.getValuesFromAnotherAddress(user.getShippingAddress());
		}
		
		if(this.getBillingAddress() == null) {
			this.billingAddress = new Address();
		} else {
			this.billingAddress.getValuesFromAnotherAddress(user.getBillingAddress());				
		}
		this.sameAddress = user.isSameAddress();
	}
	
	//
	// Public methods
	//
	
	/**
	 * Put isSameAddress, shippingAddress and billingAddress to user
	 */
	public void putToUser(User user) {
		user.setSameAddress(this.sameAddress);
		if(user.getShippingAddress() == null) {
			user.setShippingAddress(this.getShippingAddress());
		} else {
			user.getShippingAddress().getValuesFromAnotherAddress(this.getShippingAddress());
		}

		if(this.sameAddress == false) {
			if(user.getBillingAddress() == null) {
				user.setBillingAddress(this.getBillingAddress());
			} else {
				user.getBillingAddress().getValuesFromAnotherAddress(this.getBillingAddress());
			}		
		}
	}
	
	/**
	 * Put isSameAddress, shippingAddress and billingAddress to order 
	 * @param order
	 */
	public void putToOrder(Order order) {
		order.setSameAddress(this.sameAddress);
		if(order.getShippingAddress() == null) {
			order.setShippingAddress(this.getShippingAddress());
		} else {
			order.getShippingAddress().getValuesFromAnotherAddress(this.getShippingAddress());
		}

		if(this.sameAddress == false) {
			if(order.getBillingAddress() == null) {
				order.setBillingAddress(this.getBillingAddress());
			} else {
				order.getBillingAddress().getValuesFromAnotherAddress(this.getBillingAddress());
			}		
		}		
	}
	
	// TODO: add verification
	
	//
	// Getters and setters
	//
	public Address getShippingAddress() {
		return shippingAddress;
	}
	public void setShippingAddress(Address shippingAddress) {
		this.shippingAddress = shippingAddress;
	}
	public Address getBillingAddress() {
		return billingAddress;
	}
	public void setBillingAddress(Address billingAddress) {
		this.billingAddress = billingAddress;
	}
	public boolean isSameAddress() {
		return sameAddress;
	}
	public void setSameAddress(boolean isSameAddress) {
		this.sameAddress = isSameAddress;
	}
}
