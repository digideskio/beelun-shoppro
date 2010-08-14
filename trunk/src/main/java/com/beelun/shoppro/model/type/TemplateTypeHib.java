package com.beelun.shoppro.model.type;

public class TemplateTypeHib extends IntEnumUserType<TemplateTypeEnum> {
	public TemplateTypeHib() {
		super(TemplateTypeEnum.class, TemplateTypeEnum.values());
	}
}
