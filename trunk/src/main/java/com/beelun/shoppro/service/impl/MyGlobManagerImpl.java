package com.beelun.shoppro.service.impl;

import java.util.List;

import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.beelun.shoppro.dao.hibernate.GenericDaoHibernate;
import com.beelun.shoppro.model.MyGlob;
import com.beelun.shoppro.service.MyGlobManager;
import com.beelun.shoppro.service.TabManager;

/**
 * Manage all the variables used across the site
 * 
 * @author Bill Li(bill@beelun.com)
 *
 */
@Service(value = "myGlobManager")
public class MyGlobManagerImpl extends GenericDaoHibernate<MyGlob, Long> implements MyGlobManager {
	// A cache for myGlob
	private MyGlob myGlob = null;
	
	@Autowired
	public MyGlobManagerImpl(SessionFactory sessionFactory) {
		super(MyGlob.class, sessionFactory); 
	}
	
	@Autowired
	TabManager tabManager;

	@Override
	public MyGlob fetch() {
		if(this.myGlob == null) {
			List<MyGlob> l = super.getAll();
			if(l != null && !l.isEmpty()) {
				MyGlob myGlob = l.get(0);
				myGlob.setShownTabs(tabManager.getShownTabs());
				this.myGlob = l.get(0);
			} else {
				this.myGlob = null;
			}
		}
		
		return this.myGlob;
	}

	@Override
	public MyGlob save(MyGlob myGlob) {
		// invalidate the cache
		// NB: this might result in bad page due to threading issue, but refresh
		//     the page should be able to fix that. So we won't worry about that now
		this.myGlob = null;  
		return super.save(myGlob);
	}

}
