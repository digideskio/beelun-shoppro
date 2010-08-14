package com.beelun.shoppro.service.impl;

import java.util.Date;
import java.util.List;

import org.hibernate.SessionFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.beelun.shoppro.dao.hibernate.GenericDaoHibernate;
import com.beelun.shoppro.model.Order;
import com.beelun.shoppro.model.User;
import com.beelun.shoppro.model.type.OrderStatusEnum;
import com.beelun.shoppro.pojo.ChartDataPointModel;
import com.beelun.shoppro.pojo.TimeAccuracyEnum;
import com.beelun.shoppro.service.OrderManager;

/**
 * Manage 'Order' object
 * 
 * @author <a href="mailto:bill@beelun.com">Bill Li</a>
 * 
 */
@Service(value = "orderManager")
public class OrderManagerImpl extends GenericDaoHibernate<Order, Long> implements OrderManager {

    @Autowired
    public OrderManagerImpl(SessionFactory sessionFactory) {
        super(Order.class, sessionFactory);
    }

    public Order save(Order order) {
        return super.save(order);
    }

    @SuppressWarnings("unchecked")
    public List<Order> getOrderListByUser(User user) {
        return getHibernateTemplate().find("from Order order where order.user=?", user);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public List<Order> getNewOrder() {
        return getOrderListByStatus(OrderStatusEnum.NOTPAID);
    }

    /**
     * {@inheritDoc}
     */
    @Override
    public List<Order> getPaidOrder() {
        return getOrderListByStatus(OrderStatusEnum.PAID);
    }

    /**
     * {@inheritDoc}
     */
    @SuppressWarnings("unchecked")
    @Override
    public List<Order> getOrderListByStatus(OrderStatusEnum orderStatus) {
        return getHibernateTemplate().find("from Order order where order.status=?", orderStatus);
    }

    @SuppressWarnings("unchecked")
    @Override
    public List<Order> getActiveOrder() {
        return getHibernateTemplate().find("from Order order where order.status<>?", OrderStatusEnum.CLOSED);
    }

    @Override
    public List<Order> getByCondition(String orderBy, boolean ascending, int firstResult, int maxResult) {
        return super.getByCondition(orderBy, ascending, firstResult, maxResult);
    }

    @Override
    public int getAllCount() {
        return super.getAllCount();
    }

    @Override
    public List<ChartDataPointModel> getSalesByBrand(Date specifiedTime, TimeAccuracyEnum accuracy) {
        // TODO Auto-generated method stub
        return null;
    }

    @Override
    public Order getByOrderNo(String orderNo) {
        // Order No(or serial number) is in the format of: yyyyMMdd + orderId.
        // So we can pickup out the orderId for orderNo and then get fetch the
        // order according to orderId
        try {
            String orderIdString = orderNo.substring(8); // Size of "yyyyMMdd"
            // is 8
            return get(Long.parseLong(orderIdString));
        } catch (Exception ex) {
            log.error("Fail to get the object from order NO: " + orderNo);
            return null;
        }
    }

    @Override
    public Order changeOrderStatus(Long orderId, OrderStatusEnum newStatus) {
        try {
            Order order = get(orderId);
            if (order != null) {
                order.setStatus(newStatus);
                Order newOrder = save(order);
                return newOrder;
            }
            return null;
        } catch (Exception ex) {
            log.error("Fail to change order status: " + ex.getMessage());
            return null;
        }
    }
}
