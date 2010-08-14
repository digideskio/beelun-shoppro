package com.beelun.shoppro.service;

import java.util.List;

import com.beelun.shoppro.model.Category;
import com.beelun.shoppro.model.Tab;
import com.beelun.shoppro.model.Tab2CategoryMap;
import com.beelun.shoppro.pojo.MappingStatusEnum;

public interface Tab2CategoryMapManager {
	@SuppressWarnings("unchecked")
	public List getAll();
	public Tab2CategoryMap save(Tab2CategoryMap t2c);

	/**
	 * Again, I have rename the methods or move it around to work around web service errors in VS :(
	 * @param tabIdList
	 * @return
	 */
	public boolean purgeByCategory(Long[] categoryIdList);	
	public boolean purgeByTab(Long[] tabIdList);
	public boolean deleteMapIfExist(Long tabId, Long categoryId);

	/**
	 * Save one object which is partially filled to db. 
	 * @param t2c
	 * 		The tab and category object only contain id field. Other fields are not filled.
	 * @return
	 * 		Valid fully filled object
	 */
	public Tab2CategoryMap saveLazy(Tab2CategoryMap t2c); 
	public Tab2CategoryMap createMapIfNotExists(Tab tab, Category category);
	public Tab2CategoryMap createMapIfNotExists(Long tabId, Long categoryId);
	public MappingStatusEnum getMappingStatus(Long tabId, List<Long> categoryIdList);
		
}
