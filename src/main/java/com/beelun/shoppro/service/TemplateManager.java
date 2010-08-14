package com.beelun.shoppro.service;

import java.util.List;

import com.beelun.shoppro.model.Template;
import com.beelun.shoppro.model.type.TemplateTypeEnum;

public interface TemplateManager {
	public Template save(Template template);
	public List<Template> GetByType(TemplateTypeEnum templateType);
	public List<Template> GetAll();
}
