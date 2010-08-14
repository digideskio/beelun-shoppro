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
import com.beelun.shoppro.model.Item;
import com.beelun.shoppro.service.CategoryManager;
import com.beelun.shoppro.service.ItemManager;

/**
 * Test Class CategoryController
 * 
 * @author Bill Li(bill@beelun.com)
 *
 */
public class CategoryControllerTest extends MockObjectTestCase {
	private static final Logger log = Logger.getLogger(CategoryControllerTest.class);
	
	private CategoryController categoryController = new CategoryController();
	private Mock mCategory = null;
	private Mock mItem = null;
	private MockHttpServletRequest request = null;
	private ModelAndView mv = null;
	
    protected void setUp() throws Exception {
    	mCategory = new Mock(CategoryManager.class);
    	mItem = new Mock(ItemManager.class);
    	categoryController.categoryManager = (CategoryManager)mCategory.proxy();
    	categoryController.itemManager = (ItemManager) mItem.proxy();
    }
    
	public void testCategoryPage() throws Exception {
    	Category c = new Category();
    	c.setUrl("sprint.html");
    	mCategory.expects(once()).method("get").will(returnValue(c));

    	Item i = new Item();
    	Item i2 = new Item();
    	List<Item> l = new ArrayList<Item>();
    	l.add(i);
    	l.add(i2);
    	mItem.expects(once()).method("getShown").will(returnValue(l));  // return two items
    	
    	request = new MockHttpServletRequest("GET", "/category/showCategory.html");
    	request.addParameter("categoryUrl", "sprint.html");
        // ModelMap map = new ModelMap();
        mv = categoryController.handleRequest(request, new MockHttpServletResponse());
        assertEquals("uploads/template/defaultCategoryTemplate", mv.getViewName());
                        
        mCategory.verify();
        mItem.verify();
        
        log.info("done.");    	
    }
}
