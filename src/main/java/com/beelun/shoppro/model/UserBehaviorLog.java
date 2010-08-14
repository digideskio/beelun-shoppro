package com.beelun.shoppro.model;

import java.util.Date;

/**
 * The class to record user's behavior in the site.
 * We will use this for further analysis
 * 
 * @author bali
 *
 */
public class UserBehaviorLog extends BaseObject {
	private static final long serialVersionUID = 1L;
	
	private Long id;
	private String sessionId;
	private Date when = new Date();
	private String url;
	private String userId;	// optional
	private String ipAddress;
	
	public Long getId() {
		return id;
	}
	public void setId(Long id) {
		this.id = id;
	}
	public String getSessionId() {
		return sessionId;
	}
	public void setSessionId(String sessionId) {
		this.sessionId = sessionId;
	}
	public String getUrl() {
		return url;
	}
	public void setUrl(String url) {
		this.url = url;
	}
	public String getUserId() {
		return userId;
	}
	public void setUserId(String userId) {
		this.userId = userId;
	}
	public String getIpAddress() {
		return ipAddress;
	}
	public void setIpAddress(String ipAddress) {
		this.ipAddress = ipAddress;
	}
	public void setWhen(Date when) {
		this.when = when;
	}
	public Date getWhen() {
		return when;
	}
}
