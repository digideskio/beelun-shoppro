package com.beelun.shoppro.model;


/**
 * User/Role map
 * 
 * @author Bill Li(bill@beelun.com)
 *
 */
public class Membership extends BaseObject {
	private static final long serialVersionUID = 1L;
	
	private Long id;
	private User user;
	private Role role;
	
	public Long getId() {
		return id;
	}
	public void setId(Long id) {
		this.id = id;
	}
	public User getUser() {
		return user;
	}
	public void setUser(User user) {
		this.user = user;
	}
	public Role getRole() {
		return role;
	}
	public void setRole(Role role) {
		this.role = role;
	}
	
}
