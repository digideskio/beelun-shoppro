package com.beelun.shoppro.dao;

import org.springframework.dao.DataAccessException;

import com.beelun.shoppro.dao.UserDao;
import com.beelun.shoppro.model.User;

public class UserDaoTest extends BaseDaoTestCase {
    private User user = null;
    private UserDao dao = null;

    public void setUserDao(UserDao userDao) {
        this.dao = userDao;
    }

    public void testGetUsers() {
        user = new User();
        user.setName("Rod");
        user.setEmail("a@b.com");
        user.setPassword("password");
        		
        dao.saveUser(user);

        assertTrue(dao.getUsers().size() >= 1);
    }

    public void testSaveUser() throws Exception {
        user = new User();
        user.setName("Rod");
        user.setEmail("a1@b.com");
        user.setPassword("password");

        dao.saveUser(user);
        assertTrue("primary key assigned", user.getId() != null);

        assertNotNull(user.getName());
    }

    public void testAddAndRemoveUser() throws Exception {
        user = new User();
        user.setName("Rod");
        user.setEmail("a2@b.com");
        user.setPassword("password");

        dao.saveUser(user);

        assertNotNull(user.getId());
        assertTrue(user.getName().equals("Rod"));

        log.debug("removing user...");

        dao.removeUser(user.getId());
        endTransaction();

        try {
            user = dao.getUser(user.getId());
            fail("User found in database");
        } catch (DataAccessException dae) {
            log.debug("Expected exception: " + dae.getMessage());
            assertNotNull(dae);
        }
    }
}
