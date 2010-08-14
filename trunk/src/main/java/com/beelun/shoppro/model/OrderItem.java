package com.beelun.shoppro.model;

import java.math.BigDecimal;

import javax.xml.bind.Unmarshaller;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlTransient;

import com.beelun.shoppro.utils.GeneralUtils;

/**
 * Items in one specific order
 * This class should take a snapshot of Item in case the Item is subject to change
 * 
 * Several points:
 * (1) We store item as xml string. Don't store item as FK to shoppro_item table since it should be snapshot of Item
 * (2) For easy access, we de-serialize string back to Item 
 * 
 * @author Bill Li(bill@beelun.com)
 *
 */
public class OrderItem extends BaseObject {
	private static final long serialVersionUID = 1L;
	
	private Long id;
	private transient Order theOrder;
	//private Item item;
	private Long itemCount;
	
	// Price is changing. We need save what the price is when user places the order
	private BigDecimal sellPrice;
	private String itemInXmlString;
	
	public OrderItem() {
		
	}
	
	public OrderItem(Order order, Item item, BigDecimal sellPrice, Long itemCount) {
		this.theOrder = order;
		//this.item = item;
		this.itemCount = itemCount;
		this.sellPrice = sellPrice;
		this.itemInXmlString = GeneralUtils.ConvertItemToXmlString(item);
	}
		
	public Long getId() {
		return id;
	}
	public void setId(Long id) {
		this.id = id;
	}
	
	@XmlElement
	public Item getItem() {
		// return item;
		return GeneralUtils.ConvertXmlStringToItem(itemInXmlString);
	}
	public void setItem(Item item) {
		this.itemInXmlString = GeneralUtils.ConvertItemToXmlString(item);
	}
	public void setItemCount(Long itemCount) {
		this.itemCount = itemCount;
	}
	public Long getItemCount() {
		return itemCount;
	}
	public void setTheOrder(Order theOrder) {
		this.theOrder = theOrder;
	}
	
	@XmlTransient
	public Order getTheOrder() {
		return theOrder;
	}

	public BigDecimal getSellPrice() {
		return sellPrice;
	}

	public void setSellPrice(BigDecimal sellPrice) {
		this.sellPrice = sellPrice;
	}
	
	public void afterUnmarshal(Unmarshaller u, Object parent) {
	    this.theOrder = (Order)parent;
	}

	public String getItemInXmlString() {
		return itemInXmlString;
	}

	public void setItemInXmlString(String itemInXmlString) {
		this.itemInXmlString = itemInXmlString;
	}
	
}
