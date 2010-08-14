package com.beelun.shoppro.model;

import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Set;

import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlTransient;

import com.beelun.shoppro.SuperContainer;


/**
 * Model class for tab in the web page
 * 
 * @author bali
 * 
 */
public class Tab extends BaseObject {
	private static final long serialVersionUID = 7704081219275921414L;

	private Long id;
	private String name;
	private boolean isShown = true;
	private String content;
	private Long showOrder;	
	private transient Set<Tab2CategoryMap> categoryMap;	
	private Date updated = new Date(); // will be set on save

	//
	// For SEO
	//
	private String pageTitle;
	// Two purposes for 'keywords':
	// (1) SEO keyword meta tag
	// (2) Search by keyword in front end and back end
	// And each keyword is separated by comma(,)
	private String keywords; // TODO: put this into view
	private String description;
	private String metaTag; // For customized meta tags, for example, for
							// tracking
	private String url;

	/**
	 * Get all categories belong to this tab
	 * 
	 * @return List of categories
	 * 
	 */
	public List<Category> getAllCategories() {
		List<Category> categoryList = new ArrayList<Category>();
		if (null != this.categoryMap) {
			for (Tab2CategoryMap map : this.categoryMap) {
				categoryList.add(map.getCategory());
			}
		}

		return categoryList;
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
			ret = String.format("%s/tab/%s-%s", SuperContainer.BaseUrl, getId() + "", getUrl());
			
			// Always add html to the end
			if(!getUrl().endsWith(".html")) {
				ret = ret + ".html";
			}
		}

		return ret;
	}

	public Long getId() {
		return id;
	}

	public void setId(Long id) {
		this.id = id;
	}

	public String getPageTitle() {
		return pageTitle;
	}

	public void setPageTitle(String pageTitle) {
		this.pageTitle = pageTitle;
		setUpdated();
	}

	public String getContent() {
		return content;
	}

	public void setContent(String content) {
		this.content = content;
		setUpdated();
	}

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
		setUpdated();
	}

	public boolean getIsShown() {
		return isShown;
	}

	public void setIsShown(boolean isShown) {
		this.isShown = isShown;
	}

	public String getKeywords() {
		return keywords;
	}

	public void setKeywords(String keywords) {
		this.keywords = keywords;
		setUpdated();
	}

	public String getDescription() {
		return description;
	}

	public void setDescription(String description) {
		this.description = description;
		setUpdated();
	}

	public String getMetaTag() {
		return metaTag;
	}

	public void setMetaTag(String metaTag) {
		this.metaTag = metaTag;
		setUpdated();
	}

	public String getUrl() {
		return url;
	}

	public void setUrl(String url) {
		this.url = url;
		setUpdated();
	}

	public void setCategoryMap(Set<Tab2CategoryMap> categoryMap) {
		this.categoryMap = categoryMap;
	}

	@XmlTransient
	public Set<Tab2CategoryMap> getCategoryMap() {
		return categoryMap;
	}

	public Long getShowOrder() {
		return showOrder;
	}

	public void setShowOrder(Long showOrder) {
		this.showOrder = showOrder;
	}

	public void setUpdated(Date updated) {
		this.updated = updated;
	}

	public Date getUpdated() {
		return updated;
	}

	public void setUpdated() {
		setUpdated(new Date());
	}

}
