package com.beelun.shoppro.service;

import java.util.List;

import com.beelun.shoppro.model.Category;
import com.beelun.shoppro.model.Item;
import com.beelun.shoppro.pojo.MappingStatus;
import com.beelun.shoppro.pojo.MappingStatusEnum;

public interface ItemManager {
	public Item get(Long id);
	public Item save(Item item);
		
	/**
	 * Remove item(s)
	 * @param id
	 * @return true if removed successfully, false otherwise
	 */
	public boolean removeItem(Long id);
	public boolean removeItems(Long[] idList);
	
	/**
	 * Don't show the item(s) in the site
	 * Use WebMethod annotation with different operationName to allow method overloading
	 * @param id
	 */
	public void hide(Long id);
	public void hide(Long[] idList);
	
	/**
	 * Show the item(s) in the site
	 */
	public void show(Long id);	
	public void show(Long[] idList);
	
	/**
	 * Get items list which should be shown under this category
	 * @param category
	 * @return
	 */
	public List<Item> getShown(Category category);
	public List<Item> getShownAll();
	public List<Item> getAll();
	public List<Item> getByIdList(List<Long> idList);
	public Item getItemByUrl(String itemUrl);
	
	/**
	 * Get a list of item according to certain condition
	 * @param orderBy
	 * @param ascending
	 * @param firstResult
	 * @param maxResult
	 *     		0 means result all, otherwise, apply normally
	 * @param categoryId 
	 * 			>=0 works normally; -1 means 'all categories', -2 means 'uncategorized'
	 * @return
	 */
	public List<Item> getByCondition(String orderBy, boolean ascending, int firstResult, int maxResult, Long categoryId);
	public int getCountByCondition(Long categoryId);
	
	/**
	 * Return a list of item based on user's input in search box
	 * 
	 * @param text
	 * @return
	 */
	public List<Item> searchByText(String text, int firstResult, int maxResult);
	
	/**
	 * Get count of a search
	 * 
	 * @param text
	 * @return
	 */
	public int searchByTextCount(String text);
	/**
	 * Return how many items in the db
	 * @return
	 */
	public int getAllCount();
	
	/**
	 * Get item by netsiteID
	 * @param nid
	 * @return
	 */
	public Item getItemByNID(Long nid);
	// TODO: all currency should expressed as BigDecimal type
	public float getItemTotalValue(List<Long> idList); 
	public boolean exists(Long itemId);
	
	/**
	 * Get specified items' mapping status to category
	 * 
	 * @param idList
	 * @return
	 */
	public List<MappingStatus> getMappingStatus(List<Long> idList);
	
	/**
	 * Set item list to category mapping status
	 * 
	 * @param itemIdList
	 * @param categoryId
	 * @param mappingStatus
	 * 			Should be NONE or ALL. Will do nothing if PARTIAL
	 * @return
	 */
	public boolean setMappingStatus(List<Long> itemIdList, Long categoryId, MappingStatusEnum mappingStatus);
}
