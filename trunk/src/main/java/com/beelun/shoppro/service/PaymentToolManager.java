package com.beelun.shoppro.service;

import java.util.List;

/**
 * Manage how customers pay the site money
 * 
 * @author Bill Li(bill@beelun.com)
 *
 */
public interface PaymentToolManager {
	@SuppressWarnings("unchecked")
	public List getAll();
	@SuppressWarnings("unchecked")
	public List getAllAvailable();
}
