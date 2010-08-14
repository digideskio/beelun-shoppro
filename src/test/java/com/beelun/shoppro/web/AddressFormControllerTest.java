package com.beelun.shoppro.web;

import java.util.Map;

import org.apache.log4j.Logger;
import org.jmock.Mock;
import org.jmock.MockObjectTestCase;
import org.springframework.mock.web.MockHttpServletRequest;
import org.springframework.mock.web.MockHttpServletResponse;
import org.springframework.security.Authentication;
import org.springframework.security.GrantedAuthority;
import org.springframework.security.GrantedAuthorityImpl;
import org.springframework.security.context.SecurityContextHolder;
import org.springframework.security.userdetails.UserDetails;
import org.springframework.web.servlet.ModelAndView;

import com.beelun.shoppro.helper.FakeAuthenticationToken;
import com.beelun.shoppro.model.Address;
import com.beelun.shoppro.model.User;
import com.beelun.shoppro.service.AddressManager;
import com.beelun.shoppro.service.UserManager;

/**
 * Test 'AddressFormController'
 *
 * @author <a href="mailto:bill@beelun.com">Bill Li</a>
 *
 */
public class AddressFormControllerTest extends MockObjectTestCase {
	private static final Logger log = Logger.getLogger(AddressFormControllerTest.class);
	
	AddressAjaxController addressFormController = new AddressAjaxController();
	
	private Mock mUserManager = null;
	private Mock mAddressManager = null;
	private MockHttpServletRequest request = null;
	private ModelAndView mv = null;
	
    protected void setUp() throws Exception { 
    	mUserManager = new Mock(UserManager.class);
    	mAddressManager = new Mock(AddressManager.class);
    	addressFormController.userManager = (UserManager)mUserManager.proxy();
    	addressFormController.addressManager = (AddressManager) mAddressManager.proxy();
    }

    @SuppressWarnings("unchecked")
	public void testPosotive() throws Exception {
    	User user = new User();
    	user.setId(new Long(1));
    	mUserManager.expects(once()).method("saveUser").will(returnValue(user));
    	
    	Address address = new Address();
    	address.setId(new Long(2));
    	mAddressManager.expects(once()).method("save").will(returnValue(address));
    	
    	// New address
    	request = new MockHttpServletRequest("GET", "/customer/create-update-address.html");
    	request.addParameter("name", "name1");
    	request.addParameter("address", "address1");
    	request.addParameter("zip", "200000");
    	request.addParameter("recipientName", "bill");
    	request.addParameter("phoneNumber", "123");
    	request.addParameter("mobileNumber", "456");
    	
    	// In Spring 3.0.0.M1, class 'TestingAuthenticationToken' will be in the core.  
    	GrantedAuthority[] grantedAuthorities = new GrantedAuthority[]{new GrantedAuthorityImpl("ROLE_CUSTOMER")};
    	UserDetails userDetails = user;
    	Authentication authentication = new FakeAuthenticationToken(userDetails, "password", grantedAuthorities);
    	SecurityContextHolder.getContext().setAuthentication(authentication);
    	    	
        mv = addressFormController.handleAjaxRequest(request, new MockHttpServletResponse());
        assertEquals("ajaxResponse", mv.getViewName());
        Map map = mv.getModel();
        assertTrue(map.get("code").toString().equals("200") == true);
        String result = map.get("result").toString();
        assertTrue(result.equals("<addressId>2</addressId>"));
        
        mUserManager.verify();
        mAddressManager.verify();
        
        log.info("done");
    }
    
    public void testNegative() {
    	log.debug("tdb...");
    }
}
