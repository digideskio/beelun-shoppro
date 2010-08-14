package com.beelun.shoppro.model;

import java.util.List;
import javax.xml.bind.annotation.XmlTransient;
import com.beelun.shoppro.model.type.SiteTypeEnum;

/**
 * Set shop wide globles such as shop name, logo, address, max upload file size, 404, 500, no search result
 * 
 * @author bali
 *
 */
public class MyGlob extends BaseObject  {
	private static final long serialVersionUID = 3599140744461407051L;
	
	private Long id;
	private String shopName;
	private String slogan;
	private String phoneNumber;
	private String logoAbsoluteUrl;
	private String address;
	private String page404;
	private String page500;
	private String pageNoSearchResult;
	private String footer;
	private String googleCustSearchCode;
	private String signupAgreement;
	private String version;
	private SiteTypeEnum siteType;
	
	// We put below signup confirmation page in the DB provide much more flexibility
	// for users to customize the page as they like
	private String unlockEmailTemplate;
	private String resetPasswordMailTemplate;
	
	// Read-only. Not persistented. Don't serialize	
	private transient List<Tab> shownTabs;
	
	public Long getId() {
		return id;
	}
	public void setId(Long id) {
		this.id = id;
	}
	public String getShopName() {
		return shopName;
	}
	public void setShopName(String shopName) {
		this.shopName = shopName;
	}
	public String getSlogan() {
		return slogan;
	}
	public void setSlogan(String slogan) {
		this.slogan = slogan;
	}
	public String getPhoneNumber() {
		return phoneNumber;
	}
	public void setPhoneNumber(String phoneNumber) {
		this.phoneNumber = phoneNumber;
	}
	public String getLogoAbsoluteUrl() {
		return logoAbsoluteUrl;
	}
	public void setLogoAbsoluteUrl(String logoUrl) {
		this.logoAbsoluteUrl = logoUrl;
	}
	public String getAddress() {
		return address;
	}
	public void setAddress(String address) {
		this.address = address;
	}
	public String getPage404() {
		return page404;
	}
	public void setPage404(String page404) {
		this.page404 = page404;
	}
	public String getPage500() {
		return page500;
	}
	public void setPage500(String page500) {
		this.page500 = page500;
	}
	public String getPageNoSearchResult() {
		return pageNoSearchResult;
	}
	public void setPageNoSearchResult(String pageNoSearchResult) {
		this.pageNoSearchResult = pageNoSearchResult;
	}
	public String getFooter() {
		return footer;
	}
	public void setFooter(String footer) {
		this.footer = footer;
	}
	public String getGoogleCustSearchCode() {
		return googleCustSearchCode;
	}
	public void setGoogleCustSearchCode(String googleCustSearchCode) {
		this.googleCustSearchCode = googleCustSearchCode;
	}

	// Annotation "@XmlTransient" is also needed
	@XmlTransient
	public List<Tab> getShownTabs() {
		return shownTabs;
	}
	public void setShownTabs(List<Tab> shownTabs) {
		this.shownTabs = shownTabs;
	}
	public void setSignupAgreement(String signupAgreement) {
		this.signupAgreement = signupAgreement;
	}
	public String getSignupAgreement() {
		return signupAgreement;
	}
	public void setUnlockEmailTemplate(String unlockEmailTemplate) {
		this.unlockEmailTemplate = unlockEmailTemplate;
	}
	public String getUnlockEmailTemplate() {
		return unlockEmailTemplate;
	}
	public void setResetPasswordMailTemplate(String resetPasswordMailTemplate) {
		this.resetPasswordMailTemplate = resetPasswordMailTemplate;
	}
	public String getResetPasswordMailTemplate() {
		return resetPasswordMailTemplate;
	}
	public String getVersion() {
		return version;
	}
	public void setVersion(String version) {
		this.version = version;
	}
	public void setSiteType(SiteTypeEnum siteType) {
		this.siteType = siteType;
	}
	public SiteTypeEnum getSiteType() {
		return siteType;
	}
}
