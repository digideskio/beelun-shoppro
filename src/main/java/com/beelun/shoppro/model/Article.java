package com.beelun.shoppro.model;

import java.util.Date;

import javax.xml.bind.annotation.XmlElement;

import com.beelun.shoppro.SuperContainer;

/**
 * Model class for Article in html format
 * Note: footer page will be also stored here
 * 
 * @author bali
 *
 */
public class Article extends BaseObject {
	private static final long serialVersionUID = 2882276736069287586L;
	
	private Long id; 
	private String title;
	private String content;
	private boolean isShown = true; // whether or not to show in the site
	private Date updated = new Date(); // will be set on save
	//
	// For SEO
	//
	private String pageTitle;	
	// Two purposes for 'keywords':
	// (1) SEO keyword meta tag
	// (2) Search by keyword in front end and back end
	// And each keyword is separated by comma(,)
	private String keywords;		
	private String description;
	private String metaTag;  // For customized meta tags, for example, for tracking
	private String url;
	
	public String getTitle() {
		return title;
	}
	public void setTitle(String title) {
		this.title = title;
	}
	public String getContent() {
		return content;
	}
	public void setContent(String content) {
		this.content = content;
	}
	public Date getUpdated() {
		return updated;
	}
	public void setUpdated(Date updated) {
		this.updated = updated;
	}
	public Long getId() {
		return id;
	}
	public void setId(Long id) {
		this.id = id;
	}
	
	/**
	 * Get the url to be shown in the site
	 * 
	 * @return
	 */	
	@XmlElement
	public String getMyUrl() {
		String ret = null;
		if (null != getId()) {
			ret = String.format("%s/article/%s-%s", SuperContainer.BaseUrl, getId() + "", getUrl());		
			// Always add html to the end
			if(!getUrl().endsWith(".html")) {
				ret = ret + ".html";
			}
		}
		return ret;
	}
	
	public String getPageTitle() {
		return pageTitle;
	}
	public void setPageTitle(String pageTitle) {
		this.pageTitle = pageTitle;
	}
	public String getKeywords() {
		return keywords;
	}
	public void setKeywords(String keywords) {
		this.keywords = keywords;
	}
	public String getDescription() {
		return description;
	}
	public void setDescription(String description) {
		this.description = description;
	}
	public String getMetaTag() {
		return metaTag;
	}
	public void setMetaTag(String metaTag) {
		this.metaTag = metaTag;
	}
	public String getUrl() {
		return url;
	}
	public void setUrl(String url) {
		this.url = url;
	}
	public boolean getIsShown() {
		return isShown;
	}
	public void setIsShown(boolean isShown) {
		this.isShown = isShown;
	}
}
