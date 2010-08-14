package com.beelun.shoppro.service.impl;

import java.io.File;
import java.io.FileOutputStream;
import java.io.OutputStream;
import java.nio.charset.Charset;
import java.util.List;

import org.apache.commons.lang.RandomStringUtils;
import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.beelun.shoppro.dao.hibernate.GenericDaoHibernate;
import com.beelun.shoppro.model.Template;
import com.beelun.shoppro.model.type.TemplateTypeEnum;
import com.beelun.shoppro.service.TemplateManager;

@Service(value = "templateManager")
public class TemplateManagerImpl extends GenericDaoHibernate<Template, Long>
		implements TemplateManager {

	@Autowired
	public TemplateManagerImpl(SessionFactory sessionFactory) {
		super(Template.class, sessionFactory);
	}

	@SuppressWarnings("unchecked")
	@Override
	public List<Template> GetByType(TemplateTypeEnum templateType) {
		return (List<Template>) getHibernateTemplate().find(
				"from Template t where t.templateType = ?", templateType);
	}

	public Template save(Template template) {
		// Generate fileNamePrefix for new template
		if (null == template.getFileNamePrefix()) {
			template
					.setFileNamePrefix(RandomStringUtils.random(16, true, true));
		}

		// Save to DB
		Template t = super.save(template);

		// Save content to file system
		if (t != null) {
			// Save content to file
			try {
				// Save file now. We also need make sure file update works here.
				File f = new File(t.getFileAbsolutePath());
				OutputStream outputStream = new FileOutputStream(f);
				byte[] contentBytes = t.getTemplateContent().getBytes(
						Charset.forName("utf-8"));
				outputStream.write(contentBytes, 0, contentBytes.length);
				outputStream.close();
			} catch (Exception ex) {
				log
						.error("Error during save template. Msg: "
								+ ex.getMessage());
				super.remove(t.getId());
				return null;
			}
		}
		// return
		return t;
	}

	@Override
	public List<Template> GetAll() {
		return super.getAll();
	}
}
