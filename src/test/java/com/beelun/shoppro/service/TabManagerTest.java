package com.beelun.shoppro.service;

import org.springframework.test.AbstractTransactionalDataSourceSpringContextTests;

/**
 * Test TabManager
 * @author Bill Li(bill@beelun.com)
 *
 */
public class TabManagerTest extends AbstractTransactionalDataSourceSpringContextTests {
	private TabManager tabManager;
	
	public void setTabManager(TabManager tabManager) {
		this.tabManager = tabManager;
	}
	
    protected String[] getConfigLocations() {
        setAutowireMode(AUTOWIRE_BY_NAME);
        return new String[] {"classpath*:/WEB-INF/applicationContext*.xml"};
    }

    protected void onSetUpInTransaction() throws Exception {
        // deleteFromTables(new String[] {"shoppro_tab"});
    }
    
    public void testGetAllTabs() {
    	assertTrue(this.tabManager.getAll().size() == 7);
    	assertTrue(this.tabManager.getShownTabs().size() == 6);
    }
}
