package com.beelun.shoppro.web;

import java.util.HashMap;
import java.util.Map;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.jmock.Mock;
import org.jmock.MockObjectTestCase;
import org.springframework.beans.MutablePropertyValues;
import org.springframework.context.support.ResourceBundleMessageSource;
import org.springframework.context.support.StaticApplicationContext;
import org.springframework.mock.web.MockHttpServletRequest;
import org.springframework.mock.web.MockHttpServletResponse;
import org.springframework.validation.BindException;
import org.springframework.validation.Errors;
import org.springframework.validation.Validator;
import org.springframework.web.servlet.ModelAndView;

import com.beelun.shoppro.model.User;
import com.beelun.shoppro.service.UserManager;
import com.beelun.shoppro.web.UserFormController;


public class UserFormControllerTest extends MockObjectTestCase {
    private final Log log = LogFactory.getLog(UserFormControllerTest.class);
    private UserFormController c = new UserFormController();
    private MockHttpServletRequest request = null;
    private ModelAndView mv = null;
    private User user = new User();
    private Mock mockManager = null;
    private Mock mockValidator = null;

    protected void setUp() throws Exception {
        super.setUp();
        mockManager = new Mock(UserManager.class);
        
        // manually set properties (dependencies) on userFormController
        c.userManager = (UserManager) mockManager.proxy();
        c.setFormView("userForm");
        
        mockValidator = new Mock(Validator.class);
        c.validator = ((Validator)mockValidator.proxy());
        
        // set context with messages avoid NPE when controller calls 
        // getMessageSourceAccessor().getMessage()
        StaticApplicationContext ctx = new StaticApplicationContext();
        Map<String, String> properties = new HashMap<String, String>();
        properties.put("basename", "messages");
        ctx.registerSingleton("messageSource", ResourceBundleMessageSource.class, 
                              new MutablePropertyValues(properties));
        ctx.refresh();
        c.setApplicationContext(ctx);    
        
        // setup user values
        user.setId(1L);
        user.setName("Matt");
        //user.setLastName("Raible");
    }
    
    public void testEdit() throws Exception {
        log.debug("testing edit...");
        
        // set expected behavior on manager
        mockManager.expects(once()).method("getUserById")
                   .will(returnValue(new User()));
                
        request = new MockHttpServletRequest("GET", "/userform.html");
        request.addParameter("id", user.getId().toString());
        mv = c.handleRequest(request, new MockHttpServletResponse());
        assertEquals("userForm", mv.getViewName());
        
        // verify expectations
        mockManager.verify();
    }

    public void testSave() throws Exception {
        // set expected behavior on manager
        // called by formBackingObject()
        mockManager.expects(once()).method("getUserById")
                   .will(returnValue(user));
        mockManager.expects(once()).method("exists").will(returnValue(false));
        
        mockValidator.expects(once()).method("supports").will(returnValue(true));
        mockValidator.expects(once()).method("validate");

        User savedUser = user;
        savedUser.setName("Updated Last Name");
        // called by onSubmit()
        //mockManager.expects(once()).method("save").with(eq(savedUser));
        mockManager.expects(once()).method("saveUser").will(returnValue(savedUser));
        
        request = new MockHttpServletRequest("POST", "/userform.html");
        request.addParameter("id", user.getId().toString());
        request.addParameter("Name", user.getName());
        //c.setValidator(new org.springmodules.validation.commons.DefaultBeanValidator());
        // c.setValidateOnBinding(false);
        mv = c.handleRequest(request, new MockHttpServletResponse());
        Errors errors = (Errors) mv.getModel().get(BindException.MODEL_KEY_PREFIX + "user");
        assertNull(errors);
        // assertNotNull(request.getSession().getAttribute("message"));// This is removed.
        
        // verify expectations
        mockManager.verify();
        mockValidator.verify();
    }
    
    // We don't support remove any more
//    public void testRemove() throws Exception {
//        // set expected behavior on manager
//        // called by formBackingObject()
//        mockManager.expects(once()).method("getUser")
//                   .will(returnValue(user));
//        // called by onSubmit()
//        mockManager.expects(once()).method("removeUser").with(eq("1"));
//        
//        request = new MockHttpServletRequest("POST", "/userform.html");
//        request.addParameter("delete", "");
//        request.addParameter("id", user.getId().toString());
//        mv = c.handleRequest(request, new MockHttpServletResponse());
//        assertNotNull(request.getSession().getAttribute("message"));
//        
//        // verify expectations
//        mockManager.verify();
//    }   
}
