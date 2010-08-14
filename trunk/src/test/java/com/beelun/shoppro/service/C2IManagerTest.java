package com.beelun.shoppro.service;

import java.util.ArrayList;
import java.util.List;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.test.AbstractTransactionalDataSourceSpringContextTests;

import com.beelun.shoppro.pojo.MappingStatusEnum;

public class C2IManagerTest extends
		AbstractTransactionalDataSourceSpringContextTests {
	private transient final Log log = LogFactory.getLog(C2IManagerTest.class);

	private Category2ItemMapManager c2iManager;
	
	public void setCategory2ItemMapManager(Category2ItemMapManager c2iManager) {
		this.c2iManager = c2iManager;
	}
	
    protected String[] getConfigLocations() {
        setAutowireMode(AUTOWIRE_BY_NAME);
        return new String[] {"classpath*:/WEB-INF/applicationContext*.xml"};
    }

    public void testDeleteByItems()
    {    
    	List<Long> idList = new ArrayList<Long>();
    	idList.add(new Long(0));
    	idList.add(new Long(1));
    	c2iManager.deleteByItems(idList.toArray((new Long[] {})));
    	assertTrue(c2iManager.getAll().size() == 2);
    	log.debug("pass");
    }
    
    public void testGetMappingStatus()
    {
    	// All
    	List<Long> l = new ArrayList<Long>();
    	l.add(new Long(0));
    	l.add(new Long(1));
    	MappingStatusEnum s1 = c2iManager.getMappingStatus(new Long(1), l);
    	assertTrue(s1 == MappingStatusEnum.ALL);
    	
    	// None
    	l.clear();
    	l.add(new Long(1000000));
    	MappingStatusEnum s2 = c2iManager.getMappingStatus(new Long(1), l);
    	assertTrue(s2 == MappingStatusEnum.NONE);
    	
    	// Partial
    	l.clear();
    	l.add(new Long(0));
    	l.add(new Long(1));
    	l.add(new Long(1000000));
    	MappingStatusEnum s3 = c2iManager.getMappingStatus(new Long(1), l);
    	assertTrue(s3 == MappingStatusEnum.PARTIAL);    	
    }
}
