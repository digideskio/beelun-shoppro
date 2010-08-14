package com.beelun.shoppro.model;

import javax.xml.bind.Unmarshaller;

/**
 * Model class to define which categories are contained in the tab
 * 
 * @author bali
 *
 */
public class Tab2CategoryMap extends BaseObject {
	private static final long serialVersionUID = 6958187560280885065L;
	
	private Long id;	
	private Tab tab;   // parent pointer 	
	private Category category; // parent pointer	
	private Long categoryOrder = new Long(100);
	
	/**
	 * Refer to: https://jaxb.dev.java.net/guide/Mapping_cyclic_references_to_XML.html
	 * @param u
	 * @param parent
	 */
	public void afterUnmarshal(Unmarshaller u, Object parent) {
		if(parent instanceof Tab) {
			this.tab = (Tab)parent;
		} else if (parent instanceof Category) {
			this.category = (Category)category;
		}	    
	}	
	
	public Long getId() {
		return id;
	}
	public void setId(Long id) {
		this.id = id;
	}
	public Long getCategoryOrder() {
		return categoryOrder;
	}
	public void setCategoryOrder(Long categoryOrder) {
		this.categoryOrder = categoryOrder;
	}
	public Tab getTab() {
		return tab;
	}
	public void setTab(Tab tab) {
		this.tab = tab;
	}
	public Category getCategory() {
		return category;
	}
	public void setCategory(Category category) {
		this.category = category;
	}
}
