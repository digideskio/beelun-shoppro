package com.beelun.shoppro.service.impl;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.hibernate.Criteria;
import org.hibernate.Query;
import org.hibernate.SessionFactory;
import org.hibernate.criterion.Criterion;
import org.hibernate.criterion.LogicalExpression;
import org.hibernate.criterion.Order;
import org.hibernate.criterion.Restrictions;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.beelun.shoppro.Constants;
import com.beelun.shoppro.dao.hibernate.GenericDaoHibernate;
import com.beelun.shoppro.model.Category;
import com.beelun.shoppro.model.Item;
import com.beelun.shoppro.pojo.MappingStatus;
import com.beelun.shoppro.pojo.MappingStatusEnum;
import com.beelun.shoppro.service.Category2ItemMapManager;
import com.beelun.shoppro.service.CategoryManager;
import com.beelun.shoppro.service.ItemManager;

@Service(value = "itemManager")
public class ItemManagerImpl extends GenericDaoHibernate<Item, Long> implements ItemManager {
    private transient final Log log = LogFactory.getLog(ItemManagerImpl.class);

    @Autowired
    Category2ItemMapManager c2iMapManager;

    @Autowired
    CategoryManager categoryManager;

    @Autowired
    public ItemManagerImpl(SessionFactory sessionFactory) {
        super(Item.class, sessionFactory);
    }

    @SuppressWarnings("unchecked")
    @Override
    public List<Item> getShown(Category category) {
        List<Item> l = null;
        if (category != null) {
            l = getHibernateTemplate().find("select i from Item i, Category2ItemMap c2i where i.isShown=true and i.id=c2i.item.id and c2i.category.id = ? order by c2i.itemOrder", category.getId());
        }

        return l;
    }

    @SuppressWarnings("unchecked")
    @Override
    public Item getItemByUrl(String itemUrl) {
        List<Item> l = getHibernateTemplate().find("from Item i where i.url = ?", itemUrl);
        if (l != null && !l.isEmpty()) {
            return l.get(0);
        } else {
            return null;
        }
    }

    @SuppressWarnings("unchecked")
    @Override
    public List<Item> getByIdList(List<Long> idList) {
        if (idList != null && !idList.isEmpty()) {
            return (List<Item>) getHibernateTemplate().findByNamedQueryAndNamedParam("ItemManager.getByIdList", "itemIdList", idList);
        } else {
            return null;
        }
    }

    @SuppressWarnings("unchecked")
    @Override
    public float getItemTotalValue(List<Long> idList) {
        log.debug("in getItemTotalValue()...");
        float retTotalValue = 0;
        if (idList != null && !idList.isEmpty()) {
            // TODO: Hibernate will return a double list for float type?
            List<BigDecimal> totalList = getHibernateTemplate().findByNamedQueryAndNamedParam("ItemManager.getItemTotalValue", "itemIdList", idList);
            if (totalList != null && !totalList.isEmpty()) {
                if (null != totalList.get(0)) {
                    return totalList.get(0).floatValue();
                } else {
                    return 0;
                }
            }
        }

        return retTotalValue;
    }

    /**
     * True successfully removed, false otherwise
     */
    @Override
    public boolean removeItems(Long[] idList) {
        log.debug("in remove by idList()...");
        if (idList != null) {
            try {
                // Remove FK to this items and then delete the items
                c2iMapManager.deleteByItems(idList);
                getHibernateTemplate().deleteAll(getByIdList(Arrays.asList(idList)));
                getHibernateTemplate().flush(); // Commit the change now
            } catch (Exception ex) {
                log.error(ex.getMessage());
                return false;
            }
        }
        return true;
    }

    @Override
    public void hide(Long id) {
        Item item = get(id);
        item.setIsShown(false);
        save(item);
    }

    /**
     * true - successfully removed, false otherwise
     */
    @Override
    public boolean removeItem(Long id) {
        try {
            log.info("delete: " + id);
            // TODO: delete map first
            c2iMapManager.deleteByItems(new Long[] { id });
            remove(id);
        } catch (Exception ex) {
            log.error(ex.getMessage());
            return false;
        }
        return true;
    }

    @Override
    public void show(Long id) {
        Item item = get(id);
        item.setIsShown(true);
        save(item);
    }

    @Override
    public void hide(Long[] idList) {
        for (Long id : idList) {
            hide(id);
        }
    }

    @Override
    public void show(Long[] idList) {
        for (Long id : idList) {
            show(id);
        }
    }

    @SuppressWarnings("unchecked")
    @Override
    public List<Item> getShownAll() {
        List<Item> l = null;
        l = getHibernateTemplate().find("select i from Item i where i.isShown=true");

        return l;
    }

    /**
     * Do this stupid thing to work around following erro: Data Access Failure
     * org.springframework.dao.InvalidDataAccessApiUsageException: Write
     * operations are not allowed in read-only mode (FlushMode.NEVER/MANUAL):
     * Turn your Session into FlushMode.COMMIT/AUTO or remove 'readOnly' marker
     * from transaction definition.
     */
    public Item save(Item item) {
        item.setUpdated(); // Set 'Updated' to current time
        return super.save(item);
    }

    /**
     * {@inheritDoc}
     */
    @SuppressWarnings("unchecked")
    @Override
    public Item getItemByNID(Long nid) {
        List<Item> l = getHibernateTemplate().find("from Item i where i.netSuiteId = ?", nid);
        if (l != null && !l.isEmpty()) {
            return l.get(0);
        } else {
            return null;
        }
    }

    @SuppressWarnings("unchecked")
    @Override
    public List<Item> getByCondition(String orderBy, boolean ascending, int firstResult, int maxResult, Long categoryId) {
        log.debug("in getByCondition()");
        log.debug("orderBy: " + orderBy);
        log.debug("ascending: " + ascending);
        log.debug("firstResult: " + firstResult);
        log.debug("maxResult: " + maxResult);
        log.debug("categoryId: " + categoryId);
        Query qry = null;
        if (categoryId >= 0) {
            // I have to use string conflation to get the result because:
            // (1) Passing "order by" to Query as parameter doesn't work
            // (2) Criteria doesn't support Category2ItemMap.item very well
            String queryStringFormat = null;
            if (ascending) {
                queryStringFormat = "select m.item from com.beelun.shoppro.model.Category2ItemMap m where m.category.id=(%s) order by m.item.%s asc";
            } else {
                queryStringFormat = "select m.item from com.beelun.shoppro.model.Category2ItemMap m where m.category.id=(%s) order by m.item.%s desc";
            }
            String queryString = String.format(queryStringFormat, categoryId, orderBy);
            qry = getHibernateTemplate().getSessionFactory().getCurrentSession().createQuery(queryString);
            if (maxResult != 0) {
                qry.setFirstResult(firstResult);
                qry.setMaxResults(maxResult);
            }
            return qry.list();
        } else if (categoryId.equals(Constants.ALL_CATEGORIES)) {
            // This branch can also be written in plain query. Use Criteria here
            // just for demo
            Criteria criteria = getHibernateTemplate().getSessionFactory().getCurrentSession().createCriteria(Item.class);
            if (ascending) {
                criteria.addOrder(Order.asc(orderBy));
            } else {
                criteria.addOrder(Order.desc(orderBy));
            }
            if (maxResult != 0) {
                criteria.setFirstResult(firstResult);
                criteria.setMaxResults(maxResult);
            }
            return criteria.list();
        } else if (categoryId.equals(Constants.UNCATEGORIED)) {
            String queryStringFormat = null;
            if (ascending) {
                queryStringFormat = "select item from com.beelun.shoppro.model.Item item where item not in (select m.item from com.beelun.shoppro.model.Category2ItemMap m) order by item.%s asc";
            } else {
                queryStringFormat = "select item from com.beelun.shoppro.model.Item item where item not in (select m.item from com.beelun.shoppro.model.Category2ItemMap m) order by item.%s desc";
            }
            String queryString = String.format(queryStringFormat, orderBy);
            qry = getHibernateTemplate().getSessionFactory().getCurrentSession().createQuery(queryString);
            if (maxResult != 0) {
                qry.setFirstResult(firstResult);
                qry.setMaxResults(maxResult);
            }
            return qry.list();
        } else {
            return null;
        }
    }

    @Override
    public int getAllCount() {
        final Query qry = getHibernateTemplate().getSessionFactory().getCurrentSession().createQuery("select count(id) from com.beelun.shoppro.model.Item");
        Object uniqueResult = qry.uniqueResult();
        if (null == uniqueResult) {
            return 0;
        } else {
            return ((Number) uniqueResult).intValue();
        }
    }

    @Override
    public int getCountByCondition(Long categoryId) {
        Query qry = null;
        if (categoryId >= 0) {
            qry = getHibernateTemplate().getSessionFactory().getCurrentSession().getNamedQuery("ItemManager.getCountByCondition");
        } else if (categoryId.equals(Constants.ALL_CATEGORIES)) {
            return getAllCount();
        } else if (categoryId.equals(Constants.UNCATEGORIED)) {
            qry = getHibernateTemplate().getSessionFactory().getCurrentSession().getNamedQuery("ItemManager.getCountByCondition_Uncategorized");
        }
        if (null != qry) {
            try {
                qry.setParameter("categoryId", categoryId);
            } catch (Exception ex) {}
            ; // Catch exceptions when there is no 'categoryId' parameter
            // available
            Object uniqueResult = qry.uniqueResult();
            if (null == uniqueResult) {
                return 0;
            } else {
                return ((Number) uniqueResult).intValue();
            }
        } else {
            return 0;
        }
    }

    /**
     * Return a search result based on name and metaTag only.
     * 
     * Enhancement: Several other options: (1) This can be also implemented use
     * MySQL build-in fulltext search which has no-so-good reputation for
     * performance. This requires myisam engine
     * http://p2p.wrox.com/book-beginning-mysql/32739-adding-fulltext-myisam-table-error.html
     * 
     * (2) We may consider move to hibernate search which is based on lucene:
     * https://www.hibernate.org/410.html
     */
    @SuppressWarnings("unchecked")
    @Override
    public List<Item> searchByText(String text, int firstResult, int maxResult) {
        log.debug("in searchByText()");
        log.debug("text: " + text);
        log.debug("firstResult: " + firstResult);
        log.debug("maxResult: " + maxResult);
        Criteria criteria = getHibernateTemplate().getSessionFactory().getCurrentSession().createCriteria(Item.class);
        Criterion metaTagC = Restrictions.sqlRestriction("upper(metaTag) like ('%" + text.toUpperCase() + "%')");
        Criterion nameC = Restrictions.sqlRestriction("upper(name) like ('%" + text.toUpperCase() + "%')");
        LogicalExpression orExp = Restrictions.or(nameC, metaTagC);
        criteria.add(orExp);
        // criteria.add(metaTagC);
        if (maxResult > 0) {
            criteria.setFirstResult(firstResult);
            criteria.setMaxResults(maxResult);
        }
        return criteria.list();
    }

    @Override
    public int searchByTextCount(String text) {
        final String metaTagQueryString = "upper(metaTag) like ('%" + text.toUpperCase() + "%')";
        final String nameQueryString = "upper(name) like ('%" + text.toUpperCase() + "%')";
        final Query qry = getHibernateTemplate().getSessionFactory().getCurrentSession().createQuery("select count(id) from com.beelun.shoppro.model.Item where " + metaTagQueryString + " or " + nameQueryString);
        Object uniqueResult = qry.uniqueResult();
        if (null == uniqueResult) {
            return 0;
        } else {
            return ((Number) uniqueResult).intValue();
        }
    }

    @Override
    public List<MappingStatus> getMappingStatus(List<Long> itemIdList) {
        List<MappingStatus> l = new ArrayList<MappingStatus>();

        // Get all categories
        List<Category> allCategories = categoryManager.getAll();

        // For each category, check whether all items belong to this category
        for (Category c : allCategories) {
            MappingStatus m = new MappingStatus();
            m.setId(c.getId());
            m.setName(c.getName());
            m.setMappingStatus(c2iMapManager.getMappingStatus(c.getId(), itemIdList));
            l.add(m);
        }

        // return
        return l;
    }

    @Override
    public boolean setMappingStatus(List<Long> itemIdList, Long categoryId, MappingStatusEnum mappingStatus) {
        if (mappingStatus == MappingStatusEnum.ALL) {
            for (Long itemId : itemIdList) {
                c2iMapManager.createMapIfNotExists(categoryId, itemId);
            }
        } else if (mappingStatus == MappingStatusEnum.NONE) {
            for (Long itemId : itemIdList) {
                c2iMapManager.deleteMapIfExist(categoryId, itemId);
            }
        }

        return true;
    }
}
