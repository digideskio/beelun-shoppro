package com.beelun.shoppro.service;

import java.util.List;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.test.AbstractTransactionalDataSourceSpringContextTests;

import com.beelun.shoppro.model.PaymentTool;

public class PaymentToolManagerTest extends
		AbstractTransactionalDataSourceSpringContextTests {

	private transient final Log log = LogFactory.getLog(PaymentToolManagerTest.class);
	
	private PaymentToolManager paymentToolManager;
	
	public void setPaymentToolManager(PaymentToolManager paymentToolManager) {
		this.paymentToolManager = paymentToolManager;
	}
	
    protected String[] getConfigLocations() {
        setAutowireMode(AUTOWIRE_BY_NAME);
        return new String[] {"classpath*:/WEB-INF/applicationContext*.xml"};
    }
    
    @SuppressWarnings("unchecked")
	public void testMe() {
    	List<PaymentTool> l = paymentToolManager.getAllAvailable();
    	assertTrue(l.size() == 2);
    	log.debug("paymentTool[0].name: " + l.get(0).getName());    	
    }
}
