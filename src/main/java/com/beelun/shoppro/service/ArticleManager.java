package com.beelun.shoppro.service;

import java.util.List;
import com.beelun.shoppro.model.Article;

/**
 * Manager static content such as articles
 * 
 * @author bali
 *
 */
public interface ArticleManager {
	public Article get(Long id);
	public Article save(Article article);
	public boolean removeOne(Long id); 
	public boolean removeMany(Long[] idList); 
	public void show(Long id);
	public void hide(Long id);
	public List<Article> getShownAll();	
	public List<Article> getAll();
}
