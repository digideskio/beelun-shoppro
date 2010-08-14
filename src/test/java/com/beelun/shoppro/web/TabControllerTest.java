package com.beelun.shoppro.web;

import java.util.ArrayList;
import java.util.List;

import org.apache.log4j.Logger;
import org.jmock.Mock;
import org.jmock.MockObjectTestCase;
import org.springframework.mock.web.MockHttpServletRequest;
import org.springframework.mock.web.MockHttpServletResponse;
import org.springframework.web.servlet.ModelAndView;

import com.beelun.shoppro.model.Category;
import com.beelun.shoppro.model.Tab;
import com.beelun.shoppro.service.CategoryManager;
import com.beelun.shoppro.service.TabManager;

public class TabControllerTest extends MockObjectTestCase {
	private static final Logger log = Logger.getLogger(TabControllerTest.class);
	
	private TabController t = new TabController();
	private Mock mockManager = null;
	private Mock mockManagerC = null;
	private MockHttpServletRequest request = null;
	private ModelAndView mv = null;
	
	
    protected void setUp() throws Exception {
        mockManager = new Mock(TabManager.class);
        mockManagerC = new Mock(CategoryManager.class);
        t.tabManager = (TabManager) mockManager.proxy();
        t.categoryManager = (CategoryManager)mockManagerC.proxy();
    }
    
    /**
     * Index page
     */
    public void testGetIndexPage() {        
        log.info("todo.");
    }
    
    /**
     * Tab page
     * @throws Exception 
     */
    public void testTabPage() throws Exception {
    	Tab tab = new Tab();
    	tab.setUrl("brand.html");
    	mockManager.expects(once()).method("get").will(returnValue(tab));;
    	
    	Category c = new Category();
    	List<Category> l = new ArrayList<Category>();
    	l.add(c);
    	mockManagerC.expects(once()).method("getShown").will(returnValue(l));

    	request = new MockHttpServletRequest("GET", "/category/showTab.html");
    	request.addParameter("tabUrl", "brand.html");
        mv = t.handleRequest(request, new MockHttpServletResponse());
        assertEquals("tabPage", mv.getViewName());
        
        mockManager.verify();
        mockManagerC.verify();
        
        log.info("done.");    	
    }
}
