package com.beelun.shoppro.model;

import java.math.BigDecimal;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Set;

import javax.xml.bind.annotation.XmlElement;

import com.beelun.shoppro.model.type.OrderStatusEnum;
import com.beelun.shoppro.model.type.ShipDateEnum;
import com.beelun.shoppro.model.type.ShipTimeEnum;


/**
 * The order placed by customers
 * @author bali
 *
 */
public class Order extends BaseObject {	
	private static final long serialVersionUID = 1L;
	
	//---------both us and cn ----------
	private Long id;
	// TODO: orderNo is used to identify order rather than id from customer's perspective
	// this is good for business security
	// private String orderNo;
	
	// When this order was placed.
	private Date orderDate = new Date();
	private OrderStatusEnum status = OrderStatusEnum.NOTPAID; 
	private Set<OrderItem> orderItemSet;
		
	private User user;	// Who placed this order
	private Address shippingAddress;
	
	//---------us only ----------
	private Address billingAddress; // us only
	private boolean sameAddress;	// us only
	
	//---------cn only ----------
	// 'shipDate' and 'shipTime' are actually select box in the UI.
	// Right now, we save it to db as String, while put select options to resources.
	// Alternative design is to put them into DB, which looks unnecessary at this moment
	// TODO: is this the best way to do the design? especially for localization?
	private ShipDateEnum shipDate;  // among(select.shipDate.x) // cn only
	private ShipTimeEnum shipTime;  // among(select.shipTime.x) // cn only
	
	// This is valid only when shipDate==SpecifiedDate
	private Date specifiedShipDate = null; // cn only
	
	private String notes; // cn only
	private ExpressCorp expressCorp; // cn only
	private PaymentTool paymentTool; // cn only
	
	//
	// Constructors
	//
	public Order(User user) {
		this.user = user;
	}
	
	public Order() {}

	//
	// Setters and getters
	//
	public Long getId() {
		return id;
	}

	public void setId(Long id) {
		this.id = id;
	}

	public String getNotes() {
		return notes;
	}

	public void setNotes(String notes) {
		this.notes = notes;
	}

	public ExpressCorp getExpressCorp() {
		return expressCorp;
	}

	public void setExpressCorp(ExpressCorp expressCorp) {
		this.expressCorp = expressCorp;
	}

	public void setUser(User user) {
		this.user = user;
	}

	public User getUser() {
		return user;
	}

	public void setOrderItemSet(Set<OrderItem> orderItemSet) {
		this.orderItemSet = orderItemSet;
	}

	public Set<OrderItem> getOrderItemSet() {
		return orderItemSet;
	}

	public void setOrderDate(Date orderDate) {
		this.orderDate = orderDate;
	}

	public Date getOrderDate() {
		return orderDate;
	}

	public void setSpecifiedShipDate(Date specifiedShipDate) {
		this.specifiedShipDate = specifiedShipDate;
	}

	public Date getSpecifiedShipDate() {
		return specifiedShipDate;
	}

	public PaymentTool getPaymentTool() {
		return paymentTool;
	}

	public void setPaymentTool(PaymentTool paymentTool) {
		this.paymentTool = paymentTool;
	}

	@XmlElement
	public String getSerialNumber() {
		if(null != getId()) {
			final String dataTimeFormat = "yyyyMMdd";
			return (new SimpleDateFormat(dataTimeFormat).format(getOrderDate())) + getId();
		} else {
			return null;
		}
	}

	public void setStatus(OrderStatusEnum status) {
		this.status = status;
	}

	public OrderStatusEnum getStatus() {
		return status;
	}

	public ShipDateEnum getShipDate() {
		return shipDate;
	}

	public void setShipDate(ShipDateEnum shipDate) {
		this.shipDate = shipDate;
	}

	public ShipTimeEnum getShipTime() {
		return shipTime;
	}

	public void setShipTime(ShipTimeEnum shipTime) {
		this.shipTime = shipTime;
	}
	
	// TODO: remove this and caller should call getAmount() instead
//	public float getPaymentAmount() {
//		return getAmount().floatValue();
//	}

	@XmlElement
	public BigDecimal getAmount() {
		BigDecimal paymentAmount = new BigDecimal(0);
		for(OrderItem orderItem : orderItemSet) {
			// Note: use orderItem.getSellPrice() since we should take the price when user placed the order
			paymentAmount = paymentAmount.add(orderItem.getSellPrice().multiply(new BigDecimal(orderItem.getItemCount()))); 
		}
		return paymentAmount;
	}

	public Address getShippingAddress() {
		return shippingAddress;
	}

	public void setShippingAddress(Address shippingAddress) {
		this.shippingAddress = shippingAddress;
	}

	public Address getBillingAddress() {
		return billingAddress;
	}

	public void setBillingAddress(Address billingAddress) {
		this.billingAddress = billingAddress;
	}

	public boolean isSameAddress() {
		return sameAddress;
	}

	public void setSameAddress(boolean sameAddress) {
		this.sameAddress = sameAddress;
	}
}
