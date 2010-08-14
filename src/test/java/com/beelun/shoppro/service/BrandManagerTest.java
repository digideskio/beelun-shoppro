package com.beelun.shoppro.service;

import org.springframework.test.AbstractTransactionalDataSourceSpringContextTests;

import com.beelun.shoppro.model.Brand;

public class BrandManagerTest extends AbstractTransactionalDataSourceSpringContextTests {
	private BrandManager brandManager;
	
	public void setBrandManager(BrandManager brandManager) {
		this.brandManager = brandManager;
	}
	
    protected String[] getConfigLocations() {
        setAutowireMode(AUTOWIRE_BY_NAME);
        return new String[] {"classpath*:/WEB-INF/applicationContext*.xml"};
    }
    
    public void testPosotive() {
    	Brand brand = this.brandManager.getByName("Spirent");
    	assertTrue(brand.getImage().equals("1.jpg"));
    }
    
    public void testgetOrCreateByName() {
    	// 1. if not exist, should create a new one
    	Brand brand = this.brandManager.getOrCreateByName("notexitss");
    	assertTrue(brand != null);
    	assertTrue(this.brandManager.get(brand.getId()).getName().equals("notexitss"));
    	
    	// 2. ignore case
    	Brand brand2 = this.brandManager.getOrCreateByName("NotExitss");
    	assertTrue(brand == brand2);
    }

}
