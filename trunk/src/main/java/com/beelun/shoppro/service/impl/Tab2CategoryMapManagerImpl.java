package com.beelun.shoppro.service.impl;

import java.util.List;

import org.hibernate.Query;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.beelun.shoppro.dao.hibernate.GenericDaoHibernate;
import com.beelun.shoppro.model.Category;
import com.beelun.shoppro.model.Tab;
import com.beelun.shoppro.model.Tab2CategoryMap;
import com.beelun.shoppro.pojo.MappingStatusEnum;
import com.beelun.shoppro.service.CategoryManager;
import com.beelun.shoppro.service.Tab2CategoryMapManager;
import com.beelun.shoppro.service.TabManager;

@Service(value = "tab2CategoryMapManager")
public class Tab2CategoryMapManagerImpl extends GenericDaoHibernate<Tab2CategoryMap, Long> implements Tab2CategoryMapManager {

	@Autowired
	TabManager tabManager;

	@Autowired
	CategoryManager categoryManager;

	@Autowired
	public Tab2CategoryMapManagerImpl(SessionFactory sessionFactory) {
		super(Tab2CategoryMap.class, sessionFactory);
	}
	
	/**
	 * {@inheritDoc} 
	 */
	public Tab2CategoryMap saveLazy(Tab2CategoryMap t2c)
	{
		if(t2c != null) {
			// Get Tab and Category
			Long tabId = t2c.getTab().getId();
			Tab tab = tabManager.get(tabId);
			t2c.setTab(tab);
			
			Long categoryId = t2c.getCategory().getId();
			Category category = categoryManager.get(categoryId);
			t2c.setCategory(category);
			
			// Save
			return save(t2c);		
		} else {
			return null;
		}
	}

	@SuppressWarnings("unchecked")
	@Override
	public Tab2CategoryMap createMapIfNotExists(Tab tab, Category category) {
		List<Tab2CategoryMap> l = getHibernateTemplate().find(
				"from com.beelun.shoppro.model.Tab2CategoryMap as t2c where t2c.tab.id = (?) and t2c.category.id = (?)", new Object[] {tab.getId(), category.getId()});				
		if (l != null && !l.isEmpty()) {
			return l.get(0);
		} else {
			Tab2CategoryMap t2c = new Tab2CategoryMap();
			t2c.setTab(tab);
			t2c.setCategory(category);
			return this.save(t2c);
		}		
	}

	//@Override
	public boolean purgeByCategory(Long[] categoryIdList) {
		if(null != categoryIdList && categoryIdList.length != 0) {	
			// TODO: this is bad in perf. Use this temporarily before figuring out buildUpdate things
			for(Long theId: categoryIdList) {
				String queryString = "delete com.beelun.shoppro.model.Tab2CategoryMap m where m.category.id in (:theId)";
				Query qry = getHibernateTemplate().getSessionFactory().getCurrentSession().createQuery(queryString);
				qry.setParameter("theId", theId);
				qry.executeUpdate();
			}
		}
		
		// Always true
		return true;
	}

	@Override
	public boolean purgeByTab(Long[] tabIdList) {
		if(null != tabIdList && tabIdList.length != 0) {	
			// TODO: this is bad in perf. Use this temporarily before figuring out buildUpdate things
			for(Long theId: tabIdList) {
				String queryString = "delete com.beelun.shoppro.model.Tab2CategoryMap m where m.tab.id in (:theId)";
				Query qry = getHibernateTemplate().getSessionFactory().getCurrentSession().createQuery(queryString);
				qry.setParameter("theId", theId);
				qry.executeUpdate();
			}
		}
		
		// Always true
		return true;
	}

	@SuppressWarnings("unchecked")
	@Override
	public Tab2CategoryMap createMapIfNotExists(Long tabId, Long categoryId) {
		List<Tab2CategoryMap> l = getHibernateTemplate()
				.find(
						"from com.beelun.shoppro.model.Tab2CategoryMap as m where m.tab.id = (?) and m.category.id = (?)",
						new Object[] { tabId, categoryId });
		if (l != null && !l.isEmpty()) {
			return l.get(0);
		} else {
			Tab2CategoryMap m = new Tab2CategoryMap();
			m.setTab(tabManager.get(tabId));
			m.setCategory(categoryManager.get(categoryId));
			return this.save(m);
		}
	}

	@SuppressWarnings("unchecked")
	@Override
	public MappingStatusEnum getMappingStatus(Long tabId, List<Long> categoryIdList) {
		if (categoryIdList != null && categoryIdList.size() != 0) {
			List<Long> l = getHibernateTemplate()
					.findByNamedQueryAndNamedParam(
							"T2CMapManager.getCategoryIdByTabIdAndCategoryList", new String[] {"tabId","categoryIdList"}, new Object[]{tabId, categoryIdList});
			int countOfInputtedCategories = categoryIdList.size();
			int countOfMappedCategories = 0;
			if(l != null) {
				countOfMappedCategories = l.size();
			}
			if (countOfInputtedCategories == countOfMappedCategories) { // We should make sure there is no dup maps inside
				return MappingStatusEnum.ALL;
			} else if (countOfMappedCategories == 0) {
				return MappingStatusEnum.NONE;
			} else {
				return MappingStatusEnum.PARTIAL;
			}
		} else {
			return MappingStatusEnum.NONE;
		}
	}

	@Override
	public boolean deleteMapIfExist(Long tabId, Long categoryId) {
		String queryString = "delete com.beelun.shoppro.model.Tab2CategoryMap m where m.category.id = (:categoryId) and m.tab.id = (:tabId)";
		Query qry = getHibernateTemplate().getSessionFactory()
				.getCurrentSession().createQuery(queryString);
		qry.setParameter("tabId", tabId);
		qry.setParameter("categoryId", categoryId);
		qry.executeUpdate();
		
		return true;
	}
}
