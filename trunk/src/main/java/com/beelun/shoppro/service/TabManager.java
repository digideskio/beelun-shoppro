package com.beelun.shoppro.service;

import java.util.List;

import com.beelun.shoppro.model.Tab;

public interface TabManager {
	public Tab get(Long id);
	public List<Tab> getShownTabs(); 	// Get tabs which will shown in the site
	public List<Tab> getAll(); 			// Get all tabs in the db
	public Tab save(Tab tab);
	public Tab getTabByUrl(String urlName);
	
	/**
	 * Remove one tab
	 * @param id 
	 * 		id of the tab
	 * @return
	 */
	public boolean removeOne(Long id);
	
	/**
	 * Remove many tabs
	 * @param idList 
	 * 		id list of the tabs
	 * @return
	 */
	public boolean removeMany(Long[] idList);
	
}
