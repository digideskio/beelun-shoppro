package com.beelun.shoppro.service.impl;

import java.util.Arrays;
import java.util.List;

import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.beelun.shoppro.dao.hibernate.GenericDaoHibernate;
import com.beelun.shoppro.model.Article;
import com.beelun.shoppro.service.ArticleManager;

@Service(value = "articleManager")
public class ArticleManagerImpl extends GenericDaoHibernate<Article, Long> implements
		ArticleManager {

	@Autowired
	public ArticleManagerImpl(SessionFactory sessionFactory) {
		super(Article.class, sessionFactory);
	}
	
	@Override
	public void hide(Long id) {
		Article article = get(id);
		article.setIsShown(false);
		save(article);
	}

	@Override
	public void show(Long id) {
		Article article = get(id);
		article.setIsShown(true);
		save(article);
	}

	@SuppressWarnings("unchecked")
	@Override
	public List<Article> getShownAll() {
		List<Article> l = null;
		l = getHibernateTemplate().find(
				"select a from Article a where a.isShown=true");

		return l;
	}
	
	/**
	 * true - successfully removed, false otherwise
	 */
	@Override
	public boolean removeOne(Long id) {
		try {
			log.info("delete: " + id);
			// TODO: delete map first
			remove(id);
		} catch (Exception ex) {
			log.error(ex.getMessage());
			return false;
		}
		return true;
	}
	
	@Override
	public boolean removeMany(Long[] idList) {
		log.debug("in remove by idList()...");
		if (idList != null) {
			try { 
				// Remove FK to this items and then delete the items
				getHibernateTemplate().deleteAll(getByIdList(Arrays.asList(idList)));
				getHibernateTemplate().flush(); // Commit the change now
			} catch (Exception ex) {
				log.error(ex.getMessage());
				return false;
			}
		}
		return true;		
	}
	
	@SuppressWarnings("unchecked")
	private List<Article> getByIdList(List<Long> idList) {
		if (idList != null && !idList.isEmpty()) {
			return (List<Article>) getHibernateTemplate()
					.findByNamedQueryAndNamedParam("ArticleManager.getByIdList",
							"itemIdList", idList);
		} else {
			return null;
		}
	}	

}
