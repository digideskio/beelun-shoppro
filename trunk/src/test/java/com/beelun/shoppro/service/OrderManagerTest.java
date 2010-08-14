package com.beelun.shoppro.service;

import java.util.List;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.test.AbstractTransactionalDataSourceSpringContextTests;

import com.beelun.shoppro.model.type.OrderStatusEnum;

public class OrderManagerTest extends
		AbstractTransactionalDataSourceSpringContextTests {
	private transient final Log log = LogFactory.getLog(OrderManagerTest.class);
	
	private OrderManager orderManager;
	
    public void setOrderManager(OrderManager orderManager) {
		this.orderManager = orderManager;
	}

	protected String[] getConfigLocations() {
        setAutowireMode(AUTOWIRE_BY_NAME);
        return new String[] {"classpath*:/WEB-INF/applicationContext*.xml"};
    }

    protected void onSetUpInTransaction() throws Exception {
        // deleteFromTables(new String[] {"shoppro_tab"});
    }

    @SuppressWarnings("unchecked")
	public void testGetOrderByStatus() 
    {
    	List l = orderManager.getOrderListByStatus(OrderStatusEnum.NOTPAID);
    	assertTrue(l.size() == 1);
    	log.info("done");
    }
}
