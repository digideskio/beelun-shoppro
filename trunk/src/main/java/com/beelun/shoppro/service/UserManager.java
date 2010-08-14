package com.beelun.shoppro.service;

import com.beelun.shoppro.model.User;
import com.beelun.shoppro.pojo.ChartDataPointModel;
import com.beelun.shoppro.pojo.TimeAccuracyEnum;

import org.springframework.security.userdetails.UserDetails;    
import org.springframework.security.userdetails.UsernameNotFoundException;   
import org.springframework.transaction.annotation.Transactional;

import java.util.Date;
import java.util.List;


public interface UserManager {
    @SuppressWarnings("unchecked")
	public List getUsers();
    public User getUserById(String userId);
    public User getUserByEmail(String email);
    public boolean exists(String email);
    public void removeUser(String userId);
    public boolean unlock(String token);
    public boolean resetPassword(String token, String newPassword);
    public User saveUser(User user, boolean needEncodePassword);
    
    /**
     * Gets users information based on login name.
     * @param username the user's username
     * @return userDetails populated userDetails object
     * @throws org.springframework.security.userdetails.UsernameNotFoundException thrown when user not found in database
     */
    @Transactional    
    public UserDetails loadUserByUsername(String email) throws UsernameNotFoundException;

    /**
     * Verify that inputed username/password is for a valid admin
     * @param email
     * @param passwordPlainText
     * @return
     */
    public User getAdmin(String email, String passwordPlainText);

    /**
     * Get a list of User according to certain condition
     * 
     * @param orderBy
     * @param ascending
     * @param firstResult
     * @param maxResult
     * @return
     */
	public List<User> getByCondition(String orderBy, boolean ascending, int firstResult, int maxResult);
	public int getAllCount();
	
	/**
	 * Get trend of new user registration
	 * 
	 * @param specifiedStartingTime
	 * 			Starting which time(year, month, or day, ...)
	 * @param timeAccuracy
	 * 			Determine the time accuracy of 'specifiedTime'
	 * @param maxDays
	 * 			Max days to return
	 * @return
	 * 			List of <Date, long> (other x fields are not used)
	 */
	public List<ChartDataPointModel> getNewUserTrend(Date specifiedStartingTime, TimeAccuracyEnum timeAccuracy, int maxCount);
	
	/**
	 * Reset one user's password
	 * 
	 * @param userId
	 * @param newPasswordInPlainText
	 * @return
	 */
	public boolean resetPassword(Long userId, String newPasswordInPlainText);
	
	/**
	 * Lock or unlock one user's access to the web site
	 * 
	 * @param userId
	 * 				The user Id
	 * @param isEnabled
	 * 				True - user be able to access the site normally
	 * 				False - user will be locked out of the web site. He/She can browse, but not able to login to access protected resources
	 * @return
	 * 			True if success, false otherwise
	 */
	public boolean enableDisableUserAccess(Long userId, boolean isEnabled);
}
