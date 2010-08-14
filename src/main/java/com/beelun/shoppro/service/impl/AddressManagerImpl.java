package com.beelun.shoppro.service.impl;

import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.beelun.shoppro.dao.hibernate.GenericDaoHibernate;
import com.beelun.shoppro.model.Address;
import com.beelun.shoppro.service.AddressManager;

@Service(value = "addressManager")
public class AddressManagerImpl extends GenericDaoHibernate<Address, Long>
		implements AddressManager {

	@Autowired
	public AddressManagerImpl(SessionFactory sessionFactory) {
		super(Address.class, sessionFactory);
	}
	
	public Address save(Address address) {
		return super.save(address);
	}
}
