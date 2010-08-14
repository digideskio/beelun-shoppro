package com.beelun.shoppro.model;

import java.math.BigDecimal;
import java.util.Date;

import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;

import com.beelun.shoppro.SuperContainer;

/**
 * Model class for one individual item(a sell-able unit)
 * 
 * @author bali
 *
 */
@XmlRootElement
public class Item extends BaseObject {
	private static final long serialVersionUID = -5030782839458276723L;
	
	private Long id; 
	private Long netSuiteId; // For import/export purpose
	private Brand brand; // FK to Brand
	private String name;
	private String serialNumber;
	private String shortDescription;
	private String detailedDescription;
	private String image; 		// big picture
	private String thumbNail; 	// small image
	private BigDecimal listPrice;
	private BigDecimal sellPrice;
	private Long inventoryNumber;
	private boolean isShown = true; // whether or not to show in the site
	private Template template = null;
	private Date updated = new Date(); // will be set on save

	//
	// For SEO
	//
	private String pageTitle = "Untitled";	
	// Two purposes for 'keywords':
	// (1) SEO keyword meta tag
	// (2) Search by keyword in front end and back end
	// And each keyword is separated by comma(,)
	private String keywords;		
	private String description;
	private String metaTag;  // For customized meta tags, for example, for tracking
	private String url;

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
	}
	public String getShortDescription() {
		return shortDescription;
	}
	public void setShortDescription(String shortDescription) {
		this.shortDescription = shortDescription;
	}
	public String getDetailedDescription() {
		return detailedDescription;
	}
	public void setDetailedDescription(String detailedDescription) {
		this.detailedDescription = detailedDescription;
	}
	public String getPageTitle() {
		return pageTitle;
	}
	public void setPageTitle(String pageTitle) {
		this.pageTitle = pageTitle;
	}
	public BigDecimal getListPrice() {
		return listPrice;
	}
	public void setListPrice(BigDecimal listPrice) {
		this.listPrice = listPrice;
	}
	public BigDecimal getSellPrice() {
		return sellPrice;
	}
	public void setSellPrice(BigDecimal sellPrice) {
		this.sellPrice = sellPrice;
	}
	public Long getInventoryNumber() {
		return inventoryNumber;
	}
	public void setInventoryNumber(Long inventoryNumber) {
		this.inventoryNumber = inventoryNumber;
	}
	public Long getNetSuiteId() {
		return netSuiteId;
	}
	public void setNetSuiteId(Long netSuiteId) {
		this.netSuiteId = netSuiteId;
	}
	public String getSerialNumber() {
		return serialNumber;
	}
	public void setSerialNumber(String serialNumber) {
		this.serialNumber = serialNumber;
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
	public String getImage() {
		return image;
	}
	public void setImage(String image) {
		this.image = image;
	}
	public String getThumbNail() {
		return thumbNail;
	}
	public void setThumbNail(String thumbNail) {
		this.thumbNail = thumbNail;
	}
	public Brand getBrand() {
		return brand;
	}
	public void setBrand(Brand brand) {
		this.brand = brand;
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
			ret = String.format("%s/item/%s-%s", SuperContainer.BaseUrl, getId() + "", getUrl());
			
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
