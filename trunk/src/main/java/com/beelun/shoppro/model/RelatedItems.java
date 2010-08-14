package com.beelun.shoppro.model;

/**
 * Visitors might be also interested in other items for a given item
 * For example, for a record: 1,1,2 means item1's SeeAlso is item2, and item2's SeeAlso is item1
 * It is NOT necessary to put another record 1,2,1
 *  
 * @author bali
 *
 */
public class RelatedItems extends BaseObject {
	private static final long serialVersionUID = 1077611684098388007L;
	
	private Long id;
	private Long itemIdA;
	private Long itemIdB;
	public Long getId() {
		return id; 
	}
	public void setId(Long id) {
		this.id = id;
	}
	public Long getItemIdA() {
		return itemIdA;
	}
	public void setItemIdA(Long itemIdA) {
		this.itemIdA = itemIdA;
	}
	public Long getItemIdB() {
		return itemIdB;
	}
	public void setItemIdB(Long itemIdB) {
		this.itemIdB = itemIdB;
	}
}
