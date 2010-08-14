package com.beelun.shoppro.service.impl;

import java.util.Arrays;
import java.util.List;

import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.beelun.shoppro.Constants;
import com.beelun.shoppro.dao.hibernate.GenericDaoHibernate;
import com.beelun.shoppro.model.Brand;
import com.beelun.shoppro.service.BrandManager;

@Service(value = "brandManager")
public class BrandManagerImpl extends GenericDaoHibernate<Brand, Long> implements
		BrandManager {

	@Autowired
	public BrandManagerImpl(SessionFactory sessionFactory) {
		super(Brand.class, sessionFactory);
	}
	
	@SuppressWarnings("unchecked")
	@Override
	public Brand getByName(String name) {
		List<Brand> l = getHibernateTemplate().find(
				"from Brand b where b.name = ?", name);
		if (l != null && !l.isEmpty()) {
			return l.get(0);
		} else {
			return null;
		}		
	}

	@SuppressWarnings("unchecked")
	@Override
	public Brand getOrCreateByName(String name) {
		// Case insensitive
		List<Brand> l = getHibernateTemplate().find(
				"from Brand b where lower(b.name) = lower(?)", name);
		
		if (l != null && !l.isEmpty()) {
			return l.get(0);
		} else {
			Brand brand = new Brand();
			brand.setName(name);
			return save(brand);
		}		
	}
	
	public Brand save(Brand brand) {
		return super.save(brand);
	}

	@Override
	public Brand getNoBrand() {
		return this.get(Constants.NO_BRAND_ID);
	}

	@SuppressWarnings("unchecked")
	@Override
	public boolean removeMany(Long[] idList) {
		log.debug("in remove by idList()...");
		if (idList != null && idList.length != 0) {
			try { 
				// Remove FK to these guys and then delete them
				List<Brand> l = (List<Brand>)getHibernateTemplate().findByNamedQueryAndNamedParam("BrandManager.getByIdList",
						"idList", Arrays.asList(idList));								
				getHibernateTemplate().deleteAll(l);
				getHibernateTemplate().flush(); // Commit the change now
			} catch (Exception ex) {
				// In most cases, there is because of FK issues.
				log.error(ex.getMessage());
				return false;
			}
		}
		return true;
	}

}
