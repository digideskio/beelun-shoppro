package com.beelun.shoppro.service;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.test.AbstractTransactionalDataSourceSpringContextTests;

import com.beelun.shoppro.model.Category;
import com.beelun.shoppro.model.Tab;
import com.beelun.shoppro.model.Tab2CategoryMap;

public class TabMapManagerTest extends
		AbstractTransactionalDataSourceSpringContextTests {
	private transient final Log log = LogFactory
			.getLog(TabMapManagerTest.class);

	private Tab2CategoryMapManager tab2CategoryMapManager;

	public void setTab2CategoryMapManager(
			Tab2CategoryMapManager tab2CategoryMapManager) {
		this.tab2CategoryMapManager = tab2CategoryMapManager;
	}

	protected String[] getConfigLocations() {
		setAutowireMode(AUTOWIRE_BY_NAME);
		return new String[] { "classpath*:/WEB-INF/applicationContext*.xml" };
	}

	protected void onSetUpInTransaction() throws Exception {
		// Due to DB constraints, it is not deletable now.
		// deleteFromTables(new String[] {"shoppro_user"});
	}

	public void testSaveLazy() {
		Tab tab = new Tab();
		tab.setId(new Long(1));

		Category c = new Category();
		c.setId(new Long(1));

		Tab2CategoryMap tabMap = new Tab2CategoryMap();
		tabMap.setTab(tab);
		tabMap.setCategory(c);
		tabMap.setCategoryOrder(new Long(1));
		Tab2CategoryMap newTabMap = tab2CategoryMapManager.saveLazy(tabMap);

		assertTrue(newTabMap.getId() != null);
		log.info("done successfully.");
	}
}
