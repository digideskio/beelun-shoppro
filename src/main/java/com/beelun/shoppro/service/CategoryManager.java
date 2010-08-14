package com.beelun.shoppro.service;

import java.util.List;

import com.beelun.shoppro.model.Brand;
import com.beelun.shoppro.model.Category;
import com.beelun.shoppro.model.Tab;
import com.beelun.shoppro.pojo.MappingStatus;
import com.beelun.shoppro.pojo.MappingStatusEnum;

/**
 * Class to manage Category
 * 
 * @author Bill Li(bill@beelun.com)
 *
 */                                                
public interface CategoryManager {
	public Category get(Long id);
	public Category save(Category category);
	
	/**
	 * Remove one category
	 * @param id 
	 * 		id of the category
	 * @return
	 */
	public boolean removeOne(Long id);
	
	/**
	 * Remove many categories
	 * @param idList 
	 * 		id list of the categories
	 * @return
	 */
	public boolean removeMany(Long[] idList);
	public List<Category> getShown(Tab tab);
	public List<Category> getShownAll();
	public List<Category> getAll();
	public Category getCategoryByUrl(String categoryUrl);
	public Category getOrCreateCategoryFromBrand(Brand brand);
	/**
	 * Get a list of category according to certain condition
	 * @param orderBy
	 * @param ascending
	 * @param firstResult
	 * @param maxResult
	 *     		0 means result all, otherwise, apply normally
	 * @param categoryId 
	 * 			>=0 works normally; -1 means 'all categories', -2 means 'uncategorized'
	 * @return
	 */
	public List<Category> getByCondition(String orderBy, boolean ascending, int firstResult, int maxResult);
	
	/**
	 * Get specified categories' mapping status to tab
	 * 
	 * @param idList
	 * @return
	 */
	public List<MappingStatus> getMappingStatus(List<Long> idList);
		
	/**
	 * Set category list to tab mapping status. It can interpreted as followings:
	 * If mappingStatus==NONE, "remove all these categories from the tab"
	 * If mappingStatus==ALL, "Add all these categories to the tab"
	 * 
	 * @param categoryIdList
	 * @param tabId
	 * @param mappingStatus
	 * 			Should be NONE or ALL. Will do nothing if PARTIAL.
	 * @return
	 */
	public boolean setMappingStatus(List<Long> categoryIdList, Long tabId, MappingStatusEnum mappingStatus);	
}
