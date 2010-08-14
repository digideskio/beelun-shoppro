package com.beelun.shoppro.model;

import javax.xml.bind.Unmarshaller;

/**
 * Define which items are contained in certain category
 * 
 * @author Bill Li(bill@beelun.com)
 *
 */
public class Category2ItemMap extends BaseObject {
	private static final long serialVersionUID = -3357221910755362137L;
	
	private Long id;	
	private Category category;  	// FK to category	
	private Item item;		// FK to item	
	private Long itemOrder = new Long(100);
	
	public Category2ItemMap() {}
	
	public Category2ItemMap(Category c, Item i, Long itemOrder)
	{
		this.category = c;
		this.item = i;
		this.itemOrder = itemOrder;		
	}
	
	/**
	 * Refer to: https://jaxb.dev.java.net/guide/Mapping_cyclic_references_to_XML.html
	 * TODO: remove this
	 * @param u
	 * @param parent
	 */
	public void afterUnmarshal(Unmarshaller u, Object parent) {
		if(parent instanceof Item) {
			this.item = (Item)parent;
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
	public Long getItemOrder() {
		return itemOrder;
	}
	public void setItemOrder(Long itemOrder) {
		this.itemOrder = itemOrder;
	}
	public Category getCategory() {
		return category;
	}
	public void setCategory(Category category) {
		this.category = category;
	}
	public Item getItem() {
		return item;
	}
	public void setItem(Item item) {
		this.item = item;
	}
}
