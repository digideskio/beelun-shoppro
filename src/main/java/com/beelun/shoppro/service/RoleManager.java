package com.beelun.shoppro.service;

import com.beelun.shoppro.model.Role;

/**
 * Interface to define 'Role' actions
 * 
 * @author bali
 *
 */
public interface RoleManager {
	public Role getCustomerRole();
	public Role getAdminRole();
}
