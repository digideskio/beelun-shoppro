package com.beelun.shoppro.web;

import net.sourceforge.jwebunit.junit.WebTestCase;
import net.sourceforge.jwebunit.html.Table;
import net.sourceforge.jwebunit.html.Row;
import net.sourceforge.jwebunit.html.Cell;

import java.util.List;
import java.util.ResourceBundle;

import org.apache.log4j.Logger;

/**
 * Test user operations
 * TODO: fix deprecation
 * 
 * @author Bill Li(bill@beelun.com)
 *
 */
public class UserWebTest extends WebTestCase {
	private static final Logger log = Logger.getLogger(UserWebTest.class);
	
    private ResourceBundle messages;

    public void setUp() throws Exception {
        //super(name);
    	super.setUp();
        setBaseUrl("http://localhost:25888");
        getTestContext().setResourceBundleName("messages");
        messages = ResourceBundle.getBundle("messages");
        //getTestContext().setLocale(Locale.GERMAN);
        //getTestContext().getWebClient().setHeaderField("Accept-Language", "de");
    }

    public void testWelcomePage() {
        beginAt("/");
        assertTitleEquals("Home page title");
    }

    @SuppressWarnings("deprecation")
	public void testAddUser() {
        beginAt("/userform.html");
        assertTitleKeyMatches("userForm.title");
        setFormElement("name", "Spring");
        setFormElement("email", "aaa@beelun.com");
        submit("save");
        assertTitleKeyMatches("userList.title");
    }

    public void testListUsers() {
        beginAt("/users.html");

        // check that table is present
        assertTablePresent("userList");

        //check that a set of strings are present somewhere in table
        assertTextInTable("userList",
                new String[]{"Spring", "aaa@beelun.com"});
    }

    @SuppressWarnings("deprecation")
	public void testEditUser() {
        beginAt("/userform.html?id=" + getInsertedUserId());
        assertFormElementEquals("name", "Spring");
        submit("save");
        assertTitleKeyMatches("userList.title");
    }

    public void testDeleteUser() {
        beginAt("/userform.html?id=" + getInsertedUserId());
        assertTitleKeyMatches("userForm.title");
        submit("delete");
        assertTitleKeyMatches("userList.title");
    }

    /**
     * Convenience method to get the id of the inserted user
     * Assumes last inserted user is "Spring User"
     * @return last id in the table
     */
    @SuppressWarnings("unchecked")
	protected String getInsertedUserId() {
        beginAt("/users.html");
        assertTablePresent("userList");
        assertTextInTable("userList", "Spring");
        Table t = getTable("userList");
        List<Row> rowList = t.getRows();
        List<Cell> cellList = rowList.get(rowList.size()-1).getCells();      
        String userId = cellList.get(0).getValue();
        log.debug("userId: " + userId);
        return userId;
    }

    protected void assertTitleKeyMatches(String title) {
        assertTitleEquals(messages.getString(title)); 
    }
}
