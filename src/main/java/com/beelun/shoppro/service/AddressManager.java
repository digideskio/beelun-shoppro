package com.beelun.shoppro.service;

import com.beelun.shoppro.model.Address;

/**
 * Manage 'Address' model object
 *
 * @author <a href="mailto:bill@beelun.com">Bill Li</a>
 *
 */
public interface AddressManager {
	public Address get(Long addressId);
	public Address save(Address address);
}
