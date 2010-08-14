package com.beelun.shoppro.model.type;

/**
 * For hibernate mapping
 * 
 * @author bali
 *
 */
public class SiteTypeHib extends IntEnumUserType<SiteTypeEnum> {
	public SiteTypeHib() {
		super(SiteTypeEnum.class, SiteTypeEnum.values());
	}
}
