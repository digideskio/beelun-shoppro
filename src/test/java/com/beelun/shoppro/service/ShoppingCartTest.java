package com.beelun.shoppro.service;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.test.AbstractTransactionalDataSourceSpringContextTests;

import com.beelun.shoppro.service.impl.ShoppingCartImpl;
import com.beelun.shoppro.utils.GeneralUtils;

public class ShoppingCartTest extends
		AbstractTransactionalDataSourceSpringContextTests {
	private transient final Log log = LogFactory.getLog(ShoppingCartTest.class);	

	private ShoppingCart shoppingCart;
	private ItemManager itemManager;
	
	public void setItemManager(ItemManager itemManager) {
		this.itemManager = itemManager;
	}
	
    protected String[] getConfigLocations() {
        setAutowireMode(AUTOWIRE_BY_NAME);
        return new String[] {"classpath*:/WEB-INF/applicationContext*.xml"};
    }
    
    public void testMe() {
    	shoppingCart = new ShoppingCartImpl(this.itemManager);
    	
    	// Add one
    	shoppingCart.add(new Long(0), new Long(1));
    	shoppingCart.add(new Long(0), new Long(1));
    	log.debug("total Items: " + shoppingCart.getItemNumber());
    	assertTrue(shoppingCart.getItemNumber() == 2);
    	
    	// Remove
    	shoppingCart.remove(new Long(0), new Long(1));
    	assertTrue(shoppingCart.getItemNumber() == 1);
    	
    	// getItemList
    	shoppingCart.add(new Long(1), new Long(2));
    	assertTrue(shoppingCart.getItemNumber() == 3);
    	
    	// getItemTotalValue, 1 item0, 2 item1
    	assertTrue(GeneralUtils.floatEqual(shoppingCart.getItemTotalValue(), 286.97f));
    	
    	// clearOne
    	shoppingCart.clearOne(new Long(1));
    	assertTrue(shoppingCart.getItemNumber() == 1);
    	
    	// Clear all
    	shoppingCart.clearAll();
    	assertTrue(shoppingCart.getItemNumber() == 0);    	   
    	
    	// Remove multiple items
    	shoppingCart.add(new Long(0), new Long(2));
    	shoppingCart.add(new Long(0), new Long(1));
    	shoppingCart.remove(new Long(0), new Long(3));
    	assertTrue(shoppingCart.getItemNumber() == 0);
    	assertTrue(shoppingCart.getCartItemList().size() == 0);

    	shoppingCart.add(new Long(1), new Long(2));
    	shoppingCart.remove(new Long(1), new Long(2));
    	assertTrue(shoppingCart.getItemNumber() == 0);
    	assertTrue(shoppingCart.getCartItemList().size() == 0);
    	
    	// setItems
    	shoppingCart.clearAll();
    	shoppingCart.add(new Long(0), new Long(1));
    	shoppingCart.add(new Long(0), new Long(1));
    	shoppingCart.setItems(new Long(0), new Long(3));
    	assertTrue(shoppingCart.getItemNumber() == 3);
    	shoppingCart.setItems(new Long(1), new Long(5));
    	assertTrue(shoppingCart.getItemNumber() == 8);
    }
}
