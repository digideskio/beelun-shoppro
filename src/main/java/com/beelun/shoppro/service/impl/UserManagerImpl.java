package com.beelun.shoppro.service.impl;

import java.util.Date;
import java.util.List;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.GrantedAuthority;
import org.springframework.security.providers.encoding.PasswordEncoder;
import org.springframework.security.userdetails.UserDetails;
import org.springframework.security.userdetails.UserDetailsService;
import org.springframework.security.userdetails.UsernameNotFoundException;
import org.springframework.stereotype.Service;

import com.beelun.shoppro.dao.hibernate.GenericDaoHibernate;
import com.beelun.shoppro.model.User;
import com.beelun.shoppro.pojo.ChartDataPointModel;
import com.beelun.shoppro.pojo.TimeAccuracyEnum;
import com.beelun.shoppro.service.UserManager;

/**
 * The value may indicate a suggestion for a logical component name, to be turned into a Spring bean in case of an autodetected component.
 * 
 * @author bali
 * 
 */
@Service(value = "userManager")
public class UserManagerImpl extends GenericDaoHibernate<User, Long> implements UserManager, UserDetailsService {
	private transient final Log log = LogFactory.getLog(UserManagerImpl.class);
	private transient final String roleAdmin = "ROLE_ADMIN";

	@Autowired
	PasswordEncoder passwordEncoder;

	@Autowired
	public UserManagerImpl(SessionFactory sessionFactory) {
		super(User.class, sessionFactory);
	}

	@SuppressWarnings("unchecked")
	public List getUsers() {
		return getAll();
	}

	public User getUserById(String userId) {
		try {
			Long userIdLong = Long.valueOf(userId);
			return get(userIdLong);
		} catch (Exception ex) {
			log.error("fail to parse userId");
			return null;
		}
	}

	public User saveUser(User user, boolean needEncodePassword) {
		if (needEncodePassword) {
			String newPasword = passwordEncoder.encodePassword(user.getPassword(), null);
			user.setPassword(newPasword);
		}
		return super.save(user);
	}

	public void removeUser(String userId) {
		remove(Long.valueOf(userId));
	}

	/**
	 * Called when user logs in
	 */
	@SuppressWarnings("unchecked")
	@Override
	public UserDetails loadUserByUsername(String email) throws UsernameNotFoundException {
		List users = getHibernateTemplate().find("from User where enabled=true and accountLocked=false and email=?", email);
		if (users == null || users.isEmpty()) {
			throw new UsernameNotFoundException("user '" + email + "' not found...");
		} else {
			UserDetails userDetails = (UserDetails) users.get(0);
			log.debug("Access User, Id: " + userDetails.getUsername());
			return userDetails;
		}
	}

	/**
	 * Called when new user signs up
	 */
	@SuppressWarnings("unchecked")
	@Override
	public boolean exists(String email) {
		List users = getHibernateTemplate().find("from User where email=?", email);
		if (users == null || users.isEmpty()) {
			return false;
		} else {
			return true;
		}
	}

	/**
	 * unlock the user by token.
	 * Return
	 * true if find the user and unlocked successfully, false otherwise
	 */
	@SuppressWarnings("unchecked")
	@Override
	public boolean unlock(String token) {
		List users = getHibernateTemplate().find("from User where unlockToken=?", token);
		if (users != null && !users.isEmpty()) {
			User u = (User) users.get(0);
			u.setAccountLocked(false);
			u.setUnlockToken(null);
			this.saveUser(u, false);
			return true;
		}
		return false;
	}

	/**
	 * Reset password indicated by token
	 */
	@SuppressWarnings("unchecked")
	@Override
	public boolean resetPassword(String token, String newPassword) {
		List users = getHibernateTemplate().find("from User where resetPswdToken=?", token);
		if (users != null && !users.isEmpty()) {
			User u = (User) users.get(0);
			u.setPassword(newPassword);
			u.setResetPswdToken(null);
			this.saveUser(u, true); // Need hash the password
			return true;
		}
		return false;
	}

	@SuppressWarnings("unchecked")
	@Override
	public User getUserByEmail(String email) {
		List users = getHibernateTemplate().find("from User where email=?", email);
		if (users == null || users.isEmpty()) {
			return null;
		} else {
			return (User) users.get(0);
		}
	}

	@SuppressWarnings("unchecked")
	@Override
	public User getAdmin(String email, String passwordPlainText) {
		String encodedPassword = passwordEncoder.encodePassword(passwordPlainText, null);
		List users = getHibernateTemplate().find("from User where enabled=true and accountLocked=false and email=? and password=?", new Object[] { email, encodedPassword });
		if (users == null || users.isEmpty()) {
			return null;
		} else {
			User user = (User) users.get(0);
			for (GrantedAuthority a : user.getAuthorities()) {
				if (a.getAuthority().equals(roleAdmin)) {
					return user;
				} else {
					// Customer login
				}
			}
		}
		return null;
	}

	@Override
	public List<User> getByCondition(String orderBy, boolean ascending, int firstResult, int maxResult) {
		return super.getByCondition(orderBy, ascending, firstResult, maxResult);
	}

	@Override
	public int getAllCount() {
		return super.getAllCount();
	}

	@Override
	public List<ChartDataPointModel> getNewUserTrend(Date specifiedStartingTime, TimeAccuracyEnum timeAccuracy, int maxCount) {
		return null;
	}

	@Override
	public boolean enableDisableUserAccess(Long userId, boolean isEnabled) {
		User u = get(userId);
		u.setEnabled(isEnabled);
		if (this.saveUser(u, false) != null) {
			return true;
		} else {
			return false;
		}
	}

	@Override
	public boolean resetPassword(Long userId, String newPasswordInPlainText) {
		User u = get(userId);
		if (u != null) {
			u.setPassword(newPasswordInPlainText);
			u.setResetPswdToken(null);
			this.saveUser(u, true); // Need hash the password
			return true;
		}
		return false;
	}

}
