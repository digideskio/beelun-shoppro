package com.beelun.shoppro.model;

import java.io.File;
import java.util.Date;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlTransient;

import com.beelun.shoppro.SuperContainer;

/**
 * Model class to store meta data of image, video, flash, etc.
 * We only support image at this moment. 
 * 
 * @author Bill Li(bill@beelun.com)
 *
 */
public class Media extends BaseObject {
	private static final long serialVersionUID = 884241799377578342L;
	
	private Long id;
	private String fileName;
	private String title;	// To indicate what this image is about
	private String caption;
	private String description;
	private Date updated = new Date();
	
	public Media() {}
	
	public Media(String fileName, String title, String caption, String description) {
		this.fileName = fileName;
		this.title = title;
		this.caption = caption;
		this.description = description;
		this.setUpdated();
	}
	
	/**
	 * Get the full url to be shown in the site
	 * 
	 * @return
	 */
	@XmlElement
	public String getMyUrl() {
		String ret = null;
		if (null != getId()) {
			ret = String.format("%s/uploads/%s", SuperContainer.BaseUrl, getFileName());			
		}

		return ret;
	}	
	
	@XmlTransient
	public String getFileAbsolutePath()
	{
		String pathSep = File.separator; // Make it system independent
		return SuperContainer.BaseDir + pathSep +"uploads" + pathSep + getFileName();
	}
	
	public Long getId() {
		return id; 
	}
	public void setId(Long id) {
		this.id = id;
	}
	public String getTitle() {
		return title;
	}
	public void setTitle(String title) {
		this.title = title;
	}
	public String getCaption() {
		return caption;
	}
	public void setCaption(String caption) {
		this.caption = caption;
	}
	public String getDescription() {
		return description;
	}
	public void setDescription(String description) {
		this.description = description;
	}
	
	public String getFileName() {
		return fileName;
	}
	public void setFileName(String fileName) {
		this.fileName = fileName;
	}

	public Date getUpdated() {
		return updated;
	}

	public void setUpdated(Date updated) {
		this.updated = updated;
	}	
	public void setUpdated() {
		setUpdated(new Date());
	}
	
}
