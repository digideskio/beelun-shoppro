package com.beelun.shoppro.web;

import org.apache.log4j.Logger;
import org.jmock.Mock;
import org.jmock.MockObjectTestCase;
import org.springframework.mock.web.MockHttpServletRequest;
import org.springframework.mock.web.MockHttpServletResponse;
import org.springframework.web.servlet.ModelAndView;

import com.beelun.shoppro.model.Item;
import com.beelun.shoppro.service.ItemManager;

public class ItemControllerTest extends MockObjectTestCase {
	private static final Logger log = Logger.getLogger(ItemControllerTest.class);
	
	private ItemController iContoller = new ItemController();
	private Mock iManager = null;
	
    protected void setUp() throws Exception {
    	iManager = new Mock(ItemManager.class);
    	iContoller.itemManager = (ItemManager)iManager.proxy();
    }
    
    public void testMe() throws Exception {
    	Item i = new Item();    	
    	iManager.expects(once()).method("get").will(returnValue(i));
    	
    	MockHttpServletRequest request = new MockHttpServletRequest("GET", "/aaa.html");;
    	ModelAndView mv = iContoller.handleRequest(request, new MockHttpServletResponse());
    	assertEquals("uploads/template/defaultItemTemplate", mv.getViewName());
        
        iManager.verify();
        log.info("done");
    }
}
