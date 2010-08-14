package com.beelun.shoppro.model;

import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Set;

import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlTransient;

import com.beelun.shoppro.SuperContainer;

/**
 * A grouping unit of the web site, shows as a left side bar
 * 
 * Refer to: 
 *     NetSuite: https://system.netsuite.com/app/site/setup/storetab.nl?id=10&e=T&whence=
 *     ShopEx: http://localhost:83/shopadmin/index.php#ctl=goods/category&act=index
 * 
 * @author bali
 *
 */
public class Category extends BaseObject {
	private static final long serialVersionUID = -7010065597481313103L;
	
	private Long id;
	private String name;
	private boolean isShown = true;	
	private transient Set<Category2ItemMap> itemMap;
	private Template template = null;
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

	/**
	 * Get all items belong to this category
	 * 
	 * @return
	 */
	public List<Item> getAllItems() {
		List<Item> itemList = new ArrayList<Item>();
		if(this.itemMap != null) {
			for(Category2ItemMap map : this.itemMap) {
				itemList.add(map.getItem());
			}
		}
		
		return itemList;
	}
	
	public Long getId() {
		return id;
	}
	public void setId(Long id) {
		this.id = id;
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
	public String getPageTitle() {
		return pageTitle;
	}
	public void setPageTitle(String pageTitle) {
		this.pageTitle = pageTitle;
		setUpdated();
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
	
	@XmlTransient
	public Set<Category2ItemMap> getItemMap() {
		return itemMap;
	}
	public void setItemMap(Set<Category2ItemMap> itemMap) {
		this.itemMap = itemMap;
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
	/**
	 * Get the url to be shown in the site
	 * 
	 * @return
	 */	
	@XmlElement
	public String getMyUrl() {
		String ret = null;
		if (null != getId()) {
			ret = String.format("%s/category/%s-%s", SuperContainer.BaseUrl, getId() + "", getUrl());
			
			// Always add html to the end
			if(!getUrl().endsWith(".html")) {
				ret = ret + ".html";
			}
		}

		return ret;
	}

	public Template getTemplate() {
		return template;
	}

	public void setTemplate(Template template) {
		this.template = template;
	}
		
}
