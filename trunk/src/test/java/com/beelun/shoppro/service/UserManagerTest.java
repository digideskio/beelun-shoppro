package com.beelun.shoppro.service;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.security.userdetails.UsernameNotFoundException;
import org.springframework.test.AbstractTransactionalDataSourceSpringContextTests;

import com.beelun.shoppro.model.User;
import com.beelun.shoppro.service.UserManager;

public class UserManagerTest extends AbstractTransactionalDataSourceSpringContextTests {
	private transient final Log log = LogFactory.getLog(UserManagerTest.class);

	private UserManager userManager;

    public void setUserManager(UserManager userManager) {
        this.userManager = userManager;
    }

    protected String[] getConfigLocations() {
        setAutowireMode(AUTOWIRE_BY_NAME);
        return new String[] {"classpath*:/WEB-INF/applicationContext*.xml"};
    }

    protected void onSetUpInTransaction() throws Exception {
    	// Due to DB constraints, it is not deletable now.
        // deleteFromTables(new String[] {"shoppro_user"});
    }

    public void testGetUsers() {
    	long oldValue = userManager.getUsers().size();
    	
        User kid1 = new User();
        kid1.setName("Abbie");
        kid1.setEmail("a2@b.com");
        kid1.setPassword("password");
        
        User kid2 = new User();
        kid2.setName("Jack");
        kid2.setEmail("a3@b.com"); // unique
        kid2.setPassword("password");
        
        userManager.saveUser(kid1, true);
        userManager.saveUser(kid2, true);

        // Verify that we get another two new users
        assertEquals(oldValue + 2, userManager.getUsers().size());
    }
    
    public void testGetUserAddress() {
    	User u = userManager.getUserById("1");
    	assertTrue(u.getShippingAddress().getName().equals("Office's address"));
    }
    
    /**
     * Verify that locked user can not be loaded
     */
    public void testLockedUser() {
    	boolean bGetExp = false;
    	try {
    	userManager.loadUserByUsername("test2@beelun.com");
    	}catch(UsernameNotFoundException e) {
    		bGetExp = true;
    		log.info("get expected exception.");
    	} finally {
    		assertTrue(bGetExp==true);
    	}
    }
    
    public void testGetAllCount()
    {
    	assertTrue(userManager.getAllCount() == 5);
    }
}
