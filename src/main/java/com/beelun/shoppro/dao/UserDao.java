package com.beelun.shoppro.dao;

import com.beelun.shoppro.model.User;

import java.util.List;


public interface UserDao extends Dao {
    @SuppressWarnings("unchecked")
	public List getUsers();
    public User getUser(Long userId);
    public void saveUser(User user);
    public void removeUser(Long userId);
}
