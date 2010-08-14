package com.beelun.shoppro.service;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.test.AbstractTransactionalDataSourceSpringContextTests;

import com.beelun.shoppro.model.Item;
import com.beelun.shoppro.utils.GeneralUtils;

public class ItemManagerTest extends AbstractTransactionalDataSourceSpringContextTests {
	private transient final Log log = LogFactory.getLog(ItemManagerTest.class);

	private ItemManager itemManager;
	
	public void setItemManager(ItemManager itemManager) {
		this.itemManager = itemManager;
	}
	
    protected String[] getConfigLocations() {
        setAutowireMode(AUTOWIRE_BY_NAME);
        return new String[] {"classpath*:/WEB-INF/applicationContext*.xml"};
    }
    
    public void testBasic() {
    	List<Long> l = new ArrayList<Long>();
    	
    	// zero items
    	assertTrue(0 == this.itemManager.getItemTotalValue(l));
    	List<Item> itemList = this.itemManager.getByIdList(l); 
    	assertTrue(itemList == null || itemList.isEmpty());    	
    	
    	// Two items
    	l.add(new Long(0));
    	l.add(new Long(1));    	
    	assertTrue(this.itemManager.getByIdList(l).size() == 2);
    	float totalValue = this.itemManager.getItemTotalValue(l);
    	log.debug("totalValue: " + totalValue);
    	assertTrue(GeneralUtils.floatEqual(totalValue, 157.98f));
    	
    	// Not exist items
    	l.clear();
    	l.add(new Long(9999));
    	l.add(new Long(8888));
    	assertTrue(this.itemManager.getByIdList(l).size() == 0);
    	float totalValue2 = this.itemManager.getItemTotalValue(l);
    	log.debug("totalValue: " + totalValue2);
    	assertTrue(GeneralUtils.floatEqual(totalValue2, 0f));

    	log.debug("itemManager.getByIdList() tests ok.");
    }    
    
    public void testSave() {
    	Item item = new Item();
    	item.setName("testSave");
    	item.setBrand(null);
    	item.setInventoryNumber(new Long(3));
    	item.setSellPrice(new BigDecimal(100f));
    	item.setPageTitle("t");
    	item.setUrl("url");
    	Item newItem = itemManager.save(item);
    	log.debug("new item id: " + newItem.getId());
    	
    	List<Item> itemAll = itemManager.getAll();
    	assertTrue(itemAll.size() == 8);
    }
    
    public void testRemove() {
    	// Remove one with fk pointing to it, will succeed anyway
    	boolean r = itemManager.removeItem(new Long(0));
    	assertTrue(itemManager.exists(new Long(0)) == false);
    	assertTrue(r == true);
    	
    	// Remove one without fk point to it, success
    	assertTrue(itemManager.removeItem(new Long(3)) == true);
    	
    	// Remove all - without fk point to it, success
    	List<Long> idList = new ArrayList<Long>();
    	idList.add(new Long(5));
    	idList.add(new Long(6));
    	assertTrue(itemManager.removeItems(idList.toArray(new Long[] {})) == true);   
    	idList.clear();
   
    	// Remove all -  with fk pointing to it, but will succeed
    	idList.add(new Long(1));
    	idList.add(new Long(0));
    	assertTrue(itemManager.removeItems(idList.toArray(new Long[]{})) == true);    	
    }
    
    public void testGetItemByNID() {
    	Item item = itemManager.getItemByNID(new Long(1));
    	assertTrue(item != null);    	
    	assertTrue(itemManager.getItemByNID(new Long(456)) == null);
    }

    public void testSearchByText() {
    	List<Item> l = itemManager.searchByText("Content3", 0, 20);
    	assertTrue(l.size() != 0);
    }
    
    public void testSearchByTextCount() {
    	int count = itemManager.searchByTextCount("Content3");
    	assertTrue(count != 0);    	
    }
}
