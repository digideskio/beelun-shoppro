package com.beelun.shoppro.model;

/**
 * Model class to manage the shipping service provider, such as UPS, DHL
 * 
 * @author bali
 *
 */
public class ExpressCorp extends BaseObject {
	private static final long serialVersionUID = -7211231651566700615L;
	
	private Long id;
	private String shortName;
	private String fullName;
	private boolean enabled = true;
	private String webSite;
	
	public Long getId() {
		return id;
	}
	public void setId(Long id) {
		this.id = id;
	}
	public String getShortName() {
		return shortName;
	}
	public void setShortName(String shortName) {
		this.shortName = shortName;
	}
	public String getFullName() {
		return fullName;
	}
	public void setFullName(String fullName) {
		this.fullName = fullName;
	}
	public boolean isEnabled() {
		return enabled;
	}
	public void setEnabled(boolean enabled) {
		this.enabled = enabled;
	}
	public String getWebSite() {
		return webSite;
	}
	public void setWebSite(String webSite) {
		this.webSite = webSite;
	}

}
