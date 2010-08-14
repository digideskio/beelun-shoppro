package com.beelun.shoppro.service.impl;

import java.util.List;

import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.beelun.shoppro.dao.hibernate.GenericDaoHibernate;
import com.beelun.shoppro.model.ExpressCorp;
import com.beelun.shoppro.service.ExpressCorpManager;

@Service(value = "expressCorpManager")
public class ExpressCorpManagerImpl extends GenericDaoHibernate<ExpressCorp, Long> implements ExpressCorpManager {

	@Autowired
	public ExpressCorpManagerImpl(SessionFactory sessionFactory) {
		super(ExpressCorp.class, sessionFactory);
	}

	@SuppressWarnings("unchecked")
	@Override
	public List getAllAvailable() {
		return getHibernateTemplate().find("from ExpressCorp ec where ec.enabled=true");
	}

}
