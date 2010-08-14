package com.beelun.shoppro.service.impl;

import java.util.List;

import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.beelun.shoppro.dao.hibernate.GenericDaoHibernate;
import com.beelun.shoppro.model.Role;
import com.beelun.shoppro.service.RoleManager;

@Service(value = "roleManager")
public class RoleManagerImpl extends GenericDaoHibernate<Role, Long> implements
		RoleManager {

	@Autowired
	public RoleManagerImpl(SessionFactory sessionFactory) {
		super(Role.class, sessionFactory);
	}

	@SuppressWarnings("unchecked")
	@Override
	public Role getCustomerRole() {
		List<Role> l = getHibernateTemplate().find("from Role r where r.name='ROLE_CUSTOMER'");
		if(l != null && !l.isEmpty()) {
			return l.get(0);
		} else {
			return null;
		}
	}

	@SuppressWarnings("unchecked")
	@Override
	public Role getAdminRole() {
		List<Role> l = getHibernateTemplate().find("from Role r where r.name='ROLE_ADMIN'");
		if(l != null && !l.isEmpty()) {
			return l.get(0);
		} else {
			return null;
		}
	}

}
