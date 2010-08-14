package com.beelun.shoppro.service;

import java.util.List;

import com.beelun.shoppro.model.Media;

public interface MediaManager {
	public Media get(Long id);
	public Media save(Media media);
	
	/**
	 * Create a new media along with the content
	 * 
	 * @param media
	 * @param content
	 * @return
	 */
	public Media createNew(Media media, byte[] content);
	
	/**
	 * Remove many medias
	 * @param idList 
	 * 		id list of the categories
	 * @return
	 */
	public boolean removeMany(Long[] mediaIdList);
	public List<Media> getByCondition(String orderBy, boolean ascending, int firstResult, int maxResult);
	public int getAllCount();

	/**
	 * Return a list of media based on user's input in search box
	 * 
	 * @param text
	 * @return
	 */
	public List<Media> searchByText(String text, int firstResult, int maxResult);
	
	/**
	 * Get count of a search
	 * 
	 * @param text
	 * @return
	 */
	public int searchByTextCount(String text);
	
}
