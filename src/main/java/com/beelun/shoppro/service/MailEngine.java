package com.beelun.shoppro.service;

import java.util.Map;

import org.springframework.mail.SimpleMailMessage;

public interface MailEngine {

	/**
	 * Send a message
	 * 
	 * @param msg
	 * @param templateName - The final mail is constructed from template in file system indicated by 'templateName'
	 * @param model
	 */
	@SuppressWarnings("unchecked")
	public void sendMessage(SimpleMailMessage msg, String templateName, Map model);
	
	/**
	 * Send a message
	 * 
	 * @param msg
	 * @param templateString - the plain text string of the template
	 * @param model
	 */
	@SuppressWarnings("unchecked")
	public void SendMessageByStringSource(SimpleMailMessage msg, String templateString, Map model);
}
