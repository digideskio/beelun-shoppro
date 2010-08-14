package com.beelun.shoppro.service.impl;

import java.util.List;

import org.hibernate.Query;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.beelun.shoppro.dao.hibernate.GenericDaoHibernate;
import com.beelun.shoppro.model.Category;
import com.beelun.shoppro.model.Category2ItemMap;
import com.beelun.shoppro.model.Item;
import com.beelun.shoppro.pojo.MappingStatusEnum;
import com.beelun.shoppro.service.Category2ItemMapManager;
import com.beelun.shoppro.service.CategoryManager;
import com.beelun.shoppro.service.ItemManager;

@Service(value = "category2ItemMapManager")
public class Category2ItemMapManagerImpl extends
		GenericDaoHibernate<Category2ItemMap, Long> implements
		Category2ItemMapManager {

	@Autowired
	CategoryManager categoryManager;

	@Autowired
	ItemManager itemManager;

	@Autowired
	public Category2ItemMapManagerImpl(SessionFactory sessionFactory) {
		super(Category2ItemMap.class, sessionFactory);
	}

	@Override
	public Category2ItemMap save(Category2ItemMap c2i) {
		return super.save(c2i);
	}

	/**
	 * {@inheritDoc}
	 */
	@Override
	public Category2ItemMap saveLazy(Category2ItemMap c2i) {
		if (c2i != null) {
			// Get Item and Category
			Long categoryId = c2i.getCategory().getId();
			Category category = categoryManager.get(categoryId);
			c2i.setCategory(category);

			Long tabId = c2i.getItem().getId();
			Item item = itemManager.get(tabId);
			c2i.setItem(item);

			// Save
			return save(c2i);
		} else {
			return null;
		}
	}

	@Override
	public Category2ItemMap createMapIfNotExists(Category c, Item i) {
		return createMapIfNotExists(c.getId(), i.getId());
	}

	@SuppressWarnings("unchecked")
	@Override
	public Category2ItemMap createMapIfNotExists(Long categoryId, Long itemId) {
		List<Category2ItemMap> l = getHibernateTemplate()
				.find(
						"from com.beelun.shoppro.model.Category2ItemMap as m where m.category.id = (?) and m.item.id = (?)",
						new Object[] { categoryId, itemId });
		if (l != null && !l.isEmpty()) {
			return l.get(0);
		} else {
			Category2ItemMap m = new Category2ItemMap();
			m.setCategory(categoryManager.get(categoryId));
			m.setItem(itemManager.get(itemId));
			return this.save(m);
		}
	}

	@Override
	public boolean deleteByItems(Long[] itemIdList) {
		if (null != itemIdList && itemIdList.length != 0) {
			// TODO: this is bad in perf. Use this temporarily before figuring
			// out buildUpdate things
			for (Long theId : itemIdList) {
				String queryString = "delete com.beelun.shoppro.model.Category2ItemMap m where m.item.id in (:theId)";
				Query qry = getHibernateTemplate().getSessionFactory()
						.getCurrentSession().createQuery(queryString);
				qry.setParameter("theId", theId);
				qry.executeUpdate();
			}
		}

		// Always true
		return true;
	}

	@Override
	public boolean deleteByCategories(Long[] categoryIdList) {
		if (null != categoryIdList && categoryIdList.length != 0) {
			// TODO: this is bad in perf. Use this temporarily before figuring
			// out buildUpdate things
			for (Long theId : categoryIdList) {
				String queryString = "delete com.beelun.shoppro.model.Category2ItemMap m where m.category.id in (:theId)";
				Query qry = getHibernateTemplate().getSessionFactory()
						.getCurrentSession().createQuery(queryString);
				qry.setParameter("theId", theId);
				qry.executeUpdate();
			}
		}

		// Always true
		return true;
	}

	@SuppressWarnings("unchecked")
	@Override
	public MappingStatusEnum getMappingStatus(Long categoryId, List<Long> itemIdList) {
		if (itemIdList != null && itemIdList.size() != 0) {
			List<Long> l = getHibernateTemplate()
					.findByNamedQueryAndNamedParam(
							"C2IMapManager.getItemIdByCategoryIdAndItemList", new String[] {"categoryId","itemIdList"}, new Object[]{categoryId, itemIdList});
			int countOfInputtedItems = itemIdList.size();
			int countOfMappedItems = 0;
			if(l != null) {
				countOfMappedItems = l.size();
			}
			if (countOfInputtedItems == countOfMappedItems) { // We should make sure there is no dup maps inside
				return MappingStatusEnum.ALL;
			} else if (countOfMappedItems == 0) {
				return MappingStatusEnum.NONE;
			} else {
				return MappingStatusEnum.PARTIAL;
			}
		} else {
			return MappingStatusEnum.NONE;
		}
	}

	@Override
	public boolean deleteMapIfExist(Long categoryId, Long itemId) {
		String queryString = "delete com.beelun.shoppro.model.Category2ItemMap m where m.category.id = (:categoryId) and m.item.id = (:itemId)";
		Query qry = getHibernateTemplate().getSessionFactory()
				.getCurrentSession().createQuery(queryString);
		qry.setParameter("categoryId", categoryId);
		qry.setParameter("itemId", itemId);
		qry.executeUpdate();

		return true;
	}

}
