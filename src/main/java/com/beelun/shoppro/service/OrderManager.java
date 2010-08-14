package com.beelun.shoppro.service;

import java.util.Date;
import java.util.List;

import com.beelun.shoppro.model.Order;
import com.beelun.shoppro.model.User;
import com.beelun.shoppro.model.type.OrderStatusEnum;
import com.beelun.shoppro.pojo.ChartDataPointModel;
import com.beelun.shoppro.pojo.TimeAccuracyEnum;

public interface OrderManager {
	public Order get(Long orderId);
	public Order save(Order order);
	public List<Order> getOrderListByUser(User user);
	public List<Order> getAll();
	
	/**
	 * Get order list by status
	 * @param orderStatus
	 * @return
	 */
	public List<Order> getOrderListByStatus(OrderStatusEnum orderStatus);
	
	/**
	 * Get all new(unpaid) order
	 */
	public List<Order> getNewOrder();
	
	/**
	 * Get all paid order 
	 */
	public List<Order> getPaidOrder();

	/**
	 * NON-closed order which requires admin's attention
	 * 
	 * @return
	 */
	public List<Order> getActiveOrder();
	
    /**
     * Get a list of Order according to certain condition
     * 
     * @param orderBy
     * @param ascending
     * @param firstResult
     * @param maxResult
     * @return
     */
	public List<Order> getByCondition(String orderBy, boolean ascending, int firstResult, int maxResult);
	public int getAllCount();
	
	/**
	 * Get sales distribution by brand
	 * 
	 * @param specifiedTime
	 * 			A time span to calculate
	 * @param accuracy
	 * 			Determine the time accuracy of 'specifiedTime'
	 * @return
	 * 			A list of <String, long>=<Brand name, sales in $>
	 */
	public List<ChartDataPointModel> getSalesByBrand(Date specifiedTime, TimeAccuracyEnum accuracy);
	
	/**
	 * Get the order by order number
	 * 
	 * @param orderNo
	 * @return
	 */
	public Order getByOrderNo(String orderNo);

	/**
	 * Change order status
	 * 
	 * @param orderId
	 * 				The Id of order which will be changed
	 * @param newStatus
	 * 				The new status
	 * @return
	 * 				The changed order
	 */
	public Order changeOrderStatus(Long orderId, OrderStatusEnum newStatus);
}
