package com.beelun.shoppro.service;

import java.util.List;
import com.beelun.shoppro.model.Brand;

public interface BrandManager {

	public List<Brand> getAll();
	public Brand get(Long id);
	public Brand getByName(String name);
	public Brand save(Brand brand);
	
	/**
	 * Check whether item with given name exists. The check is case-insensitive.
	 * @param name
	 * 		name is item.
	 * @return
	 */
	public Brand getOrCreateByName(String name);
	/**
	 * Return 'NoBrand' brand
	 * @return
	 */
	public Brand getNoBrand();
	
	/**
	 * Remove many brands. Deletion will fail if there are FK to brands
	 * @param idList 
	 * 		id list of the brands
	 * @return
	 * 		true if success, false otherwise
	 */
	public boolean removeMany(Long[] idList);
}
