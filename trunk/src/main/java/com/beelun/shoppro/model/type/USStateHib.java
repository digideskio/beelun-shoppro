package com.beelun.shoppro.model.type;

/**
 * For hibernate mapping of USStateEnum
 * 
 * @author bali
 *
 */
public class USStateHib extends IntEnumUserType<USStateEnum> {
	public USStateHib() {
		super(USStateEnum.class, USStateEnum.values());
	}
}
