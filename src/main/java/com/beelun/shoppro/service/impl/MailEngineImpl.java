package com.beelun.shoppro.service.impl;

import java.io.IOException;
import java.util.Map;

import javax.mail.MessagingException;
import javax.mail.internet.MimeMessage;

import org.apache.commons.lang.StringUtils;
import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;

import org.springframework.core.io.ClassPathResource;
import org.springframework.mail.MailException;
import org.springframework.mail.MailSender;
import org.springframework.mail.SimpleMailMessage;
import org.springframework.mail.javamail.JavaMailSenderImpl;
import org.springframework.mail.javamail.MimeMessageHelper;
import org.springframework.ui.freemarker.FreeMarkerTemplateUtils;

import com.beelun.shoppro.service.MailEngine;

import freemarker.cache.StringTemplateLoader;
import freemarker.template.Configuration;
import freemarker.template.Template;
import freemarker.template.TemplateException;

/**
 * Class for sending e-mail messages based on FreeMarker templates or with
 * attachments.
 * 
 */
public class MailEngineImpl implements MailEngine {
	private final Log log = LogFactory.getLog(MailEngineImpl.class);

	private MailSender mailSender;
	private String defaultFrom;
	private Configuration configuration;

	public void setMailSender(MailSender mailSender) {
		this.mailSender = mailSender;
	}

	public void setFrom(String from) {
		this.defaultFrom = from;
	}
	
    public void setFreemarkerMailConfiguration(Configuration configuration) {
        this.configuration = configuration;
    }

	/**
	 * Send a simple message based on a Velocity template.
	 * (Get template from file system)
	 * 
	 * @param msg
	 *            the message to populate
	 * @param templateName
	 *            the Velocity template to use (relative to classpath)
	 * @param model
	 *            a map containing key/value pairs
	 */
	@SuppressWarnings("unchecked")
	public void sendMessage(SimpleMailMessage msg, String templateName,
			Map model) {
		String result = null;

		try {
			Template template = this.configuration.getTemplate(templateName);
			result = FreeMarkerTemplateUtils.processTemplateIntoString(
					template, model);
		} catch (TemplateException e) {
			e.printStackTrace();
			log.error(e.getMessage());
		} catch (Exception e) {
			e.printStackTrace();
			log.error(e.getMessage());
		}

		msg.setText(result);
		send(msg);
	}

	/**
	 * Send message
	 * (Usually get template from DB, and pass that as templateString parameter )
	 * 
	 */
	@SuppressWarnings("unchecked")
	@Override
	public void SendMessageByStringSource(SimpleMailMessage msg, String templateString, Map model) {
		String result = null;

		try {
			Template template = this.convertStringToTemplate(templateString);
			result = FreeMarkerTemplateUtils.processTemplateIntoString(template, model);
		} catch (TemplateException e) {
			e.printStackTrace();
			log.error(e.getMessage());
		} catch (Exception e) {
			e.printStackTrace();
			log.error(e.getMessage());
		}

		log.info("Mail content: " + result);
		msg.setText(result);
		if(StringUtils.isBlank(msg.getFrom())) {
			msg.setFrom(this.defaultFrom);
		}
		send(msg);
	}
	
	
	private Template convertStringToTemplate(String templateString) {
	    final String tempTemplateName = "TEMP_TEMPLATE_NAME";

	    StringTemplateLoader stringTemplateLoader = new StringTemplateLoader();
	    stringTemplateLoader.putTemplate(tempTemplateName, templateString);
	    this.configuration.setTemplateLoader(stringTemplateLoader);

	    try {
	        return this.configuration.getTemplate(tempTemplateName);
	    }
	    catch (IOException e) {
	        // Should never happen
	    	log.debug("something unexpected happens");
	        return null;
	    }
	}
	

	/**
	 * Send a simple message with pre-populated values.
	 * 
	 * @param msg
	 *            the message to send
	 * @throws org.springframework.mail.MailException
	 *             when SMTP server is down
	 */
	public void send(SimpleMailMessage msg) throws MailException {
		// TODO: is it a good idea to swallow the exception here?
		// We might need notify the admin
		try {
			mailSender.send(msg);
		} catch (MailException ex) {
			log.error(ex.getMessage());
			// Don't throw this out
			// throw ex;
		} catch(Exception ex) {
			log.error(ex.getMessage());
		}
	}

	/**
	 * Convenience method for sending messages with attachments.
	 * 
	 * @param recipients
	 *            array of e-mail addresses
	 * @param sender
	 *            e-mail address of sender
	 * @param resource
	 *            attachment from classpath
	 * @param bodyText
	 *            text in e-mail
	 * @param subject
	 *            subject of e-mail
	 * @param attachmentName
	 *            name for attachment
	 * @throws MessagingException
	 *             thrown when can't communicate with SMTP server
	 */
	public void sendMessage(String[] recipients, String sender,
			ClassPathResource resource, String bodyText, String subject,
			String attachmentName) throws MessagingException {
		MimeMessage message = ((JavaMailSenderImpl) mailSender)
				.createMimeMessage();

		// use the true flag to indicate you need a multipart message
		MimeMessageHelper helper = new MimeMessageHelper(message, true);

		helper.setTo(recipients);

		// use the default sending if no sender specified
		if (sender == null) {
			helper.setFrom(defaultFrom);
		} else {
			helper.setFrom(sender);
		}

		helper.setText(bodyText);
		helper.setSubject(subject);

		helper.addAttachment(attachmentName, resource);

		((JavaMailSenderImpl) mailSender).send(message);
	}

}
