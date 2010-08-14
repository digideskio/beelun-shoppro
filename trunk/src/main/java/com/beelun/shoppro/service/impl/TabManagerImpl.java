package com.beelun.shoppro.service.impl;

import java.util.Arrays;
import java.util.List;

import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.beelun.shoppro.dao.hibernate.GenericDaoHibernate;
import com.beelun.shoppro.model.Tab;
import com.beelun.shoppro.service.Tab2CategoryMapManager;
import com.beelun.shoppro.service.TabManager;

@Service(value = "tabManager")
public class TabManagerImpl extends GenericDaoHibernate<Tab, Long> implements TabManager {
	
	@Autowired
	Tab2CategoryMapManager t2cMapManager;
	
	@Autowired
	public TabManagerImpl(SessionFactory sessionFactory) {
		super(Tab.class, sessionFactory);
	}

	@SuppressWarnings("unchecked")
	@Override
	public List<Tab> getShownTabs() {
		return getHibernateTemplate().find(
		"from Tab t where t.isShown = true order by t.showOrder"); 
	}

	@SuppressWarnings("unchecked")
	@Override
	public Tab getTabByUrl(String urlName) {
		List<Tab> l = getHibernateTemplate().find(
		"from Tab t where t.url = ?", urlName);
		if(l != null && !l.isEmpty()) {
			return l.get(0);
		} else {
			return null;
		}
	}

	@SuppressWarnings("unchecked")
	@Override
	public boolean removeMany(Long[] idList) {
		log.debug("in remove by idList()...");
		if (idList != null) {
			try { 
				// Remove FK to this items and then delete the items
				t2cMapManager.purgeByTab(idList);
				List<Tab> l = (List<Tab>)getHibernateTemplate().findByNamedQueryAndNamedParam("TabManager.getByIdList",
						"idList", Arrays.asList(idList));				
				getHibernateTemplate().deleteAll(l);
				getHibernateTemplate().flush(); // Commit the change now
			} catch (Exception ex) {
				log.error(ex.getMessage());
				return false;
			}
		}
		return true;
	}

	@Override
	public boolean removeOne(Long id) {
		try {
			log.info("delete: " + id);
			// TODO: delete map first
			t2cMapManager.purgeByTab(new Long[]{id});
			remove(id);
		} catch (Exception ex) {
			log.error(ex.getMessage());
			return false;
		}
		return true;
	}
}
