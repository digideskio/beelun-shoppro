package com.beelun.shoppro.service.impl;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

import org.apache.commons.lang.RandomStringUtils;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.beelun.shoppro.dao.hibernate.GenericDaoHibernate;
import com.beelun.shoppro.model.Brand;
import com.beelun.shoppro.model.Category;
import com.beelun.shoppro.model.Tab;
import com.beelun.shoppro.pojo.MappingStatus;
import com.beelun.shoppro.pojo.MappingStatusEnum;
import com.beelun.shoppro.service.Category2ItemMapManager;
import com.beelun.shoppro.service.CategoryManager;
import com.beelun.shoppro.service.Tab2CategoryMapManager;
import com.beelun.shoppro.service.TabManager;
import com.beelun.shoppro.utils.GeneralUtils;

@Service(value = "categoryManager")
public class CategoryManagerImpl extends GenericDaoHibernate<Category, Long> implements CategoryManager {

	@Autowired
	Category2ItemMapManager c2iMapManager;

	@Autowired
	Tab2CategoryMapManager t2cMapManager;

	@Autowired
	TabManager tabManager;

	@Autowired
	public CategoryManagerImpl(SessionFactory sessionFactory) {
		super(Category.class, sessionFactory);
	}

	/**
	 * Get all categories which should show under this tab
	 */
	@SuppressWarnings("unchecked")
	@Override
	public List<Category> getShown(Tab tab) {
		List<Category> l = getHibernateTemplate()
				.find(
						"select c from Category c, Tab2CategoryMap t2c where c.isShown=true and c.id=t2c.category.id and t2c.tab.id = ? order by t2c.categoryOrder",
						tab.getId());
		return l;
	}

	@SuppressWarnings("unchecked")
	@Override
	public Category getCategoryByUrl(String categoryUrl) {
		List<Category> l = getHibernateTemplate().find(
				"from Category c where c.url = ?", categoryUrl);
		if (l != null && !l.isEmpty()) {
			return l.get(0);
		} else {
			return null;
		}
	}

	/**
	 * Get all categories which should be shown in the site regardless the tab
	 */
	@SuppressWarnings("unchecked")
	@Override
	public List<Category> getShownAll() {
		List<Category> l = getHibernateTemplate().find(
				"select c from Category c where c.isShown=true");
		return l;
	}

	@SuppressWarnings("unchecked")
	@Override
	public Category getOrCreateCategoryFromBrand(Brand brand) {
		// Case insensitive
		List<Category> l = getHibernateTemplate().find(
				"from Category c where lower(c.name) = lower(?)", brand.getName().trim());
		if (l != null && !l.isEmpty()) {
			return l.get(0);
		} else {
			Category c = new Category();
			c.setName(brand.getName().trim());
			// Set not null fields
			c.setPageTitle(brand.getName());
			String url = GeneralUtils.toDescriptiveUrl(brand.getName());
			if(null == url) {
				url = RandomStringUtils.random(64, true, true);
			}
			c.setUrl(url);
			return this.save(c);
		}
	}
	
	@Override
	public Category save(Category category) {
		Category c = null;
		try {
			category.setUpdated();
			c = super.save(category);
		}catch(Exception ex) { // Catch all exception. Ugly, but works. Polish it later on.
			log.error(ex.getMessage());
		}
		
		return c;
	}

	@Override
	public List<Category> getByCondition(String orderBy, boolean ascending, int firstResult, int maxResult) {
		return super.getByCondition(orderBy, ascending, firstResult, maxResult);
	}

	@SuppressWarnings("unchecked")
	@Override
	public boolean removeMany(Long[] idList) {
		log.debug("in remove by idList()...");
		if (idList != null && idList.length != 0) {
			try { 
				// Remove FK to these guys and then delete them
				c2iMapManager.deleteByCategories(idList);				
				t2cMapManager.purgeByCategory(idList);
				List<Category> l = (List<Category>)getHibernateTemplate().findByNamedQueryAndNamedParam("CategoryManager.getByIdList",
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
			c2iMapManager.deleteByCategories(new Long[]{id});
			t2cMapManager.purgeByCategory(new Long[]{id});
			remove(id);
		} catch (Exception ex) {
			log.error(ex.getMessage());
			return false;
		}
		return true;
	}

	@Override
	public List<MappingStatus> getMappingStatus(List<Long> categoryIdList) {
		List<MappingStatus> l = new ArrayList<MappingStatus>();
		
		// Get all tabs
		List<Tab> allTabs = tabManager.getAll();
				
		// For each tab, check whether all categories belong to it
		for(Tab t : allTabs) {
			MappingStatus m = new MappingStatus();
			m.setId(t.getId());
			m.setName(t.getName());
			m.setMappingStatus(t2cMapManager.getMappingStatus(t.getId(), categoryIdList));
			l.add(m);
		}

		// return
		return l;
	}

	@Override
	public boolean setMappingStatus(List<Long> categoryIdList, Long tabId,
			MappingStatusEnum mappingStatus) {
		if(mappingStatus == MappingStatusEnum.ALL) {
			for(Long categoryId : categoryIdList) {
				t2cMapManager.createMapIfNotExists(tabId, categoryId);
			}
		} else if (mappingStatus == MappingStatusEnum.NONE) {
			for(Long categoryId : categoryIdList) {
				t2cMapManager.deleteMapIfExist(tabId, categoryId);
			}
		}
		
		return true;
	}

}
