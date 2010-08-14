package com.beelun.shoppro.dao.hibernate;

import java.io.Serializable;
import java.util.ArrayList;
import java.util.Collection;
import java.util.Iterator;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.hibernate.Criteria;
import org.hibernate.Query;
import org.hibernate.SessionFactory;
import org.springframework.orm.hibernate3.HibernateTemplate;

import com.beelun.shoppro.dao.GenericDao;

/**
 * This class serves as the Base class for all other DAOs - namely to hold
 * common CRUD methods that they might all use. You should only need to extend
 * this class when your require custom CRUD logic.
 * 
 * @author bill@beelun.com
 * @param <T>
 *            a type variable
 * @param <PK>
 *            the primary key for that type
 */
public class GenericDaoHibernate<T, PK extends Serializable> implements GenericDao<T, PK> {
	/**
	 * Log variable for all child classes. Uses LogFactory.getLog(getClass())
	 * from Commons Logging
	 */
	protected final Log log = LogFactory.getLog(getClass());
	private Class<T> persistentClass;
	private HibernateTemplate hibernateTemplate;

	/**
	 * Constructor that takes in a class to see which type of entity to persist
	 * 
	 * @param persistentClass
	 *            the class type you'd like to persist
	 */
	public GenericDaoHibernate(final Class<T> persistentClass) {
		this.persistentClass = persistentClass;
	}

	/**
	 * Constructor with clazz and sessionFactory
	 * 
	 * @param persistentClass
	 * @param sessionFactory
	 */
	public GenericDaoHibernate(final Class<T> persistentClass, SessionFactory sessionFactory) {
		this.persistentClass = persistentClass;
		this.hibernateTemplate = new HibernateTemplate(sessionFactory);
	}

	/**
	 * {@inheritDoc}
	 */
	@SuppressWarnings("unchecked")
	public List<T> getAll() {
		return getHibernateTemplate().loadAll(this.persistentClass);
	}

	/**
	 * {@inheritDoc}
	 */
	@SuppressWarnings("unchecked")
	public List<T> getAllDistinct() {
		Collection result = new LinkedHashSet(getAll());
		return new ArrayList(result);
	}

	/**
	 * {@inheritDoc}
	 */
	@SuppressWarnings("unchecked")
	public T get(PK id) {
		if (null == id) {
			return null;
		}

		T entity = (T) getHibernateTemplate().get(this.persistentClass, id);

		if (entity == null) {
			log.warn("Uh oh, '" + this.persistentClass + "' object with id '" + id + "' not found...");
			// Bali: return null instead of throw exception
			// throw new ObjectRetrievalFailureException(this.persistentClass,
			// id);
		}

		return entity;
	}

	/**
	 * {@inheritDoc}
	 */
	@SuppressWarnings("unchecked")
	public boolean exists(PK id) {
		T entity = (T) getHibernateTemplate().get(this.persistentClass, id);
		return entity != null;
	}

	/**
	 * {@inheritDoc}
	 */
	@SuppressWarnings("unchecked")
	public T save(T object) {
		T ret = null;
		try {
			ret = (T) getHibernateTemplate().merge(object);
			getHibernateTemplate().flush(); // Save to DB right now to check any
			// violations
		} catch (Exception ex) {
			log.error("Fail to save, error msg: " + ex.getMessage());
			ret = null;
		}
		return ret;
	}

	/**
	 * {@inheritDoc}
	 */
	public void remove(PK id) {
		getHibernateTemplate().delete(this.get(id));
		getHibernateTemplate().flush();
	}

	/**
	 * {@inheritDoc}
	 */
	@SuppressWarnings("unchecked")
	public List<T> findByNamedQuery(String queryName, Map<String, Object> queryParams) {
		String[] params = new String[queryParams.size()];
		Object[] values = new Object[queryParams.size()];
		int index = 0;
		Iterator<String> i = queryParams.keySet().iterator();
		while (i.hasNext()) {
			String key = i.next();
			params[index] = key;
			values[index++] = queryParams.get(key);
		}
		return getHibernateTemplate().findByNamedQueryAndNamedParam(queryName, params, values);
	}

	public HibernateTemplate getHibernateTemplate() {
		return hibernateTemplate;
	}

	public void setHibernateTemplate(HibernateTemplate hibernateTemplate) {
		this.hibernateTemplate = hibernateTemplate;
	}

	@SuppressWarnings("unchecked")
	@Override
	public List<T> getByCondition(String orderBy, boolean ascending, int firstResult, int maxResult) {
		Criteria criteria = getHibernateTemplate().getSessionFactory().getCurrentSession().createCriteria(this.persistentClass);
		if (ascending) {
			criteria.addOrder(org.hibernate.criterion.Order.asc(orderBy));
		} else {
			criteria.addOrder(org.hibernate.criterion.Order.desc(orderBy));
		}
		// maxResult is set to 0, means return all
		if (maxResult != 0) {
			criteria.setFirstResult(firstResult);
			criteria.setMaxResults(maxResult);
		}
		return criteria.list();
	}

	@Override
	public int getAllCount() {
		final Query qry = getHibernateTemplate().getSessionFactory().getCurrentSession().createQuery("select count(id) from " + this.persistentClass.getName());
		Object uniqueResult = qry.uniqueResult();
		if (null == uniqueResult) {
			return 0;
		} else {
			return ((Number) uniqueResult).intValue();
		}
	}
}
