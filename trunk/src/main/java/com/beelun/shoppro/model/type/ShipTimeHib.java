package com.beelun.shoppro.model.type;

/**
 * This class is used only in the Hibernate XML configuration
 * @author bali
 *
 */
public class ShipTimeHib extends IntEnumUserType<ShipTimeEnum> {
	public ShipTimeHib() {
		super(ShipTimeEnum.class, ShipTimeEnum.values());
	}
}
