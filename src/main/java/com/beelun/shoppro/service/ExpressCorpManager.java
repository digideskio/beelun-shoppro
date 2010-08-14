package com.beelun.shoppro.service;

import java.util.List;

/**
 * Manage express corporations
 * 
 * @author Bill Li(bill@beelun.com)
 *
 */
public interface ExpressCorpManager {
	@SuppressWarnings("unchecked")
	public List getAll();
	@SuppressWarnings("unchecked")
	public List getAllAvailable();
}
