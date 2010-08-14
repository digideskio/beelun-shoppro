package com.beelun.shoppro.model;

/**
 * Payment methods for the shop such as PayPay, alipay, etc
 * 
 * @author bali
 *
 */
public class PaymentTool extends BaseObject {
	private static final long serialVersionUID = -8001489503472839870L;

	private Long id;
	private String name;
	private String description;
	private boolean enabled = true;
	
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
	public void setDescription(String description) {
		this.description = description;
	}
	public String getDescription() {
		return description;
	}
	public void setEnabled(boolean enabled) {
		this.enabled = enabled;
	}
	public boolean isEnabled() {
		return enabled;
	}

}
