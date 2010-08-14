package com.beelun.shoppro.model.type;

/**
 * This class is used only in the Hibernate XML configuration
 * @author bali
 *
 */
public class ShipDateHib extends IntEnumUserType<ShipDateEnum> {
	public ShipDateHib() {
		super(ShipDateEnum.class, ShipDateEnum.values());
	}
}
