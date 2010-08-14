package com.beelun.shoppro.pojo;

import org.springframework.web.multipart.MultipartFile;

/**
 * Command class to handle uploading of a file.
 * 
 * <p>
 * <a href="FileUpload.java.html"><i>View Source</i></a>
 * </p>
 * 
 * @author <a href="mailto:bill@beelun.com">Bill Li</a>
 */
public class FileUpload {
	private String name;
	private MultipartFile multipartFile;

	/**
	 * @return Returns the name.
	 */
	public String getName() {
		return name;
	}

	/**
	 * @param name
	 *            The name to set.
	 */
	public void setName(String name) {
		this.name = name;
	}

	public void setFile(MultipartFile file) {
		this.multipartFile = file;
	}

	public MultipartFile getFile() {
		return multipartFile;
	}

}
