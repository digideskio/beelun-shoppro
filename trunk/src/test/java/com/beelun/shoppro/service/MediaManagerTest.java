package com.beelun.shoppro.service;

import java.io.File;

import org.springframework.test.AbstractTransactionalDataSourceSpringContextTests;

import com.beelun.shoppro.SuperContainer;
import com.beelun.shoppro.model.Media;

public class MediaManagerTest extends AbstractTransactionalDataSourceSpringContextTests {
	private MediaManager mediaManager;
	
	public void setMediaManager(MediaManager mediaManager) {
		this.mediaManager = mediaManager;
	}
	
    protected String[] getConfigLocations() {
        setAutowireMode(AUTOWIRE_BY_NAME);
        return new String[] {"classpath*:/WEB-INF/applicationContext*.xml"};
    }

    protected void onSetUpInTransaction() throws Exception {
        // deleteFromTables(new String[] {"shoppro_tab"});
    }
    
    public void testCreateNew()
    {    	
    	SuperContainer.BaseDir = System.getProperty("user.dir") + "/sdfgsdfg3432";
    	new File(SuperContainer.BaseDir).mkdir();
    	new File(SuperContainer.BaseDir + "/uploads").mkdir();
    	Media media = new Media("a.jpg", "t1", "c1", "d1");
    	byte[] content = new byte[] {'a', 'b', 'c'};
    	assertTrue(mediaManager.createNew(media, content) != null);
    	new File(SuperContainer.BaseDir + "/sdfgsdfg3432").delete();
    }

}
