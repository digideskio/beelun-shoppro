package com.beelun.shoppro.service;

import java.util.List;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.test.AbstractTransactionalDataSourceSpringContextTests;

import com.beelun.shoppro.model.ExpressCorp;

/**
 * Test 'ExpressCorpManager'
 * 
 * @author Bill Li(bill@beelun.com)
 *
 */
public class ExpressCorpManagerTest extends
		AbstractTransactionalDataSourceSpringContextTests {
	private transient final Log log = LogFactory.getLog(ExpressCorpManagerTest.class);
	
	private ExpressCorpManager expressCorpManager;
	
	public void setExpressCorpManager(ExpressCorpManager expressCorpManager) {
		this.expressCorpManager = expressCorpManager;
	}
	
    protected String[] getConfigLocations() {
        setAutowireMode(AUTOWIRE_BY_NAME);
        return new String[] {"classpath*:/WEB-INF/applicationContext*.xml"};
    }
	
    @SuppressWarnings("unchecked")
	public void testMe() {
    	List<ExpressCorp> l = expressCorpManager.getAllAvailable();
    	assertTrue(l.size() == 2);
    	log.debug("expressCorp[0].name: " + l.get(0).getShortName());
    }
}
