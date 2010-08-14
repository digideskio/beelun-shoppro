package com.beelun.shoppro.service.impl;

import java.util.List;

import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.beelun.shoppro.dao.hibernate.GenericDaoHibernate;
import com.beelun.shoppro.model.PaypalAccessInfo;
import com.beelun.shoppro.service.PaypalAccessInfoManager;

@Service(value = "paypalAccessInfoManager")
public class PaypalAccessInfoManagerImpl extends GenericDaoHibernate<PaypalAccessInfo, Long>
		implements PaypalAccessInfoManager {
	// A cache for myGlob
	private PaypalAccessInfo paypalAccessInfo = null;

	@Autowired
	public PaypalAccessInfoManagerImpl(SessionFactory sessionFactory) {
		super(PaypalAccessInfo.class, sessionFactory); 
	}

	@Override
	public PaypalAccessInfo fetch() {
		if(this.paypalAccessInfo == null) {
			List<PaypalAccessInfo> l = super.getAll();
			if(l != null && !l.isEmpty()) {
				this.paypalAccessInfo = l.get(0);
			} else {
				this.paypalAccessInfo = null;
			}
		}
		
		return this.paypalAccessInfo;
	}

	@Override
	public PaypalAccessInfo save(PaypalAccessInfo paypalAccessInfo) {
		this.paypalAccessInfo = null;  
		return super.save(paypalAccessInfo);
	}

}
