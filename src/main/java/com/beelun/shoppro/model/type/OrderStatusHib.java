package com.beelun.shoppro.model.type;

/**
 * This class is used only in the Hibernate XML configuration
 * @author bali
 *
 */
public class OrderStatusHib extends IntEnumUserType<OrderStatusEnum> {
	public OrderStatusHib() {
		super(OrderStatusEnum.class, OrderStatusEnum.values());
	}
}
