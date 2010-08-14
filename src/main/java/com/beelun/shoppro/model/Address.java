package com.beelun.shoppro.model;


import com.beelun.shoppro.model.type.USStateEnum;

/**
 * The shipping & billing address
 * TODO: add ship-to area to better estimate shipping cost 
 * 
 * @author Bill Li(bill@beelun.com)
 *
 */
public class Address extends BaseObject {
	private static final long serialVersionUID = 1L;
	
	private Long id;
	
	// Nickname of the address for user to identify individual address items
	// For example, my mother's address
	private String name;          // cn
	
	private String address;       // us cn
	private String zip;           // us cn
	private String recipientName; // cn
	private String phoneNumber;  // us cn
	private String mobileNumber; // cn
	
	private String firstName;  // us
	private String lastName;   // us
	private String address2;   // us(optional)
	private String city;       // us
	private USStateEnum state = USStateEnum.AK; // us. No-null default 
	
	// public methods
	public void getValuesFromAnotherAddress(Address a) {
		if(null == a) {
			return;
		}
		// Copy all values except for id
		this.name = a.name;
		this.address = a.address;
		this.zip = a.zip;
		this.recipientName = a.recipientName;
		this.phoneNumber = a.phoneNumber;
		this.mobileNumber = a.mobileNumber;
		this.firstName = a.firstName;
		this.lastName = a.lastName;
		this.address2 = a.address2;
		this.city = a.city;
		this.state = a.state;
	}
	
	// Getters and setters
	public Long getId() {
		return id;
	}
	public void setId(Long id) {
		this.id = id;
	}
	public String getAddress() {
		return address;
	}
	public void setAddress(String address) {
		this.address = address;
	}
	public String getZip() {
		return zip;
	}
	public void setZip(String zip) {
		this.zip = zip;
	}
	public String getRecipientName() {
		return recipientName;
	}
	public void setRecipientName(String recipientName) {
		this.recipientName = recipientName;
	}
	public String getPhoneNumber() {
		return phoneNumber;
	}
	public void setPhoneNumber(String phoneNumber) {
		this.phoneNumber = phoneNumber;
	}
	public String getMobileNumber() {
		return mobileNumber;
	}
	public void setMobileNumber(String mobileNumber) {
		this.mobileNumber = mobileNumber;
	}
	public void setName(String name) {
		this.name = name;
	}
	public String getName() {
		return name;
	}
	public String getFirstName() {
		return firstName;
	}
	public void setFirstName(String firstName) {
		this.firstName = firstName;
	}
	public String getLastName() {
		return lastName;
	}
	public void setLastName(String lastName) {
		this.lastName = lastName;
	}
	public String getAddress2() {
		return address2;
	}
	public void setAddress2(String address2) {
		this.address2 = address2;
	}
	public String getCity() {
		return city;
	}
	public void setCity(String city) {
		this.city = city;
	}
	public USStateEnum getState() {
		return state;
	}
	public void setState(USStateEnum state) {
		this.state = state;
	}
}
