package com.beelun.shoppro.service;

import java.util.List;

import com.beelun.shoppro.model.Category;
import com.beelun.shoppro.model.Category2ItemMap;
import com.beelun.shoppro.model.Item;
import com.beelun.shoppro.pojo.MappingStatusEnum;

public interface Category2ItemMapManager {
	
	/**
	 * This method will return all data which is not suitable for remote access
	 * The object list is gonna big
	 * 
	 * @return
	 */
	public List<Category2ItemMap> getAll();
	public Category2ItemMap save(Category2ItemMap c2i);
	
	/**
	 * Once upon a time, there is a error when update web service in VS2008:
	 * Custom tool error: Failed to generate code for the service reference 
	 * 'Beelun.Shoppro.WebService.Category2ItemMapManager'.  
	 * Please check other error and warning messages for details.	
	 * F:\shoppro\src\adminConsole\HappyDog\HappyDog.SL\Service References\Beelun.Shoppro.WebService.Category2ItemMapManager\Reference.svcmap
	 * 
	 * Still don't know why. But here are two work around ways:
	 * (1) Generate a wrapper for this manager just like UserManager
	 * (2) Change the method name, etc. 
	 * 
	 * I change method name removeByItems -> deleteByItems, and it works.  
	 * 
	 * @param itemIdList
	 * @return
	 */
	public boolean deleteByItems(Long[] itemIdList);	
	public boolean deleteByCategories(Long[] categoryIdList);
	public boolean deleteMapIfExist(Long categoryId, Long itemId);
	
	/**
	 * Save one object which is partially filled to db. 
	 * @param c2i
	 * 		The category and item object only contain id field. Other fields are not filled.
	 * @return
	 * 		Valid fully filled object
	 */
	public Category2ItemMap saveLazy(Category2ItemMap c2i);	
	public Category2ItemMap createMapIfNotExists(Category c, Item i);
	public Category2ItemMap createMapIfNotExists(Long categoryId, Long itemId);
	public MappingStatusEnum getMappingStatus(Long categoryId, List<Long> itemIdList);
	
}
