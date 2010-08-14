package com.beelun.shoppro.web;

import java.util.Map;

import org.apache.log4j.Logger;

//import com.gargoylesoftware.htmlunit.WebClient;

import net.sourceforge.jwebunit.junit.WebTestCase; 
import net.sourceforge.jwebunit.util.TestingEngineRegistry;

/**
 * Test readonly visitor
 *  
 * @author Bill Li(bill@beelun.com)
 *
 */
public class VistorWebTest extends WebTestCase {
	private static final Logger log = Logger.getLogger(VistorWebTest.class);
	
    public void setUp() throws Exception {
        super.setUp();
        setTestingEngineKey(TestingEngineRegistry.TESTING_ENGINE_HTMLUNIT);    // use HtmlUnit
        setBaseUrl("http://localhost:25888");
    }
    
    public void testHomePage() {
    	log.info("test home page...");
        beginAt("/");
        String source = getPageSource();
        log.debug(source);
        assertTitleEquals("Home page title");    	
    }
    
    public void testTabPage() throws InterruptedException {
    	log.info("test /tab/brand.html...");
        beginAt("/tab/brand.html");
        Thread.sleep(5000);
        assertTitleEquals("brand page title");
    }

    public void testCategoryPage() {
    	log.info("test /category/sprint.html...");
        beginAt("/category/sprint.html");
        assertTitleEquals("Category 1");    	
    }
    
    public void testItemPage() {
    	log.info("test /item/item-1.html...");
        beginAt("/item/item-1.html");
        assertTitleEquals("good product, item");    	    	
    }
    
    public void testAjaxCart() {
    	log.info("/cart/cartAjax.html");
    	beginAt("/cart/cartAjax.html?itemId=0&action=add");
    	log.info(getPageSource());
    	assertTextPresent("<Code>200</Code>");
    	assertTextPresent("<itemNumber>1</itemNumber>");
    	assertTextPresent("<itemValue>28.99</itemValue>");
    	Map<String, String> headers = getAllHeaders();
    	for(String theKey: headers.keySet()) {
    		log.info(String.format("key: %s, value: %s", theKey, headers.get(theKey)));
    	}
    	assertHeaderEquals("Content-Type", "text/xml;charset=utf-8"); 
    }
    
    public void testItemsInShoppingCart() {
    	log.info("/cart/itemList.html");
    	// TODO: add your test case here.
//    	getTestContext().setBaseUrl("http://localhost:25888");
//    	gotoPage("/cart/cartAjax.html?itemId=0&action=add");
//    	beginAt("/cart/itemList.html");
//    	assertTextPresent("number");
    }
}
