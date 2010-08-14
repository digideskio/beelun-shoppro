package com.beelun.shoppro.model;

/**
 * Model class for item brand
 * @author bali
 *
 */
public class Brand extends BaseObject {
	private static final long serialVersionUID = -1330774257555708569L;
	
	private Long id;
	private String name;
	private String image;
	private String webSite;
	
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
	public String getImage() {
		return image;
	}
	public void setImage(String image) {
		this.image = image;
	}
	public String getWebSite() {
		return webSite;
	}
	public void setWebSite(String webSite) {
		this.webSite = webSite;
	}
}
