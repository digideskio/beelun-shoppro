package com.beelun.shoppro.service;

import java.util.List;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.test.AbstractTransactionalDataSourceSpringContextTests;

import com.beelun.shoppro.model.Category;
import com.beelun.shoppro.model.Tab;

public class CategoryManagerTest extends AbstractTransactionalDataSourceSpringContextTests {
	private transient final Log log = LogFactory.getLog(CategoryManagerTest.class);
	
	private CategoryManager categoryManager;
	
	public void setCategoryManager(CategoryManager categoryManager) {
		this.categoryManager = categoryManager;
	}
	
    protected String[] getConfigLocations() {
        setAutowireMode(AUTOWIRE_BY_NAME);
        return new String[] {"classpath*:/WEB-INF/applicationContext*.xml"};
    }

    public void testGetAllShown() {
    	Tab t = new Tab();
    	t.setId(new Long(2)); // Product
    	assertTrue(this.categoryManager.getAll().size() == 4);
    	List<Category> l = this.categoryManager.getShown(t);
    	log.debug("all shown catogry #: " + l.size());
    	assertTrue(l.size() == 4);
    	assertTrue(l.get(0).getName().equals("Spirent"));
    }
    
    public void testGetByCondition()
    {
    	List<Category> l = this.categoryManager.getByCondition("id", false, 0, 3);
    	assertTrue(l.size() == 3);
    	assertTrue(l.get(0).getId() == 4);    	
    }
}
