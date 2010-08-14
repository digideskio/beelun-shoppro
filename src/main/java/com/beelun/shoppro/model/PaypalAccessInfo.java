package com.beelun.shoppro.model;

/**
 * Paypal access information
 * 
 * @author bali
 *
 */
public class PaypalAccessInfo extends BaseObject {
	private static final long serialVersionUID = 1L;

	private Long id;
	private String apiUserName;
	private String apiPassword;
	private String apiSignature;
	private boolean useSandbox;
	
	public Long getId() {
		return id;
	}
	public void setId(Long id) {
		this.id = id;
	}
	public String getApiUserName() {
		return apiUserName;
	}
	public void setApiUserName(String apiUserName) {
		this.apiUserName = apiUserName;
	}
	public String getApiPassword() {
		return apiPassword;
	}
	public void setApiPassword(String apiPassword) {
		this.apiPassword = apiPassword;
	}
	public String getApiSignature() {
		return apiSignature;
	}
	public void setApiSignature(String apiSignature) {
		this.apiSignature = apiSignature;
	}
	public boolean isUseSandbox() {
		return useSandbox;
	}
	public void setUseSandbox(boolean useSandbox) {
		this.useSandbox = useSandbox;
	}
}
