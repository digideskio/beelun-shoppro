package com.beelun.shoppro.service.impl;

import java.util.List;

import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.beelun.shoppro.dao.hibernate.GenericDaoHibernate;
import com.beelun.shoppro.model.PaymentTool;
import com.beelun.shoppro.service.PaymentToolManager;

@Service(value = "paymentToolManager")
public class PaymentToolManagerImpl extends GenericDaoHibernate<PaymentTool, Long> implements PaymentToolManager {

	@Autowired
	public PaymentToolManagerImpl(SessionFactory sessionFactory) {
		super(PaymentTool.class, sessionFactory);
	}

	@SuppressWarnings("unchecked")
	@Override
	public List getAllAvailable() {
		return getHibernateTemplate().find("from PaymentTool pt where pt.enabled=true");
	}
}
